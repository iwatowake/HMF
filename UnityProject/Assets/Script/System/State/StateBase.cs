using UnityEngine;
using System.Collections;

public class StateBase : MonoBehaviour_Extends {
	
	private bool	isQuit = false;
	
	public virtual void Exec () {
	
	}
	
	protected virtual void OnDestruct(){
		
	}
	
	void OnApplicationQuit(){
		isQuit = true;
	}
	
	void OnDestroy(){
		if(!isQuit)
		{
			OnDestruct();
		}
	}
}
