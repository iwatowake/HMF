using UnityEngine;
using System.Collections;

public class InGameState : StateBase {
	
	// 状態管理
	enum STATE{
		eEnter_Init = 0,
		eEnter_Wait,
		
		eMain_Init,
		eMain_Wait,
		
		eExit_Init,
		eExit_Wait,
		
		eChangeState
	}
	
	private STATE	state		= STATE.eEnter_Init;	//!< 状態管理用
	private float	tention		= 0.1f;					//!< テンション(0.1f～1.0f)
	private	float	leftSec		= 180.0f;				//!< 残り時間(秒)
	private float	timer		= 0.0f;
	
	protected override void Start ()
	{
		base.Start ();
	}
	
	public override void Exec ()
	{
		switch(state)
		{
			// インゲーム　状態入り
		case STATE.eEnter_Init:
			fade = GameObject.Find("Fade").GetComponent<Fade>();
			fade.gameObject.renderer.material.color = Color.white;
			Debug.Log("Enter_InGame");
			Game_CityLayer.Instance.CityLayerEnable(0,true);
			UI_TimeCounter.Instance.SetEnable(false);
			UI_TentionGauge.Instance.SetEnable(false);
			fade.Tween_FadeIn(gameObject, "OnCompleteFade", 1.5f);
			state++;
			break;
		case STATE.eEnter_Wait:
			break;
			
			// インゲーム　入力待ち
		case STATE.eMain_Init:
			UI_TimeCounter.Instance.SetEnable(true);
			UI_TentionGauge.Instance.SetEnable(true);
			state++;
			break;
		case STATE.eMain_Wait:
			UI_TimeCounter.Instance.Exec();
			
			if(Input.GetKeyDown(KeyCode.A))
			{
				UI_TentionGauge.Instance.SetResult(75);
				UI_TimeCounter.Instance.SetResult(75);
			}
			if(Input.GetKeyDown(KeyCode.S))
			{
				UI_TentionGauge.Instance.SetResult(100);
				UI_TimeCounter.Instance.SetResult(100);
			}
			
			if(Input.GetKeyDown(KeyCode.Space))
			{
				state++;
			}
			break;
			
			// インゲーム　状態離脱
		case STATE.eExit_Init:
			state++;
			break;
		case STATE.eExit_Wait:
			state++;
			break;
			
			// 次の状態へ
		case STATE.eChangeState:
			StateController.Instance.ChangeState(E_STATE.Result);
			break;
		}
	}
	
	protected override void OnDestruct ()
	{
		
	}
	
	
	//! 状態入り　初期化
	private void Enter_Init(){
		
	}
	//! 状態入り　待機
	private void Enter_Wait(){
		
	}
	
	
	//! メイン処理　初期化
	private void Main_Init(){
		
	}
	//! メイン処理　待機
	private void Main_Wait(){
		
	}
	
	private void OnCompleteFade(){
		state++;
	}
}
