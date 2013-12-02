using UnityEngine;
using System.Collections;

public class UI_TimeCounter : SingletonMonoBehaviour<UI_TimeCounter> {
	
	public	enum TIMERSTATE 
	{
		eStop_Init = 0,
		eStop_Wait,
		
		eStart_Init,
		eStart_Wait,
		
		eExec_Init,
		eExec_Wait,
		
		eOver_Init,
		eOver_Wait,
		
		eEnumEnd
	}
	
	public			UILabel label_s;
	public			UILabel	label_ms;
	private			float	counter = 0;					//!< 残り時間カウント(秒)

	private	const	float	SC_COUNTER_INITIALIZE = 60.0f;	//!< 残り時間初期値
	private	const	float		SC_COUNTER_MAX = 5940.0f;
	private			bool		alert = false;
	public			GameObject	effect_TimeIncrease;
	private			TIMERSTATE	state = TIMERSTATE.eStop_Init;
	
	private			int		s = 0;
	private			int		ms = 0;
	
	public			bool	timeOver = false;
	
	/* 残り時間のゲッタ */
	public	float	leftTime{
		get{return counter;}
		set{counter = value;}
	}
	
	void Start () {
		counter = SC_COUNTER_INITIALIZE;
		label_s.color = label_ms.color = Color.white;
		label_s.alpha = label_ms.alpha = 0;
	}
		
	public	void SetEnable(bool b){
		label_s.enabled = label_ms.enabled = b;
	}
	
	/* 時間をカウント */
	void Update(){
		
		switch(state)
		{
		case TIMERSTATE.eStop_Init:
			counter = SC_COUNTER_INITIALIZE;
			alert = false;
			label_s.text	= SC_COUNTER_INITIALIZE.ToString("00");
			label_ms.text	= "00";
			state++;
			break;
		case TIMERSTATE.eStop_Wait:
			break;
			
		case TIMERSTATE.eStart_Init:

			counter = SC_COUNTER_INITIALIZE;
			alert = false;
			label_s.text	= SC_COUNTER_INITIALIZE.ToString("00");
			label_ms.text	= "00";
			InstantiateGameObjectAsChild(effect_TimeIncrease, new Vector3(470, 285, 10), true);
			label_s.color = label_ms.color = Color.white;
			label_s.alpha = label_ms.alpha = 0;
			Tween_FadeIn();
			state++;
			break;
		case TIMERSTATE.eStart_Wait:
			break;
			
		case TIMERSTATE.eExec_Init:
			state++;
			break;
		case TIMERSTATE.eExec_Wait:
			counter -= Time.deltaTime;
			
			if(counter <= 0)
			{
				counter = 0;
				timeOver = true;
				state++;
			}
			
			s  = Mathf.FloorToInt(counter);
			ms = Mathf.FloorToInt((counter - s) * 100.0f);
			label_s.text	= s.ToString("00");
			label_ms.text	= ms.ToString("00");
			break;
			
		case TIMERSTATE.eOver_Init:
			Tween_FadeOut();
			state++;
			break;
		case TIMERSTATE.eOver_Wait:
			break;
			
		case TIMERSTATE.eEnumEnd:
			state = TIMERSTATE.eStop_Init;
			break;
		}		

	}
	

	public	void StartTimer(){

		state = TIMERSTATE.eStart_Init;
		timeOver = false;
	}
	
	public	void StopTimer(){
		if(state == TIMERSTATE.eExec_Wait)
			state = TIMERSTATE.eOver_Init;
	}
	
	private void Tween_Alert(){
		iTween.StopByName("alert_stop");
		
		Hashtable ht = new Hashtable();
		
		ht.Add("name", "alert");
		ht.Add("from", 1.0f);
		ht.Add("to", 0.0f);
		ht.Add("time", 0.5f);
		ht.Add("onupdate", "UpdateHandler_Alert");
		ht.Add("loopType", iTween.LoopType.pingPong);
		
		iTween.ValueTo(gameObject, ht);
	}
	
	private void Tween_StopAlert(){
		iTween.StopByName("alert");
		
		Hashtable ht = new Hashtable();
		
		ht.Add("name", "alert_stop");
		ht.Add("from", label_s.color.g);
		ht.Add("to", 1.0f);
		ht.Add("time", 0.25f);
		ht.Add("onupdate", "UpdateHandler_Alert");
		ht.Add("loopType", iTween.LoopType.none);
		
		iTween.ValueTo(gameObject, ht);
	}
	
	private void Tween_FadeIn(){
		Hashtable ht = new Hashtable();
		
		ht.Add("name", "fadein");
		ht.Add("from", 0.0f);
		ht.Add("to", 1.0f);
		ht.Add("time", 3.0f);
		ht.Add("onupdate", "UpdateHandler_Alpha");
		ht.Add("oncomplete", "OnCompleteTween");
		
		iTween.ValueTo(gameObject, ht);
	}
	
	private void Tween_FadeOut(){
		Hashtable ht = new Hashtable();
		
		ht.Add("name", "fadein");
		ht.Add("from", 1.0f);
		ht.Add("to", 0.0f);		ht.Add("time", 0.25f);
		ht.Add("onupdate", "UpdateHandler_Alpha");
		ht.Add("oncomplete", "OnCompleteTween");
		
		iTween.ValueTo(gameObject, ht);
	}
	
	private void UpdateHandler_Alert(float arg){
//		label_m.color = new Color(arg,0,0,1);
		label_s.color = new Color(1,arg,arg,1);
		label_ms.color = new Color(1,arg,arg,1);
	}	
	
	private void UpdateHandler_Alpha(float arg){
		label_s.alpha = arg;
		label_s.effectColor.SetAlpha(arg);
		label_ms.alpha = arg;
		label_ms.effectColor.SetAlpha(arg);
	}
	
	private void OnCompleteTween(){
		state++;
	}
}
