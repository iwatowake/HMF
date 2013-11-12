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
		
		eExit_Init,
		eExit_Wait,
		
		eChangeState
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
			UI_TimeCounter.Instance.SetEnable(false);
			UI_TentionGauge.Instance.SetEnable(false);
			FadeIn(1.5f);			
			GameObject WakuGeneratorObj = Instantiate( WakuGeneratorPrefab ) as GameObject;
			WakuGeneratorObj.transform.parent = transform;
			WakuGeneratorObj.GetComponent<WakuGenerator>().Init();
			
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
			FadeOut(1.5f);
			GameObject.Find("UI_InGame").SetActive(false);
			state++;
			break;
		case STATE.eExit_Wait:
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
	
	override protected void OnCompleteFade(){
		state++;
	}
}
