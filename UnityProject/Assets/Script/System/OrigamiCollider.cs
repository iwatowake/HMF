using UnityEngine;
using System.Collections;

public class OrigamiRay {
	public Vector3	Position;
	public bool		Enable;
}

public class OrigamiCollider : MonoBehaviour {
	
	public 	GameObject		FrameObject;
	private	float			PlaneSize = 0.5f;
	private	int				RayOffset = 50;
	public	OrigamiRay[]	RayPointPlane;
	public	OrigamiRay[]	RayPoint;
	private	int				Num;

	// Use this for initialization
	void Start () {
		Num = RayOffset*RayOffset;
		RayPoint = new OrigamiRay[Num];
		RayPointPlane = new OrigamiRay[Num];
		for( int i = 0; i < RayOffset; i++ ){
			for( int j = 0; j < RayOffset; j++ ){
				RayPointPlane[i*RayOffset+j] = new OrigamiRay();
				RayPointPlane[i*RayOffset+j].Position.x = PlaneSize / (float)RayOffset * j - PlaneSize / 2.0f;
				RayPointPlane[i*RayOffset+j].Position.y = 1.0f;
				RayPointPlane[i*RayOffset+j].Position.z = PlaneSize / (float)RayOffset * i - PlaneSize / 2.0f;
				RayPointPlane[i*RayOffset+j].Enable = false;
			}
		}
		for( int i = 0; i < Num; i++ ){
			RayPoint[i] = new OrigamiRay();
			RayPoint[i].Position = RayPointPlane[i].Position;
			RayPoint[i].Enable = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public float	GetPercent	(){
		Collider 	FrameCollider = FrameObject.collider;
		Ray			ray = new Ray();
		RaycastHit 	HitInfo = new RaycastHit();
		ray.direction = Camera.main.transform.forward;
		int	HitCnt = 0;
		int	PlaneHitCnt = 0;
		for( int i = 0; i < RayOffset; i++ ){
			for( int j = 0; j < RayOffset; j++ ){
				ray.origin = transform.TransformPoint( RayPointPlane[i*RayOffset+j].Position );
				if( FrameCollider.Raycast( ray, out HitInfo, 100.0f ) ){
					PlaneHitCnt++;
				}
			}
		}
		for( int i = 0; i < RayOffset; i++ ){
			for( int j = 0; j < RayOffset; j++ ){
				if( RayPoint[i*RayOffset+j].Enable ){
					ray.origin = transform.TransformPoint( RayPoint[i*RayOffset+j].Position );
					if( FrameCollider.Raycast( ray, out HitInfo, 100.0f ) ){
						HitCnt++;
					}
				}
			}
		}
		
		float PlanePer = (float)PlaneHitCnt/(float)Num * 100.0f;
		float OrigamiPer = (float)HitCnt/(float)Num * 100.0f;
		float Per;
		if( PlanePer != 0.0f ){
			Per = OrigamiPer / PlanePer * 100.0f;
		}
		else{
			Per = 0.0f;
		}
		print( Per.ToString()+"%" );
		return Per;
	}
}
