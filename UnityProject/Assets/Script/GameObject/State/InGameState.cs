using UnityEngine;
using System.Collections;

public class InGameState : StateBase {
	
	public GameObject WakuGeneratorPrefab;
	
	// 状態管理
	enum STATE{
		eEnter_Init = 0,
		eEnter_Wait,
		
		eMain_Init,
		eMain_Wait,
		
		eGameOver_Init,
		eGameOver_Wait,
		
		eGameClear_Init,
		eGameClear_Wait,
		
		eExit_Init,
		eExit_Wait,
		
		eChangeState_ToGameOver,
		eChangeState_ToResult
	}
	
	private STATE	state		= STATE.eEnter_Init;	//!< 状態管理用
	
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
			CRI_SoundManager_2D.Instance.PlayBGM(BGM_ID.INGAME);

			Debug.Log("Enter_InGame");
			Game_CityLayer.Instance.CityLayerEnable(0,true);
//			UI_TimeCounter.Instance.SetEnable(false);
			UI_TentionGauge.Instance.SetEnable(false);
			FadeIn(1.5f);			
			GameObject WakuGeneratorObj = Instantiate( WakuGeneratorPrefab ) as GameObject;
			WakuGeneratorObj.transform.parent = transform;
			WakuGeneratorObj.GetComponent<WakuGenerator>().Init();
			
			state++;
			break;
		case STATE.eEnter_Wait:
			Debug.Log("enter_wait");
			break;
			
			// インゲーム　入力待ち
		case STATE.eMain_Init:
			Debug.Log("main_init");
//			UI_TimeCounter.Instance.SetEnable(true);
			UI_TentionGauge.Instance.SetEnable(true);
			state++;
			break;
		case STATE.eMain_Wait:
//			UI_TimeCounter.Instance.Exec();
			if(UI_TentionGauge.Instance.isDead())
			{
				state = STATE.eGameOver_Init;
			}
			break;
			
		case STATE.eGameOver_Init:
			state++;
			break;
		case STATE.eGameOver_Wait:
			state = STATE.eExit_Init;
			break;
			
		case STATE.eGameClear_Init:
			state++;
			break;
		case STATE.eGameClear_Wait:
			state = STATE.eExit_Init;
			break;
			
			// インゲーム　状態離脱
		case STATE.eExit_Init:
			FadeOut(1.5f);
			GameObject.Find("UI_InGame").SetActive(false);
			state++;
			break;
		case STATE.eExit_Wait:
			break;
			
			// 次の状態へ
		case STATE.eChangeState_ToGameOver:
			StateController.Instance.ChangeState(E_STATE.GameOver);
			break;
			// 次の状態へ
		case STATE.eChangeState_ToResult:
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
	
	override protected void OnCompleteFade(){
		state++;
	}
}
