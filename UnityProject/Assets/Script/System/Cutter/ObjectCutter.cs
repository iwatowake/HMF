using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectCutter : MonoBehaviour {

	private		GameObject	HitObj = null;
	private 	Vector3 	hitpoint1;
	private 	Vector3 	hitpoint2;
	
	
	void Update () {
	}
	
	
	// 左右判定.
	private	bool GetSide ( Vector3 CutVec, Vector3 PointVec ){
		if( (CutVec.z * PointVec.x - CutVec.x * PointVec.z) < 0.0f ){
			return true;
		}
		return false;
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if( other.gameObject.layer == 8 ){
			hitpoint1 = other.collider.ClosestPointOnBounds(transform.position);
			HitObj = other.gameObject;
		}
	}
	
	
	private void OnTriggerExit(Collider other)
	{
		if(HitObj != other.gameObject || other.gameObject.layer != 8)	return;
		
		hitpoint2 = other.collider.ClosestPointOnBounds(transform.position);
		
		OrigamiCutter.Cut( other.gameObject, hitpoint1, hitpoint2 );
		// レイヤー変更.
		other.gameObject.layer = 9;
		
	}
}
