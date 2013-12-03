using UnityEngine;
using System.Collections;

public class UI_TentionGauge : SingletonMonoBehaviour<UI_TentionGauge>{
	
	public	UISprite			frame;
	public	UIFilledSprite[]	contents;
	private	int					nowLevel = 0;
	private	int					nowCityLevel = 0;
	
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
		if(Input.GetKeyDown(KeyCode.F))
		{
			SetResult(95.0f);
		}
	}
	
	/* 判定結果をゲージに反映する. 図形の一致率をそのまま入れればＯＫ */
	public	void	SetResult(float result){
		float	fillval = CheckFillVal(result);
		Fill(fillval);
	}
	
	/* 判定結果に応じてゲージの増分を返す */
	private	float	CheckFillVal(float result){
		
		int		round = Mathf.RoundToInt(result);
		
		Debug.Log(round);
		
		// Bad -----------
/*		if(round < 50)
			return -0.15f;
		
		if(round < 52)
			return -0.135f;
			
		if(round < 53)
			return -0.12f;
		
		if(round < 55)
			return -0.105f;
		
		if(round < 59)
			return -0.09f;
		
		if(round < 63)
			return -0.075f;
		
		if(round < 67)
			return -0.06f;
		
		if(round < 71)
			return -0.045f; */
		
		if(round < Define_Rate.Safe)
			return -0.03f;
		
		// Safe ----------
/*		if(round < 77)
			return 0.125f;
		
		if(round < 80)
			return 0.15f;*/
		
		if(round < Define_Rate.Good)
			return 0.25f;
		
		// Good ----------
		if(round < Define_Rate.Excellent)
			return 0.4f;
		
		// Excellent------
/*		if(round < 97)
			return 0.5f;
		
		if(round < 99)
			return 0.7f;*/
		
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
