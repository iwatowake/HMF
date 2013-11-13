using UnityEngine;
using System.Collections;

public class OrigamiRay {
	public Vector3	Position;
	public bool		Enable;
}

public class OrigamiCollider : MonoBehaviour {
	
	public 	GameObject		WakuObject;
	private	float			PlaneSize = 0.5f;
	private	int				RayOffset = 50;
	public	OrigamiRay[]	RayPointPlane;
	public	OrigamiRay[]	RayPoint;
	private	int				Num;
	
	private		Vector3[]	StartPos;
	private		Vector3[]	EndPos;
	private		int[]		OrigamiIndex;
	private		float		t = 0.0f;
	
	public void SetStartPos ( Vector3[] Pos ){ StartPos = Pos; }
	public void SetEndPos ( Vector3[] Pos ){ EndPos = Pos; }
	public void SetOrigamiIndex ( int[] Index ){ OrigamiIndex = Index; }
	
	// Use this for initialization
	void Start () {
		Num = RayOffset*RayOffset;
		RayPoint = new OrigamiRay[Num];
		RayPointPlane = new OrigamiRay[Num];
		for( int i = 0; i < RayOffset; i++ ){
			for( int j = 0; j < RayOffset; j++ ){
				RayPointPlane[i*RayOffset+j] = new OrigamiRay();
				RayPointPlane[i*RayOffset+j].Position.x = PlaneSize / (float)RayOffset * j - PlaneSize / 2.0f;
				RayPointPlane[i*RayOffset+j].Position.y = 0.0f;
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
		if( gameObject.layer == (int)LayerEnum.layer_OrigamiWait ){
			Mesh HitMesh = gameObject.GetComponent<MeshFilter>().mesh;
			Vector3[] Vertex = HitMesh.vertices;
			for( int i = 0; i < OrigamiIndex.Length; i++ ){
				Vertex[OrigamiIndex[i]] = Vector3.Slerp( StartPos[i], EndPos[i], t );
			}
			HitMesh.vertices = Vertex;
			if( StaticMath.Compensation( ref t, 1.0f, 0.02f ) ){
				t = 0.0f;
				gameObject.GetComponent<MeshCollider>().sharedMesh = HitMesh;
				gameObject.layer = (int)LayerEnum.layer_OrigamiCut;
			}
		}
	}
	
	public float	GetPercent	(){
		Collider 	WakuCollider = WakuObject.collider;
		Ray			ray = new Ray();
		RaycastHit 	HitInfo = new RaycastHit();
		ray.direction = Camera.main.transform.forward;
		int	HitCnt = 0;
		int	PlaneHitCnt = 0;
		for( int i = 0; i < RayOffset; i++ ){
			for( int j = 0; j < RayOffset; j++ ){
				ray.origin = transform.TransformPoint( RayPointPlane[i*RayOffset+j].Position );
				if( WakuCollider.Raycast( ray, out HitInfo, 100.0f ) ){
					PlaneHitCnt++;
				}
			}
		}
		for( int i = 0; i < RayOffset; i++ ){
			for( int j = 0; j < RayOffset; j++ ){
				if( RayPoint[i*RayOffset+j].Enable ){
					ray.origin = transform.TransformPoint( RayPoint[i*RayOffset+j].Position );
					if( WakuCollider.Raycast( ray, out HitInfo, 100.0f ) ){
						HitCnt++;
					}
					else{
						HitCnt--;
					}
				}
			}
		}
		
		float PlanePer = (float)PlaneHitCnt/(float)Num * 100.0f;
		float OrigamiPer = (float)HitCnt/(float)Num * 100.0f;
		float Per;
		if( PlanePer != 0.0f ){
			Per = OrigamiPer / PlanePer * 100.0f;
			if( Per < 0.0f )	Per = 0.0f;
		}
		else{
			Per = 0.0f;
		}
		print( Per.ToString()+"%" );
		return Per;
	}
	
	public bool	GetOriFlg ( Vector3 Vec, Vector3 Point ){
		int	LeftCnt = 0;
		int	RightCnt = 0;
		for( int i = 0; i < RayOffset; i++ ){
			for( int j = 0; j < RayOffset; j++ ){
				if( RayPoint[i*RayOffset+j].Enable ){
					if( OrigamiCutter.GetSide( Vec, RayPoint[i*RayOffset+j].Position - Point ) ){
						LeftCnt++;
					}
					else{
						RightCnt++;
					}
				}
			}
		}
		if( LeftCnt > RightCnt ){
			return false;
		}
		return true;
	}
}
