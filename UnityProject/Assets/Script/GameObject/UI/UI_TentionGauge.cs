using UnityEngine;
using System.Collections;

public class UI_TentionGauge : SingletonMonoBehaviour<UI_TentionGauge>{
	
	public	UISprite			frame;
	public	UISprite			deco;
	public	UIFilledSprite[]	contents;
	private	int					nowLevel = 1;
	private	int					nowCityLevel = 0;
	private	float				incremental = 0;

	private	const	float		fillSpeed = 0.0025f;
	
	private	bool				inverse = false;
	
	void Start () {
		foreach(UIFilledSprite sp in contents)
		{
			sp.alpha = 0;
			sp.fillAmount = 0.35f;
		}
		contents[1].alpha = 1;
		//contents[0].fillAmount = 0.25f;
	}
	
	public	void	SetEnable(bool b){
		foreach(UIFilledSprite sp in contents)
		{
			sp.enabled = b;
		}
		frame.enabled = b;
		deco.enabled = b;
	}
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			SetResult(Define_Rate.Excellent);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			SetResult(Define_Rate.Good);
		}
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			SetResult(Define_Rate.Safe);
		}
		if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			SetResult(Define_Rate.Safe-1);
		}
		
		Fill();
	}
	
	/* 判定結果をゲージに反映する. 図形の一致率をそのまま入れればＯＫ */
	public	void	SetResult(float result){
		incremental = CheckFillVal(result);
	}
	
	/* 判定結果に応じてゲージの増分を返す */
	private	float	CheckFillVal(float result){
		
		int		round = Mathf.RoundToInt(result);
		
		inverse = false;
		
		// Bad -----------
		if(round < Define_Rate.Safe)
		{
			iTweenEvent.GetEvent(gameObject, "Shake").Play();
			inverse = true;
			return 0.08f;
		}
		
		// Safe ----------
		if(round < Define_Rate.Good)
			return 0.0f;

		iTweenEvent.GetEvent(gameObject, "Scaling_to").Play();
		iTweenEvent.GetEvent(gameObject, "Scaling_from").Play();
		
		// Good ----------
		if(round < Define_Rate.Excellent)
			return 0.02f;
		
		// Excellent------	
		if(round < 100)
			return 0.0375f;
		
		if(round < 101)
			return 0.1f;
		
		return 0.0f;
	}
	
	public	bool	isDead(){
		if(contents[0].fillAmount <= 0)
		{
			return true;
		}else{
			return false;
		}
	}

	void Fill(){
		if(incremental == 0)	return;
		
		incremental -= fillSpeed;
		if(incremental < 0)	incremental = 0;
		
		if(inverse)
		{
			foreach(UIFilledSprite sp in contents)
			{
				sp.fillAmount -= fillSpeed;
			}
		}else{
			foreach(UIFilledSprite sp in contents)
			{
				sp.fillAmount += fillSpeed;
			}
		}
		
		float	unit = 1.0f / contents.Length;
		
		if(contents[nowLevel].fillAmount >= unit * (nowLevel+1))
		{
/*			if(nowLevel == nowCityLevel)
			{
				nowCityLevel ++;
				NaviCamera.Instance.LookTownPauseTween();
				Game_CityLayer.Instance.CityLayerEnable((int)nowCityLevel, true);
			}*/
			
			if(nowLevel < contents.Length-1)
			{
				contents[nowLevel].GetComponent<UI_FilledSprite_Fade>().FadeOut(0.5f);
				contents[nowLevel+1].GetComponent<UI_FilledSprite_Fade>().FadeIn(0.5f);
				nowLevel ++;
				
				return;
			}
		}
		
		if(contents[nowLevel].fillAmount < unit * nowLevel)
		{
			if(nowLevel > 0)
			{
				contents[nowLevel].GetComponent<UI_FilledSprite_Fade>().FadeOut(0.5f);
				contents[nowLevel-1].GetComponent<UI_FilledSprite_Fade>().FadeIn(0.5f);
				nowLevel--;
			}
		}
		
	}
}
