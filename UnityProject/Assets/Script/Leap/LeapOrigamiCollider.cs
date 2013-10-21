using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeapOrigamiCollider : MonoBehaviour {
	
	private Vector3	HitStartPos;
	private Vector3 HitEndPos;
	private	bool	HitFlg = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if( !HitFlg )	return;
	}
	
	private void OnTriggerEnter (Collider other){
		if( other.gameObject.layer == 8 ){
			HitStartPos = other.collider.ClosestPointOnBounds(transform.position);
			HitFlg = true;
		}
	}
	
	private void OnTriggerExit (Collider other){
		if( HitFlg == true && other.gameObject.layer == 8 ){
			HitEndPos = other.collider.ClosestPointOnBounds(transform.position);
			
			// 折れるかどうか判定.
			Vector3	Vec = HitEndPos - HitStartPos;
			Vector3	MediumPos = HitStartPos + Vec / 2.0f;
			Vector3	Normal = Vector3.Cross( Camera.main.transform.forward, Vec.normalized );
			
			Ray			ray = new Ray();
			RaycastHit 	HitInfo = new RaycastHit();
			ray.direction = Camera.main.transform.forward;
			MediumPos -= ray.direction;
			
			bool CutFlg = false;
			ray.origin = MediumPos + Normal * 0.1f;
			if( other.Raycast( ray, out HitInfo, 100.0f ) ){
				CutFlg = true;
			}
			if( !HitFlg ){
				ray.origin = MediumPos - Normal * 0.1f;
				if( other.Raycast( ray, out HitInfo, 100.0f ) ){
					CutFlg = true;
				}
			}
			
			if( CutFlg ){
			}
		}
	}
}
