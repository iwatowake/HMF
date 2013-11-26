using UnityEngine;
using System.Collections;

public class GameOverState : StateBase {
	
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
	
	private STATE	state = STATE.eEnter_Init;	//!< 状態管理用
	private E_STATE	nextState;
	
	protected override void Start ()
	{
		base.Start ();
		gameObject.name = "State_GameOver";
	}
	
	public override void Exec ()
	{
		switch(state)
		{
			//　状態入り
		case STATE.eEnter_Init:
			Debug.Log("Enter_GameOver");
			Game_CityLayer.Instance.CityLayerEnable(0,false);
			FadeIn(1.5f);
			state++;
			break;
		case STATE.eEnter_Wait:
			break;
			
			// 入力待ち
		case STATE.eMain_Init:
			state++;
			break;
		case STATE.eMain_Wait:
			break;
			
			// 状態離脱
		case STATE.eExit_Init:
			FadeOut(1.5f);
			state++;
			break;
		case STATE.eExit_Wait:
			break;
			
			// 次の状態へ
		case STATE.eChangeState:
			StateController.Instance.ChangeState(nextState);
			break;
		}
	}
	
	protected override void OnCompleteFade ()
	{
		state++;
	}
	
	protected override void OnDestruct ()
	{
		
	}
	
	private	void	OnRetryPressed(){
		nextState = E_STATE.InGame;
		state++;
	}
	
	private	void	OnTitlePressed(){
		nextState = E_STATE.Title;
		state++;
	}
}
