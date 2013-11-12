using UnityEngine;
using System.Collections;

public class ResultState : StateBase {
	
	// 状態管理
	enum STATE{
		eEnter_Init = 0,
		eEnter_Wait,
		
		eScoreFadeIn_Init,
		eScoreFadeIn_Wait,
		
		eNewRecordFadeIn_Init,
		eNewRecordFadeIn_Wait,
		
		eNewRecordFadeOut_Init,
		eNewRecordFadeOut_Wait,
		
		eCharsFadeIn_Init,
		eCharsFadeIn_Wait,
		
		eWaitInput_Init,
		eWaitInput_Wait,
		
		eDispNextButton_Init,
		eDispNextButton_Wait,
		
		eExit_Init,
		eExit_Wait,
		
		eChangeState
	}
	
	private STATE	state = STATE.eEnter_Init;	//!< 状態管理用
	
	private bool	isNewRecord = false;
	
	protected override void Start ()
	{
		gameObject.name = "State_Result";
		base.Start ();
	}
	
	public override void Exec ()
	{
		switch(state)
		{
			// 状態入り
		case STATE.eEnter_Init:		
			Debug.Log("Enter_Result");
			FadeIn(1.5f);
			isNewRecord = UI_ResultManager.Instance.SetScore();
			state++;
			break;
		case STATE.eEnter_Wait:
			break;
			
			// Scoreをフェードイン
		case STATE.eScoreFadeIn_Init:
			UI_ResultManager.Instance.lbFdScore.FadeIn(1.0f, 0.0f, "OnCompleteFade", gameObject);
			state++;
			break;
		case STATE.eScoreFadeIn_Wait:
			break;
			
			// NewRecordをフェードイン
		case STATE.eNewRecordFadeIn_Init:
			UI_ResultManager.Instance.spFdNewRecord.FadeIn(1.0f, 1.0f, "OnCompleteFade", gameObject);
			state++;
			break;
		case STATE.eNewRecordFadeIn_Wait:
			break;
			
			// NewRecordをフェードアウト, Scoreを上へ移動
		case STATE.eNewRecordFadeOut_Init:
			UI_ResultManager.Instance.spFdNewRecord.FadeOut(1.0f, 2.0f, "OnCompleteFade", gameObject);
			UI_ResultManager.Instance.lbFdScore.GetComponent<iTweenEvent>().Play();
			state++;
			break;
		case STATE.eNewRecordFadeOut_Wait:
			break;
			
			// 入力用文字をフェードイン
		case STATE.eCharsFadeIn_Init:
			UI_ResultManager.Instance.inputChars.FadeIn(2.0f, "OnCompleteFade", gameObject);
			state++;
			break;
		case STATE.eCharsFadeIn_Wait:
			break;
			
			// 一つ目の文字を入力待ち
		case STATE.eWaitInput_Init:
			state++;
			break;
		case STATE.eWaitInput_Wait:
			if(UI_ResultManager.Instance.inputArea.text.Length > 0)
			{
				state++;
			}
			break;
			
			// Nextボタンを表示, 押され待ち
		case STATE.eDispNextButton_Init:
			UI_ResultManager.Instance.spNextButton.alpha = 1.0f;
			UI_ResultManager.Instance.spNextButton.GetComponent<UI_Buttons>().active = true;
			state++;
			break;
		case STATE.eDispNextButton_Wait:
			break;
			
			// 文字表示と入力をオフ, フェードアウト, 新記録なら名前とスコアをセーブ
		case STATE.eExit_Init:
			UI_ResultManager.Instance.inputChars.OffDisp();
			FadeOut(1.5f);
			state++;
			break;
		case STATE.eExit_Wait:
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
	
	override protected void OnCompleteFade(){
		Debug.Log("oncomplete");
		if(!isNewRecord && state == STATE.eScoreFadeIn_Wait)
		{
			state = STATE.eDispNextButton_Init;
		}else{
			state++;
		}
	}
}
