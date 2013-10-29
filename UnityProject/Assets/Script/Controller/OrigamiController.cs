using UnityEngine;
using System.Collections;


public class OrigamiController : MonoBehaviour {
	public enum WAKU{
		LEVEL_1_1,
		NUM
	};
	public GameObject 	OrigamiPrefab;
	public GameObject[]	WakuPrefab;
	
	private	bool		OrigamiActiveFlg = false;
	private GameObject	WakuObj = null;
	private	GameObject	OrigamiObj = null;
	private	int			WakuCount = 0;
	
	public bool GetActiveFlg	() { return OrigamiActiveFlg; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKeyDown( KeyCode.F1 ) ){
			CreateOrigami( WAKU.LEVEL_1_1 );
		}
		
		if( OrigamiObj == null ) return;
		if( OrigamiObj.GetComponent<OrigamiUpdate>().GetState() == OrigamiUpdate.STATE.WAIT ){
			OrigamiObj.GetComponent<OrigamiCollider>().GetPercent();
			Destroy( OrigamiObj );
			OrigamiObj = null;
			Destroy( WakuObj );
			WakuObj = null;
		}
	}
	
	// 折り紙 枠 生成.
	public void CreateOrigami ( WAKU WakuLevel ){
		if( WakuPrefab.Length <= (int)WakuLevel ) return;
		if( OrigamiObj != null ){
			Destroy( OrigamiObj );
			OrigamiObj = null;
		}
		if( WakuObj != null ){
			Destroy( WakuObj );
			WakuObj = null;
		}
		WakuObj = Instantiate( WakuPrefab[(int)WakuLevel] ) as GameObject;
		WakuObj.transform.parent = Camera.main.transform;
		WakuObj.transform.localPosition = new Vector3( 0,0,10.0f );
		WakuCount++;
		
		OrigamiObj = Instantiate( OrigamiPrefab ) as GameObject;
		OrigamiObj.transform.parent = Camera.main.transform;
		OrigamiObj.transform.localPosition = Vector3.zero;
		OrigamiObj.layer = (int)LayerEnum.layer_OrigamiUpdate;
		OrigamiObj.GetComponent<OrigamiCollider>().WakuObject = WakuObj;
		OrigamiObj.GetComponent<OrigamiUpdate>().WakuObject = WakuObj;
		OrigamiActiveFlg = true;
	}
	
	// 拍手された.
	public void Clap (){
		if( OrigamiObj == null || WakuObj == null ) return;
		if( OrigamiObj.layer == (int)LayerEnum.layer_OrigamiCut ){
			OrigamiObj.layer = (int)LayerEnum.layer_OrigamiUpdate;
		}
	}
}
