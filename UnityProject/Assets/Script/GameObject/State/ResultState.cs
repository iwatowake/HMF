using UnityEngine;
using System.Collections;

public class ResultState : StateBase {
	
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
	
	public override void Exec ()
	{
		switch(state)
		{
			// タイトル　状態入り
		case STATE.eEnter_Init:
			Debug.Log("Enter_Result");
			state++;
			break;
		case STATE.eEnter_Wait:
			state++;
			break;
			
			// タイトル　入力待ち
		case STATE.eMain_Init:
			state++;
			break;
		case STATE.eMain_Wait:
			if(Input.GetKeyDown(KeyCode.Space))
			{
				state++;
			}
			break;
			
			// タイトル　状態離脱
		case STATE.eExit_Init:
			state++;
			break;
		case STATE.eExit_Wait:
			state++;
			break;
			
			// 次の状態へ
		case STATE.eChangeState:
			StateController.Instance.ChangeState(E_STATE.Ranking);
			break;
		}
	}
	
	protected override void OnDestruct ()
	{
		
	}
}
