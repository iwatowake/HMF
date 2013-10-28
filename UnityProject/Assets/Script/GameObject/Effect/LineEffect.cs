using UnityEngine;
using System.Collections;

public class LineEffect : MonoBehaviour {
	
	public Vector3 targetPositionStart 
	{
    	set{this.targetPositionStart = value;}
    	get{return this.targetPositionStart;}
  	}
	public Vector3 targetPositionEnd 
	{
    	set{this.targetPositionEnd = value;}
    	get{return this.targetPositionEnd;}
  	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void MoveToTargetPositionEnd()
	{
		iTween.MoveTo(gameObject,iTween.Hash("position", targetPositionStart,"time", 1.0f,"oncomplete","MoveToTargetPositionStart"));		
	}
	void MoveToTargetPositionStart()
	{
		iTween.MoveTo(gameObject,iTween.Hash("position", targetPositionStart,"time", 1.0f,"oncomplete","MoveToTargetPositionEnd"));		
	}
	
}
