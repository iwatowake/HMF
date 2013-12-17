using UnityEngine;
using System.Collections;

// 状態管理用
public enum E_STATE{
	Title=0,
	Tutorial,
	InGame,
	Result,
	Ranking,
	GameOver
}

//===========================================
/*!
 *	@brief	状態管理クラス
 *
 *	@date	2013/10/21
 *	@author	Daisuke Kojima
 */
//===========================================
public class StateController : SingletonMonoBehaviour<StateController> {
	
	public		GameObject[]	StateObjectList;	//!< 状態遷移オブジェクト(Inspectorからセット)
	private		StateBase		nowState;			//!< 現在の状態
	
	private		int				iScore = 0;
	
	public		int	score{
		get{return iScore;}
		set{iScore = value;}
	}
	
	void Start(){
		// 初期状態：タイトル
		ChangeState(E_STATE.Title);
		Application.targetFrameRate = 60;
	}
	
	void Update(){
		// 現在の状態を実行
		if(nowState != null)
		{
			nowState.Exec();
		}
	}
	
	//===========================================
	/*!
	 *	@brief	状態を遷移させる
	 *
	 *	@param	newState 次の状態を指定
	 *
	 *	@date	2013/10/21
	 *	@author	Daisuke Kojima
	 */
	//===========================================
	public void ChangeState(E_STATE newState){
		// 現在の状態が空でなければ消えてもらう
		if(nowState != null){
			GameObject oldState = nowState.gameObject;
			Destroy(oldState);
		}
		
		// 指定された状態のプレハブを生成し, 状態クラスを取り出す

		GameObject	go = InstantiateGameObject(StateObjectList[(int)newState]);
		nowState = go.GetSafeComponent<StateBase>();
	}
}