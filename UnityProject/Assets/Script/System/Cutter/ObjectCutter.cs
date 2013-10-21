using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectCutter : MonoBehaviour {

	private		GameObject	HitObj = null;
	private 	Vector3 	hitpoint1;
	private 	Vector3 	hitpoint2;
	
	private		Vector3[]	StartPos;
	private		Vector3[]	EndPos;
	private		int[]		OrigamiIndex;
	private		float		t = 0.0f;
	
	
	void Update () {
		if( HitObj == null ) return;
		if( HitObj.layer == 9 ){
			Mesh HitMesh = HitObj.GetComponent<MeshFilter>().mesh;
			Vector3[] Vertex = HitMesh.vertices;
			for( int i = 0; i < OrigamiIndex.Length; i++ ){
				Vertex[OrigamiIndex[i]] = Vector3.Slerp( StartPos[i], EndPos[i], t );
			}
			HitMesh.vertices = Vertex;
			if( StaticMath.Compensation( ref t, 1.0f, 0.02f ) ){
				HitObj.GetComponent<MeshCollider>().sharedMesh = HitMesh;
				HitObj.layer = 8;
				HitObj = null;
			}
		}
	}
	
	
	// 左右判定.
	private	bool GetSide ( Vector3 CutVec, Vector3 PointVec ){
		if( (CutVec.z * PointVec.x - CutVec.x * PointVec.z) < 0 ){
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
		Vector3	LocalHitPoint1 = other.transform.InverseTransformPoint( hitpoint1 );
		Vector3	LocalHitPoint2 = other.transform.InverseTransformPoint( hitpoint2 );
		Vector3	LocalHitVec = LocalHitPoint2 - LocalHitPoint1;
		Vector3	LocalNormal = Vector3.Cross(LocalHitVec.normalized, new Vector3(0,1,0));
		
		int			LeftCnt = 0;
		int			RightCnt = 0;
		bool		OriFlg = true;
		Mesh 		otherMesh = other.GetComponent<MeshFilter>().mesh; 
		Vector3[] 	Vertices = otherMesh.vertices;
		int[]		Index = otherMesh.triangles;
		int[]		VertCnt = new int[2]{ 0, 0 };
		int[]		TriangleCnt = new int[2];
		Vector3		CrossPoint = Vector3.zero;
		// 頂点数カウント.
		for ( int i = 0; i < Index.Length; i+=3 ){
			TriangleCnt[0] = 0;
			TriangleCnt[1] = 0;
			for( int j = 0; j < 3; j++ ){
				if( GetSide( LocalHitVec, Vertices[Index[i+j]] - LocalHitPoint1 ) ){
					LeftCnt++;
					TriangleCnt[0]++;
					if( TriangleCnt[0] == 3 ){
						TriangleCnt[0] = 1;
					}
				}
				else{
					RightCnt++;
					TriangleCnt[1]++;
					if( TriangleCnt[1] == 3 ){
						TriangleCnt[1] = 1;
					}
				}
			}
			VertCnt[0] += 3 * TriangleCnt[0];
			VertCnt[1] += 3 * TriangleCnt[1];
		}
		
		// 分割の必要なし.
		if( VertCnt[0] == 0 || VertCnt[1] == 0 )	return;
		
		// 頂点作成.
		Vector3[]	Vertex1 = new Vector3[VertCnt[0]];
		int[]		Index1	= new int[VertCnt[0]];
		Vector3[]	Vertex2 = new Vector3[VertCnt[1]];
		int[]		Index2	= new int[VertCnt[1]];
		int			OneVertIndex = 0;
		int[]		TwoVertIndex = new int[2]{ 0,0 };
		Vector3[]	VertCrossPoint = new Vector3[2];
		VertCnt[0] = 0;
		VertCnt[1] = 0;
		for ( int i = 0; i < Index.Length; i+=3 ){
			TriangleCnt[0] = 0;
			TriangleCnt[1] = 0;
			for( int j = 0; j < 3; j++ ){
				if( GetSide( LocalHitVec, Vertices[Index[i+j]] - LocalHitPoint1 ) ){
					TriangleCnt[0]++;
				}
				else{
					TriangleCnt[1]++;
				}
			}
			
			if( TriangleCnt[0] != 0 && TriangleCnt[1] != 0 ){
				int	Cnt = 0;
				for( int j = 0; j < 3; j++ ){
					if( GetSide( LocalHitVec, Vertices[Index[i+j]] - LocalHitPoint1 ) ){
						if( TriangleCnt[0] == 2 ){
							TwoVertIndex[Cnt] = Index[i+j];
							Cnt++;
						}
						else{
							OneVertIndex = Index[i+j];
						}
					}
					else{
						if( TriangleCnt[1] == 2 ){
							TwoVertIndex[Cnt] = Index[i+j];
							Cnt++;
						}
						else{
							OneVertIndex = Index[i+j];
						}
					}
				}
				
				StaticMath.RayCrossPoint( Vertices[TwoVertIndex[0]], Vertices[OneVertIndex], LocalHitPoint1-LocalHitVec, LocalHitPoint2+LocalHitVec, ref VertCrossPoint[0] );
				StaticMath.RayCrossPoint( Vertices[TwoVertIndex[1]], Vertices[OneVertIndex], LocalHitPoint1-LocalHitVec, LocalHitPoint2+LocalHitVec, ref VertCrossPoint[1] );
				
				if( TriangleCnt[0] == 2 ){
					Vertex1[VertCnt[0]+0] = Vertices[TwoVertIndex[0]];
					Vertex1[VertCnt[0]+1] = VertCrossPoint[0];
					Vertex1[VertCnt[0]+2] = VertCrossPoint[1];
					Vertex1[VertCnt[0]+3] = Vertices[TwoVertIndex[0]];
					Vertex1[VertCnt[0]+4] = VertCrossPoint[1];
					Vertex1[VertCnt[0]+5] = Vertices[TwoVertIndex[1]];
					for( int j = 0; j < 6; j++ )	Index1[Index1.Length-VertCnt[0]-j-1] = VertCnt[0]+j;
					VertCnt[0] += 6;
					Vertex2[VertCnt[1]+0] = VertCrossPoint[0];
					Vertex2[VertCnt[1]+1] = Vertices[OneVertIndex];
					Vertex2[VertCnt[1]+2] = VertCrossPoint[1];
					for( int j = 0; j < 3; j++ )	Index2[VertCnt[1]+j] = VertCnt[1]+j;
					VertCnt[1] += 3;
				}
				else{
					Vertex2[VertCnt[1]+0] = Vertices[TwoVertIndex[0]];
					Vertex2[VertCnt[1]+1] = VertCrossPoint[0];
					Vertex2[VertCnt[1]+2] = VertCrossPoint[1];
					Vertex2[VertCnt[1]+3] = Vertices[TwoVertIndex[0]];
					Vertex2[VertCnt[1]+4] = VertCrossPoint[1];
					Vertex2[VertCnt[1]+5] = Vertices[TwoVertIndex[1]];
					for( int j = 0; j < 6; j++ )	Index2[VertCnt[1]+j] = VertCnt[1]+j;
					VertCnt[1] += 6;
					Vertex1[VertCnt[0]+0] = VertCrossPoint[0];
					Vertex1[VertCnt[0]+1] = Vertices[OneVertIndex];
					Vertex1[VertCnt[0]+2] = VertCrossPoint[1];
					for( int j = 0; j < 3; j++ )	Index1[Index1.Length-VertCnt[0]-j-1] = VertCnt[0]+j;
					VertCnt[0] += 3;
				}
			}
			else{
				if( TriangleCnt[0] == 0 ){
					for( int j = 0; j < 3; j++ ){
						Vertex2[VertCnt[1]] = Vertices[Index[i+j]];
						Index2[VertCnt[1]] = VertCnt[1];
						VertCnt[1]++;
					}
				}
				else if( TriangleCnt[1] == 0 ){
					for( int j = 0; j < 3; j++ ){
						Vertex1[VertCnt[0]] = Vertices[Index[i+j]];
						Index1[Index1.Length-VertCnt[0]-1] = VertCnt[0];
						VertCnt[0]++;
					}
				}
			}
		}
		
		// 折る頂点と折った後の座標を計算.
		/*
		StartPos = new Vector3[Vertex1.Length];
		EndPos = new Vector3[Vertex1.Length];
		for( int i = 0; i < Vertex1.Length; i++ ){	
			float Len = StaticMath.RayDistancePoint( LocalHitPoint1, LocalHitPoint2, Vertex1[i], ref CrossPoint );
			StartPos[i] = Vertex1[i];
			EndPos[i] = Vertex1[i] - LocalNormal * Len * 2.0f;
		}
		*/
		
		// 最適化.
		List<Vector3>	VertList = new List<Vector3>();
		int[]	OptimizeIndex = new int[Index1.Length+Index2.Length];
		for( int i = 0; i < Index1.Length; i++ ){
			bool Hit = false;
			for( int j = 0; j < VertList.Count; j++ ){
				if( VertList[j] == Vertex1[Index1[i]]){
					Hit = true;
					OptimizeIndex[i] = j;
					break;
				}
			}
			if( !Hit ){
				VertList.Add( Vertex1[Index1[i]] );
				OptimizeIndex[i] = VertList.Count-1;
			}
		}
		for( int i = 0; i < Index2.Length; i++ ){
			bool Hit = false;
			for( int j = 0; j < VertList.Count; j++ ){
				if( VertList[j] == Vertex2[Index2[i]]){
					Hit = true;
					OptimizeIndex[Index1.Length+i] = j;
					break;
				}
			}
			if( !Hit ){
				VertList.Add( Vertex2[Index2[i]] );
				OptimizeIndex[Index1.Length+i] = VertList.Count-1;
			}
		}
		Vector3[]	OptimizeVertex = new Vector3[VertList.Count];
		for( int i = 0; i < VertList.Count; i++ ){
			OptimizeVertex[i] = VertList[i];
		}
		
		// 折る前と折った後の頂点座標とインデックスを求める.
		int OriCnt = 0;
		if( LeftCnt > RightCnt ){
			OriFlg = false;
		}
		for( int i = 0; i < OptimizeVertex.Length; i++ ){
			if( GetSide( LocalHitVec, OptimizeVertex[i] - LocalHitPoint1 ) == OriFlg ){
				OriCnt++;
			}
		}
		StartPos = new Vector3[OriCnt];
		EndPos = new Vector3[OriCnt];
		OrigamiIndex = new int[OriCnt];
		OriCnt = 0;
		for( int i = 0; i < OptimizeVertex.Length; i++ ){
			if( GetSide( LocalHitVec, OptimizeVertex[i] - LocalHitPoint1 ) == OriFlg ){
				float Len = StaticMath.RayDistancePoint( LocalHitPoint1, LocalHitPoint2, OptimizeVertex[i], ref CrossPoint );
				StartPos[OriCnt] = OptimizeVertex[i];
				if( OriFlg ){
					EndPos[OriCnt] = OptimizeVertex[i] - LocalNormal * Len * 2.0f;
				}
				else{
					EndPos[OriCnt] = OptimizeVertex[i] + LocalNormal * Len * 2.0f;
				}
				OrigamiIndex[OriCnt] = i;
				OriCnt ++;
			}
		}
		
		// メッシュ作成.
		Mesh NewMesh = new Mesh();
		Vector2[]	UVs = new Vector2[OptimizeVertex.Length];
		NewMesh.vertices = OptimizeVertex;
		NewMesh.triangles = OptimizeIndex;
		NewMesh.uv = UVs;
		NewMesh.RecalculateBounds();
		other.GetComponent<MeshFilter>().mesh = NewMesh;
		other.GetComponent<MeshCollider>().sharedMesh = NewMesh;
		//other.GetComponent<BoxCollider>().size = NewMesh.bounds.size;
		//other.GetComponent<BoxCollider>().center = NewMesh.bounds.center;
		
		// レイヤー変更.
		t = 0.0f;
		other.gameObject.layer = 9;
		
		// 判定を折る.
		Ray			ray = new Ray();
		RaycastHit 	HitInfo = new RaycastHit();
		ray.direction = Camera.main.transform.forward;
		OrigamiCollider origamiCollider = other.gameObject.GetComponent<OrigamiCollider>();
		for( int i = 0; i < origamiCollider.RayPoint.Length; i++ ){
			if( !origamiCollider.RayPoint[i].Enable )	continue;
			if( GetSide( LocalHitVec, origamiCollider.RayPoint[i].Position - LocalHitPoint1 ) == OriFlg ){
				float Len = StaticMath.RayDistancePoint( LocalHitPoint1, LocalHitPoint2, origamiCollider.RayPoint[i].Position, ref CrossPoint );
				if( OriFlg ){
					origamiCollider.RayPoint[i].Position -= LocalNormal * Len * 2.0f;
				}
				else{
					origamiCollider.RayPoint[i].Position += LocalNormal * Len * 2.0f;
				}
				ray.origin = other.transform.TransformPoint( origamiCollider.RayPoint[i].Position );
				if( other.Raycast( ray, out HitInfo, 100.0f ) ){
					origamiCollider.RayPoint[i].Enable = false;
				}
			}
		}
		origamiCollider.GetPercent();
	}
}
