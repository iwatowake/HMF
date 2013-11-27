using UnityEngine;
using System.Collections;

public class LineEffect : MonoBehaviour {
	
	public Vector3 targetPositionStart; 
	public Vector3 targetPositionEnd;
	public float	moveTime = 0.1f;
		
	Vector3 oldPos;
	
	// Use this for initialization
	void Start () {
		MoveToTargetPositionEnd();
		oldPos = targetPositionEnd;
	}
	
	// Update is called once per frame
	void Update () {
//		if(oldPos != targetPositionEnd)
			gameObject.particleSystem.renderer.enabled = false;
//		else
//			gameObject.particleSystem.renderer.enabled = true;
		
		oldPos = targetPositionEnd;
		gameObject.GetComponent<LineRenderer>().SetPosition(0,targetPositionStart);
		gameObject.GetComponent<LineRenderer>().SetPosition(1,targetPositionEnd);
	}
	
	void End()
	{
		iTween.Stop(gameObject);
	}
		
	void MoveToTargetPositionEnd()
	{
//		gameObject.particleSystem.renderer.enabled = true;
		iTween.MoveTo(gameObject,iTween.Hash("position", targetPositionEnd,"time", moveTime,"oncomplete","MoveToTargetPositionStart","easetype",iTween.EaseType.linear,"islocal",true));
	}
	void MoveToTargetPositionStart()
	{
//		gameObject.particleSystem.renderer.enabled = true;
		iTween.MoveTo(gameObject,iTween.Hash("position", targetPositionStart,"time", moveTime,"oncomplete","MoveToTargetPositionEnd","easetype",iTween.EaseType.linear,"islocal",true));		
	}
	
}
