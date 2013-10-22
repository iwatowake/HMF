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
	
	
	public override void Exec ()
	{
		switch(state)
		{
			// タイトル　状態入り
		case STATE.eEnter_Init:
			Debug.Log("Enter_InGame");
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
	
	
	//! 正解率に応じてテンションと残り時間を増減
	public void UpdateUIbyResult(float percent){
		// テーブルを参照する
	}
	
	//! テンション増減
	public void SetTention(float t){
		tention += t;
		tention = Mathf.Clamp01(tention);
		
		// UIに反映
		
		if(t>0){
			// なんか演出	
		}else if(t<0){
			// なんか演出	
		}
	}
	
	//! 残り時間増減
	public void SetSec(float s){
		leftSec += s;
		
		// UIに反映
		
		if(s>0){
			// なんか演出	
		}else if(s<0){
			// なんか演出	
		}
	}
}
