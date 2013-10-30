using UnityEngine;
using System.Collections;

public class UI_TentionGauge : SingletonMonoBehaviour<UI_TentionGauge>{
	
	public	UIFilledSprite	shinness;
	public	UISprite		frame;
	private	UIFilledSprite	sprite;
	
	void Start () {
		sprite = GetComponent<UIFilledSprite>();
		sprite.fillAmount = 0.125f;
	}
	
	// Debug
	void Update(){

	}
	
	public	void	SetEnable(bool b){
		sprite.enabled = shinness.enabled = frame.enabled = b;
	}
	
	/* 判定結果をゲージに反映する. 図形の一致率をそのまま入れればＯＫ */
	public	void	SetResult(float result){
		float	fillval = CheckFillVal(result);
		Fill(fillval);
		Tween_Shine(fillval*8);
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
		float	oldVal = Mathf.Floor(((sprite.fillAmount*1000.0f)*8.0f)/1000.0f);
		sprite.fillAmount += fillVal;
		
		float	newVal = Mathf.Floor(((sprite.fillAmount*1000.0f)*8.0f)/1000.0f);
		
		if(newVal > oldVal)
		{
			Game_CityLayer.Instance.CityLayerEnable((int)newVal-1,true);
		}
	}
	
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
}
