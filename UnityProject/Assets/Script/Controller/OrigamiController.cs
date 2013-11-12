using UnityEngine;
using System.Collections;

public class OrigamiController : MonoBehaviour {
	public GameObject 			OrigamiPrefab;
	public GameObject[]			WakuPrefab;
	public GameObject[]			ClapEffect;
	
	private	bool		OrigamiActiveFlg = false;
	private GameObject	WakuObj = null;
	private	GameObject	OrigamiObj = null;
	private	int			WakuCount = 0;
	private float		Per;
	
	private int			CutTime = 0;
	private int			CutTimeCounter = 0;
	
	public float GetPercent () { return Per; }
	public bool GetActiveFlg	() { return OrigamiActiveFlg; }

	// Use this for initialization
	void Start () {
		ELGManager.clapGestureRegistered = true;
		ELGManager.GestureRecognised += onGestureRecognized;
	}
	
	void OnDestroy(){
		ELGManager.GestureRecognised -= onGestureRecognized;
	}
	
	// Update is called once per frame
	void Update () {
		////////////
		if( Input.GetMouseButtonDown(0) ){

			CreateOrigami( WAKU.LEVEL_1_1, 10 );
		}
		////////////

		if( OrigamiObj == null ) return;
		
		// 折る制限時間経過判定.
		if( OrigamiObj.layer == (int)LayerEnum.layer_OrigamiCut ){
			if( StaticMath.Compensation( ref CutTimeCounter, CutTime, 1 ) ){
				WakuGenerator.CutTime = CutTimeCounter;
				OrigamiObj.layer = (int)LayerEnum.layer_OrigamiUpdate;
			}
		}
		
		////////////
		if( Input.GetKeyDown(KeyCode.G) ){
			Clap();
		}
		////////////
		
		if( OrigamiObj.GetComponent<OrigamiUpdate>().GetState() == OrigamiUpdate.STATE.WAIT ){
			Per = OrigamiObj.GetComponent<OrigamiCollider>().GetPercent();
			Destroy( OrigamiObj );
			OrigamiObj = null;
			Destroy( WakuObj );
			WakuObj = null;
			
			OrigamiActiveFlg = false;
			
			UI_TentionGauge.Instance.SetResult(Per);
			UI_TimeCounter.Instance.SetResult(Per);
			UI_RateManager.Instance.SetRate(Per);
		}
	}
	
	// 折り紙 枠 生成.

	public void CreateOrigami ( WAKU WakuLevel, int inCutTime ){
		if( WakuPrefab.Length <= (int)WakuLevel ) return;
		if( OrigamiObj != null ){
			Destroy( OrigamiObj );
			OrigamiObj = null;
		}
		if( WakuObj != null ){
			Destroy( WakuObj );
			WakuObj = null;
		}
		CutTime = inCutTime * 60;
		CutTimeCounter = 0;
		
		WakuObj = Instantiate( WakuPrefab[(int)WakuLevel] ) as GameObject;
		Vector3 euler = WakuObj.transform.eulerAngles;
		WakuObj.transform.parent = Camera.main.transform;
		WakuObj.transform.localEulerAngles = euler;
		WakuObj.transform.localPosition = new Vector3( 0,0,5.0f );
		WakuCount++;
		
		OrigamiObj = Instantiate( OrigamiPrefab ) as GameObject;
		euler = OrigamiObj.transform.eulerAngles;
		OrigamiObj.transform.parent = Camera.main.transform;
		OrigamiObj.transform.localEulerAngles = euler;
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
			WakuGenerator.CutTime = CutTimeCounter;
			
			for(int i=0; i<ClapEffect.Length; i++){
				Instantiate(ClapEffect[i], OrigamiObj.transform.position - Vector3.forward * 0.1f,
					Quaternion.identity);
			}
		}

	}
	
	void onGestureRecognized(EasyLeapGesture gesture){
		if(gesture.Type == EasyLeapGestureType.CLAP)
			Clap();
	}
}
