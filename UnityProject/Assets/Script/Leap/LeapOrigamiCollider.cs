using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeapOrigamiCollider : MonoBehaviour {
	
	public	GameObject		LineEffectPrefab;
	public	GameObject		PointLoopParticlePrefab;
	public	GameObject		PointOneShotParticlePrefab;
	private	GameObject[]	PointParticle = new GameObject[2]{ null, null };
	private	GameObject		HitObj = null;
	private Vector3			HitStartPos;
	private Vector3 		HitEndPos;
	private	bool			HitFlg = false;
	private OrigamiController OrigamiControllerScript;
	private	GameObject		LineEffectObj = null;
	private	LineEffect		LineEffectScript = null;
	private	bool			isEnd = false;

	// Use this for initialization
	void Start () {
		OrigamiControllerScript = GameObject.Find("OrigamiController").gameObject.GetComponent<OrigamiController>();
	}
	
	// Update is called once per frame
	void Update () {
		if( !OrigamiControllerScript.GetActiveFlg() || HitObj == null ){
			PointParticleDestroy();
			return;
		}
		if( HitObj.layer == (int)LayerEnum.layer_OrigamiCut && !HitFlg ){
			PointParticleDestroy();
		}
		
		if( LineEffectScript != null && !isEnd ){
			float z = LineEffectScript.targetPositionEnd.z;
			LineEffectScript.targetPositionEnd.Set( transform.localPosition.x, transform.localPosition.y, z );
		}
		
	}
	
	private	void PointParticleDestroy (){
		if( PointParticle[0] != null ){
			Destroy( PointParticle[0] );
			PointParticle[0] = null;
		}
		if( PointParticle[1] != null ){
			Destroy( PointParticle[1] );
			PointParticle[1] = null;
		}
		if( LineEffectObj != null ){
			iTween.Stop( LineEffectObj );
			Destroy( LineEffectObj );
		}
	}
	
	private void OnTriggerEnter (Collider other){
		if( enabled == false ) return;
		
		if( other.gameObject.layer == (int)LayerEnum.layer_OrigamiCut ){
			UI_OKButton.Instance.Off();
			UI_RevertButton.Instance.Off();
			//UI_InGame.Instance.ButtonEnable(false);
			
			if( PointParticle[0] != null ){
				Destroy( PointParticle[0] );
				PointParticle[0] = null;
			}
			if( PointParticle[1] != null ){
				Destroy( PointParticle[1] );
				PointParticle[1] = null;
			}
			
			if( LineEffectObj != null ){
				iTween.Stop( LineEffectObj );
				Destroy( LineEffectObj );
			}
			
			HitObj = other.gameObject;
			HitStartPos = other.collider.ClosestPointOnBounds(transform.position);
			PointParticle[0] = Instantiate( PointLoopParticlePrefab, HitStartPos, Quaternion.identity ) as GameObject;
			
			LineEffectObj = Instantiate( LineEffectPrefab, Vector3.zero, Quaternion.identity ) as GameObject;
			LineEffectObj.transform.parent = HitObj.transform.parent;
			LineEffectScript = LineEffectObj.GetComponent<LineEffect>();
			Vector3 pos = transform.localPosition;
			pos.z = HitObj.transform.localPosition.z - 0.2f;
			LineEffectScript.targetPositionStart = pos;
			LineEffectScript.targetPositionEnd = pos;
			LineEffectScript.MoveToTargetPositionStart();
			
			HitFlg = true;
			isEnd = false;
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
				
				// ContactParticleの座標と角度を設定.
				float Angle = Vector3.Dot( Vec.normalized, Camera.main.transform.right );
				if( LocalVec.z < 0.0f ){
					Angle = -Angle;
				}
				float Deg = Mathf.Acos(Angle)*180.0f/Mathf.PI;
				OrigamiSelect	OrigamiSelectScript = other.gameObject.GetComponent<OrigamiSelect>();
				OrigamiSelectScript.ContactParticleAngle = Deg;
				OrigamiSelectScript.ContactParticlePos = HitStartPos + Vec / 2.0f - Vector3.forward * 0.3f;
				
				PointParticle[1] = Instantiate( PointOneShotParticlePrefab, HitEndPos, Quaternion.identity ) as GameObject;

				// カット.
				if( OrigamiMeshCutter.Cut( other.gameObject, HitStartPos, HitEndPos ) ){
					other.gameObject.layer = (int)LayerEnum.layer_OrigamiWait;
					OrigamiControllerScript.SetState( OrigamiUpdate.STATE.FOLD_SELECT );
				}
				// 2013/11/26 kojima
				iTweenEvent.GetEvent(GameObject.Find("UI_Select"), "FadeIn").Play();
				isEnd = true;
			}
			else{
				Destroy( PointParticle[0] );
				iTween.Stop( LineEffectObj );
				Destroy( LineEffectObj );
				LineEffectObj = null;
				PointParticle[0] = null;
				
				UI_OKButton.Instance.On();
				UI_RevertButton.Instance.On();
			}
			
			HitFlg = false;
		}
	}
}
