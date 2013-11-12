using UnityEngine;
using System.Collections;

public class UI_TimeCounter : SingletonMonoBehaviour<UI_TimeCounter> {
	
	public			UILabel	label_m;
	public			UILabel label_s;
	public			UILabel	label_ms;
	private			float	counter = 0;					//!< 残り時間カウント(秒)
	private	const	float	SC_COUNTER_INITIALIZE = 60.0f;	//!< 残り時間初期値
	private	const	float		SC_COUNTER_MAX = 5940.0f;
	private			bool		alert = false;
	public			GameObject	effect_TimeIncrease;
	
	/* 残り時間のゲッタ */
	public	float	leftTime{
		get{return counter;}
		set{counter = value;}
	}
	
	void Start () {
		counter = SC_COUNTER_INITIALIZE;
	}
	
	void Update() {
		
	}
	
	public	void SetEnable(bool b){
		label_m.enabled = label_s.enabled = label_ms.enabled = b;
	}
	
	/* 時間をカウント */
	public	bool Exec(){
		bool timeOver = false;
		
		if(counter > 0)
		{
			counter -= Time.deltaTime;
		}
		
		if(counter < 46.0f && !alert)
		{
			alert = true;
			Tween_Alert();
		}
		
		if(counter >= 46.0f && alert)
		{
			alert = false;
			Tween_StopAlert();
		}
		
		if(counter < 0)
		{
			counter = 0;
			timeOver = true;
		}
		
		int m  = Mathf.FloorToInt(counter/60);
		int s  = Mathf.FloorToInt(counter-m*60);
		int ms = Mathf.FloorToInt((counter - (s + m*60)) * 100.0f);
		
		label_m.text	= m.ToString("00");//+" : ";
		label_s.text	= s.ToString("00");//+" : ";
		label_ms.text	= ms.ToString("00");
		
		//label.text = m.ToString("00") + " : " + s.ToString("00") + " : " + ms.ToString("00");
		
		return timeOver;
	}
	
	public	void SetResult(float result)
	{
		int round = Mathf.RoundToInt(result);
		
		if(round < 75)
		{
			AddTime(10);
			return;
		}
		
		if(round < 80)
		{
			AddTime(20);
			return;
		}
		
		if(round < 85)
		{
			AddTime(25);
			return;
		}
		
		if(round < 87)
		{
			AddTime(28);
			return;
		}
		
		if(round < 90)
		{
			AddTime(30);
			return;
		}
		
		if(round < 92)
		{
			AddTime(35);
			return;
		}
		
		if(round < 95)
		{
			AddTime(40);
			return;
		}
		
		if(round < 98)
		{
			AddTime(45);
			return;
		}
		
		if(round < 100)
		{
			AddTime(50);
			return;
		}
		
		if(round < 101)
		{
			AddTime(60);
			return;
		}
	}
	
	private	void AddTime(float val)
	{
		counter += val;
		InstantiateGameObjectAsChild(effect_TimeIncrease, new Vector3(470, 285, 0), true);
	}
	
	private void Tween_Alert(){
		iTween.StopByName("alert_stop");
		
		Hashtable ht = new Hashtable();
		
		ht.Add("name", "alert");
		ht.Add("from", 0.0f);
		ht.Add("to", 1.0f);
		ht.Add("time", 0.5f);
		ht.Add("onupdate", "UpdateHandler");
		ht.Add("loopType", iTween.LoopType.pingPong);
		
		iTween.ValueTo(gameObject, ht);
	}
	
	private void Tween_StopAlert(){
		iTween.StopByName("alert");
		
		Hashtable ht = new Hashtable();
		
		ht.Add("name", "alert_stop");
		ht.Add("from", label_m.color.r);
		ht.Add("to", 0.0f);
		ht.Add("time", 0.25f);
		ht.Add("onupdate", "UpdateHandler");
		ht.Add("loopType", iTween.LoopType.none);
		
		iTween.ValueTo(gameObject, ht);
	}
	
	private void UpdateHandler(float arg){
		label_m.color = new Color(arg,0,0,1);
		label_s.color = new Color(arg,0,0,1);
		label_ms.color = new Color(arg,0,0,1);
	}
}
