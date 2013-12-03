using UnityEngine;
using System.Collections;

public class UI_TentionGauge : SingletonMonoBehaviour<UI_TentionGauge>{
	
	public	UISprite			frame;
	public	UIFilledSprite[]	contents;
	private	int					nowLevel = 0;
	private	int					nowCityLevel = 0;
	private	float				incremental = 0;

	private	const	float		fillSpeed = 0.01f;
	
	private	bool				inverse = false;
	
	void Start () {
		foreach(UIFilledSprite sp in contents)
		{
			sp.fillAmount = 0.0f;
		}
		contents[0].fillAmount = 0.5f;
	}
	
	public	void	SetEnable(bool b){
		foreach(UIFilledSprite sp in contents)
		{
			sp.enabled = b;
		}
	}
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			SetResult(95.0f);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			SetResult(50.0f);
		}
		
		Fill();
	}
	
	/* 判定結果をゲージに反映する. 図形の一致率をそのまま入れればＯＫ */
	public	void	SetResult(float result){
//		float	fillval = CheckFillVal(result);
		incremental = CheckFillVal(result);
//		Fill(fillval);
	}
	
	/* 判定結果に応じてゲージの増分を返す */
	private	float	CheckFillVal(float result){
		
		int		round = Mathf.RoundToInt(result);
		
		inverse = false;
		
		// Bad -----------
		if(round < Define_Rate.Safe)
		{
			inverse = true;
			return 0.03f;
		}
		
		if(round < Define_Rate.Good)
			return 0.25f;
		
		// Good ----------
		if(round < Define_Rate.Excellent)
			return 0.4f;
		
		// Excellent------		
		if(round < 100)
			return 0.6f;
		
		if(round < 101)
			return 1.0f;
		
		return 0.0f;
	}
	
	/* ゲージの現在の量に応じて建物を生やす */
	private void	Fill(float fillVal){
		
		float newVal = contents[nowLevel].fillAmount + fillVal;
		float diff	 = 0;
		
		if(newVal < 0)
		{
			contents[nowLevel].fillAmount = 0;
			if(nowLevel > 0)
			{
				contents[nowLevel-1].fillAmount += newVal;
				nowLevel--;
			}
		}else	if(newVal > 1){
			if(nowLevel == nowCityLevel)
			{
				nowCityLevel++;
				NaviCamera.Instance.LookTownPauseTween();
				Game_CityLayer.Instance.CityLayerEnable((int)nowCityLevel, true);
			}
			
			contents[nowLevel].fillAmount = 1;
			if(nowLevel < contents.Length -1)
			{
				contents[nowLevel+1].fillAmount += newVal - 1;
				nowLevel++;
			}
		}else{
			contents[nowLevel].fillAmount += fillVal;
		}
		
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
			contents[nowLevel].fillAmount -= fillSpeed;
		}else{
			contents[nowLevel].fillAmount += fillSpeed;
		}
		
		if(contents[nowLevel].fillAmount >= 1)
		{
			if(nowLevel == nowCityLevel)
			{
				nowCityLevel ++;
				NaviCamera.Instance.LookTownPauseTween();
				Game_CityLayer.Instance.CityLayerEnable((int)nowCityLevel, true);
			}
			
			if(nowLevel < contents.Length-1)
			{				
				nowLevel ++;
				
				return;
			}
		}
		
		if(contents[nowLevel].fillAmount == 0)
		{
			if(nowLevel > 0)
			{
				nowLevel --;
			}
		}
	}
}
