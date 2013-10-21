using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandObjectController : MonoBehaviour {
	
	private Vector3			Pos;
	private List<Vector3>	PosArray = new List<Vector3>();
	

	// Use this for initialization
	void Start () {
		Pos = Camera.main.transform.localPosition + Camera.main.transform.forward * 10.0f;
	}
	
	// Update is called once per frame
	void	Update	(){
		if( PosArray.Count == 0 )	return;
		
		const float	OffsetLength = 0.7f;
		float	Length = Vector3.Distance( Pos, PosArray[0] );
		// 距離が長すぎた場合補間しない.
		/*if( Length > MaxLength ){
			Pos = PosArray[0];
		}*/
		if( Length > OffsetLength ){
			Pos += (PosArray[0] - Pos).normalized * OffsetLength;
		}
		else{
			int	Cnt = 0;
			for( int i = 0; i < PosArray.Count; i++ ){
				if( Vector3.Distance( Pos, PosArray[i] ) <= OffsetLength ){
					Pos = PosArray[i];
					Cnt ++;
				}
				else{
					break;
				}
			}
			PosArray.RemoveRange( 0, Cnt );
		}
		
		gameObject.transform.localPosition = Pos;
	}
	
	public void	SetPos	( Vector3 inPos ){
		
		Vector2[] ScreenPolygon = new Vector2[] {
			new Vector2( 0.0f        , 0.0f ),
			new Vector2( Screen.width, 0.0f ),
			new Vector2( 0.0f        , Screen.height ),
			new Vector2( Screen.width, Screen.height ),
		};
		Vector3	ScreenPos = Camera.main.WorldToScreenPoint( inPos );
		if( StaticMath.CheckInPolygon2D( ScreenPolygon, 4, new Vector2( ScreenPos.x, ScreenPos.y ) ) ){
			PosArray.Add( inPos );
			gameObject.SetActive( true );
		}
		else{
			gameObject.SetActive( false );
		}
	}
}
