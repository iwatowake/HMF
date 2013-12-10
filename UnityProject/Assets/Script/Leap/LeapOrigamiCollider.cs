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
	
	private Vector3			MoveVec;
	private	Vector3			OldPos;
	private	Vector3			NowPos;
	private	float			MoveVecRange;
	
	private	Ray				ray = new Ray();
	private	RaycastHit 		HitInfo = new RaycastHit();

	// Use this for initialization
	void Start () {
		ray.direction = Vector3.forward;
		OrigamiControllerScript = GameObject.Find("OrigamiController").gameObject.GetComponent<OrigamiController>();
	}
	
	// Update is called once per frame
	void Update () {
		OldPos = NowPos;
		NowPos = transform.position;
		MoveVec = (OldPos - NowPos).normalized;
		MoveVecRange = Vector3.Distance( OldPos, NowPos );
		
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
			if( other.gameObject.GetComponent<OrigamiUpdate>().GetState() == OrigamiUpdate.STATE.STOP ) return;
			UI_OKButton.Instance.Off();
			UI_RevertButton.Instance.Off();
			//UI_InGame.Instance.ButtonEnable(false);
			
			PointParticleDestroy();
			
			Vector3	Vec = MoveVec;
			Vector3	HitPos = other.ClosestPointOnBounds(transform.position);
			Vector3	OldHitPos = HitPos;
			for(;MoveVecRange != 0.0f;){
				Debug.Log("hokan");
				ray.origin = HitPos;
				if( !other.Raycast( ray, out HitInfo, 10.0f ) ){
					HitPos = OldHitPos;
					break;
				}
				OldHitPos = HitPos;
				HitPos += Vec * 0.05f;
			}

			HitObj = other.gameObject;
			HitStartPos = HitPos;
			PointParticle[0] = Instantiate( PointLoopParticlePrefab, HitStartPos, Quaternion.identity ) as GameObject;
			
			LineEffectObj = Instantiate( LineEffectPrefab, Vector3.zero, Quaternion.identity ) as GameObject;
			LineEffectObj.transform.parent = HitObj.transform.parent;
			LineEffectScript = LineEffectObj.GetComponent<LineEffect>();
			Vector3 pos = HitPos;
			pos.y -= 50000.0f;
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
			if( other.gameObject.GetComponent<OrigamiUpdate>().GetState() == OrigamiUpdate.STATE.STOP ) return;		
			
			Vector3	HitPos = other.ClosestPointOnBounds(transform.position);
			Vector3	OldHitPos = HitPos;
			for(;MoveVecRange != 0.0f;){
				Debug.Log("hokan");
				ray.origin = HitPos;
				if( !other.Raycast( ray, out HitInfo, 10.0f ) ){
					HitPos = OldHitPos;
					break;
				}
				OldHitPos = HitPos;
				HitPos -= MoveVec * 0.05f;
			}
			
			// 折れるかどうか判定.
			Vector3	Vec = HitPos - HitStartPos;
			Vector3	MediumPos = HitStartPos + Vec / 2.0f;
			Vector3	MediumPos2 = MediumPos;
			Vector3	Normal = Vector3.Cross( Camera.main.transform.forward, Vec.normalized );
			
			Ray			endray = new Ray();
			float		RayLength = 10.0f;
			float		Offset = 0.3f;
			endray.direction = Camera.main.transform.forward;
			MediumPos -= endray.direction * RayLength;
			MediumPos2 += endray.direction * RayLength;
			
			bool CutFlg1 = false;
			bool CutFlg2 = false;
			endray.origin = MediumPos + Normal * Offset;
			if( other.Raycast( endray, out HitInfo, 100.0f ) ){
				CutFlg1 = true;
			}
			else{
				endray.direction = -Camera.main.transform.forward;
				endray.origin = MediumPos2 + Normal * Offset;
				if( other.Raycast( endray, out HitInfo, 100.0f ) ){
					CutFlg1 = true;
				}
			}
			endray.direction = Camera.main.transform.forward;
			endray.origin = MediumPos - Normal * Offset;
			if( other.Raycast( endray, out HitInfo, 100.0f ) ){
				CutFlg2 = true;
			}
			else{
				endray.direction = -Camera.main.transform.forward;
				endray.origin = MediumPos2 - Normal * Offset;
				if( other.Raycast( endray, out HitInfo, 100.0f ) ){
					CutFlg2 = true;
				}
			}
			
			if( CutFlg1 && CutFlg2 )
			{
				HitEndPos = HitPos;
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
