using UnityEngine;
using System.Collections;

public class UI_TentionGauge : SingletonMonoBehaviour<UI_TentionGauge>{
	
//	public	UIFilledSprite	shinness;
	public	UISprite			frame;
	public	UIFilledSprite[]	contents;
	private	int					nowLevel = 0;
	private	int					nowCityLevel = 0;
//	private	UIFilledSprite		sprite;
	
	void Start () {
//		sprite = GetComponent<UIFilledSprite>();
//		sprite.fillAmount = 0.125f;
		foreach(UIFilledSprite sp in contents)
		{
			sp.fillAmount = 0.0f;
		}
	}
	
	public	void	SetEnable(bool b){
//		sprite.enabled = shinness.enabled = frame.enabled = b;
//		sprite.enabled = b;
		foreach(UIFilledSprite sp in contents)
		{
			sp.enabled = b;
		}
	}
	
	/* 判定結果をゲージに反映する. 図形の一致率をそのまま入れればＯＫ */
	public	void	SetResult(float result){
		float	fillval = CheckFillVal(result);
		Fill(fillval);
//		Tween_Shine(fillval*8);
	}
	
	/* 判定結果に応じてゲージの増分を返す */
	private	float	CheckFillVal(float result){
		Debug.Log(result);
		
		int		round = Mathf.RoundToInt(result);
		
		Debug.Log(round);
		
		if(round < 75)
		{
			return 0.05f;
		}
		
		if(round < 80 && round > 74)
		{
			return 0.1f;
		}
		
		if(round < 85)
		{
			return 0.15f;
		}
		
		if(round < 90)
		{
			return 0.2f;
		}
		
		if(round < 95)
		{
			return 0.25f;
		}
		
		if(round < 101)
		{
			return 0.3f;
		}
		
		return 0.0f;
	}
	
	/* ゲージの現在の量に応じて建物を生やす */
	private void	Fill(float fillVal){
		
		float newVal = contents[nowLevel].fillAmount + fillVal;
		float diff	 = 0;
		
		if(newVal < 0)
		{
			contents[nowLevel].fillAmount = 0;
			contents[nowLevel-1].fillAmount += newVal;
			nowLevel--;
		}else	if(newVal > 1){
			if(nowLevel == nowCityLevel)
			{
				nowCityLevel++;
				NaviCamera.Instance.LookTownPauseTween();
				Game_CityLayer.Instance.CityLayerEnable((int)nowLevel, true);
			}
			
			contents[nowLevel].fillAmount = 1;
			contents[nowLevel+1].fillAmount += newVal - 1;
			nowLevel++;
		}else{
			contents[nowLevel].fillAmount += fillVal;
		}
		
	}
/*	
	private void Tween_Shine(float max){
		iTween.StopByName("shine_inverse");
		
		Hashtable ht = new Hashtable();
		
		ht.Add("name", "shine");
		ht.Add("from", shinness.color.a);
		ht.Add("to", max);
		ht.Add("time", 0.1f);
		ht.Add("onupdate", "UpdateHandler");
		ht.Add("oncomplete", "Tween_Shine_Inverse");
		
		iTween.ValueTo(gameObject, ht);
	}
	
	private void Tween_Shine_Inverse(){
		Hashtable ht = new Hashtable();
		
		ht.Add("name", "shine_inverse");
		ht.Add("from", shinness.color.a);
		ht.Add("to", 0.0f);
		ht.Add("time", 0.25f);
		ht.Add("onupdate", "UpdateHandler");
		
		iTween.ValueTo(gameObject, ht);
	}
	
	private void UpdateHandler(float arg){
		shinness.color = new Color(1,1,1,arg);
	}
	*/
}
