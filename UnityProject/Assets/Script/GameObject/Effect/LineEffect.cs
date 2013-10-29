using UnityEngine;
using System.Collections;

public class LineEffect : MonoBehaviour {
	
	public Vector3 targetPositionStart; 
	public Vector3 targetPositionEnd;
	
	// Use this for initialization
	void Start () {
		gameObject.particleSystem.renderer.enabled = false;
		targetPositionStart = new Vector3(-10,0,0);
		targetPositionEnd = new Vector3(10,0,0);
		MoveToTargetPositionEnd();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void MoveToTargetPositionEnd()
	{
		gameObject.particleSystem.renderer.enabled = true;
		iTween.MoveTo(gameObject,iTween.Hash("position", targetPositionEnd,"time", 1.0f,"oncomplete","MoveToTargetPositionStart","easetype",iTween.EaseType.linear));
	}
	void MoveToTargetPositionStart()
	{
		gameObject.particleSystem.renderer.enabled = true;
		iTween.MoveTo(gameObject,iTween.Hash("position", targetPositionStart,"time", 1.0f,"oncomplete","MoveToTargetPositionEnd","easetype",iTween.EaseType.linear));		
	}
	
}
