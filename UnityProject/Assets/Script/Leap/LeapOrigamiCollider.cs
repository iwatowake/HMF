using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeapOrigamiCollider : MonoBehaviour {
	
	public	GameObject		PointLoopParticlePrefab;
	public	GameObject		PointOneShotParticlePrefab;
	public	GameObject		ContactParticlePrefab;
	private	GameObject[]	PointParticle = new GameObject[2]{ null, null };
	private	GameObject		HitObj = null;
	private Vector3			HitStartPos;
	private Vector3 		HitEndPos;
	private	bool			HitFlg = false;
	private OrigamiController OrigamiControllerScript;

	// Use this for initialization
	void Start () {
		OrigamiControllerScript = GameObject.Find("OrigamiController").gameObject.GetComponent<OrigamiController>();
	}
	
	// Update is called once per frame
	void Update () {
		if( HitObj == null )	return;
		if( HitObj.layer == (int)LayerEnum.layer_OrigamiCut && !HitFlg ){
			if( PointParticle[0] != null ){
				Destroy( PointParticle[0] );
				PointParticle[0] = null;
			}
			if( PointParticle[1] != null ){
				Destroy( PointParticle[1] );
				PointParticle[1] = null;
			}
		}
	}
	
	private void OnTriggerEnter (Collider other){
		if( enabled == false ) return;
		if( other.gameObject.layer == (int)LayerEnum.layer_OrigamiCut ){
			if( PointParticle[0] != null ){
				Destroy( PointParticle[0] );
				PointParticle[0] = null;
			}
			HitObj = other.gameObject;
			HitStartPos = other.collider.ClosestPointOnBounds(transform.position);
			PointParticle[0] = Instantiate( PointLoopParticlePrefab, HitStartPos, Quaternion.identity ) as GameObject;
			HitFlg = true;
		}
	}
	
	private void OnTriggerExit (Collider other){
		if( enabled == false ) return;
		if( other.gameObject.layer == (int)LayerEnum.layer_OrigamiCut && HitFlg ){
			HitEndPos = other.collider.ClosestPointOnBounds(transform.position);
			
			// 折れるかどうか判定.
			Vector3	Vec = HitEndPos - HitStartPos;
			Vector3	MediumPos = HitStartPos + Vec / 2.0f;
			Vector3	MediumPos2 = MediumPos;
			Vector3	Normal = Vector3.Cross( Camera.main.transform.forward, Vec.normalized );
			
			Ray			ray = new Ray();
			RaycastHit 	HitInfo = new RaycastHit();
			float		RayLength = 10.0f;
			float		Offset = 0.3f;
			ray.direction = Camera.main.transform.forward;
			MediumPos -= ray.direction * RayLength;
			MediumPos2 += ray.direction * RayLength;
			
			bool CutFlg1 = false;
			bool CutFlg2 = false;
			ray.origin = MediumPos + Normal * Offset;
			if( other.Raycast( ray, out HitInfo, 100.0f ) ){
				CutFlg1 = true;
			}
			else{
				ray.direction = -Camera.main.transform.forward;
				ray.origin = MediumPos2 + Normal * Offset;
				if( other.Raycast( ray, out HitInfo, 100.0f ) ){
					CutFlg1 = true;
				}
			}
			ray.direction = Camera.main.transform.forward;
			ray.origin = MediumPos - Normal * Offset;
			if( other.Raycast( ray, out HitInfo, 100.0f ) ){
				CutFlg2 = true;
			}
			else{
				ray.direction = -Camera.main.transform.forward;
				ray.origin = MediumPos2 - Normal * Offset;
				if( other.Raycast( ray, out HitInfo, 100.0f ) ){
					CutFlg2 = true;
				}
			}
			
			if( CutFlg1 && CutFlg2 )
			{
				Vector3	LocalStartPos = other.transform.InverseTransformPoint( HitStartPos );
				Vector3	LocalEndPos = other.transform.InverseTransformPoint( HitEndPos );
				Vector3	LocalVec = (LocalEndPos - LocalStartPos).normalized;
				float Angle = Vector3.Dot( Vec.normalized, Camera.main.transform.right );
				if( LocalVec.z < 0.0f ){
					Angle = -Angle;
				}
				float Deg = Mathf.Acos(Angle)*180.0f/Mathf.PI;
				Instantiate( ContactParticlePrefab, HitStartPos + Vec / 2.0f, Quaternion.AngleAxis( Deg, Camera.main.transform.forward ) );
				PointParticle[1] = Instantiate( PointOneShotParticlePrefab, HitEndPos, Quaternion.identity ) as GameObject;
				PointParticle[0].particleEmitter.emit = false;
				//if( OrigamiCutter.Cut( other.gameObject, HitStartPos, HitEndPos ) ){
				if( OrigamiMeshCutter.Cut( other.gameObject, HitStartPos, HitEndPos ) ){
					// レイヤー変更.
					other.gameObject.layer = (int)LayerEnum.layer_OrigamiWait;
					OrigamiControllerScript.SetState( OrigamiUpdate.STATE.FOLD_SELECT );
					Destroy( PointParticle[0] );
					PointParticle[0] = null;
				}
			}
			else{
				Destroy( PointParticle[0] );
				PointParticle[0] = null;
			}
			
			HitFlg = false;
		}
	}
}
