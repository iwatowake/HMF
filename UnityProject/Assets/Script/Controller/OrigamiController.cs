using UnityEngine;
using System.Collections;

public class OrigamiController : MonoBehaviour {

	public GameObject 		OrigamiPrefab;
	public GameObject[]		WakuPrefab;
	public GameObject[]		ClapEffect;
	
	private	bool		OrigamiActiveFlg = false;
	private GameObject	WakuObj = null;
	private	GameObject	OrigamiObj = null;
	private	int			WakuCount = 0;
	private float		Per;
	private	bool		isClap = false;
	
	private int			CutTime = 0;
	private int			CutTimeCounter = 0;
	
	private OrigamiUpdate	OrigamiUpdateScript;
	
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
		if( OrigamiObj == null ) return;
		
		// 折る制限時間経過判定.
		if( OrigamiObj.layer == (int)LayerEnum.layer_OrigamiCut ){
			if( StaticMath.Compensation( ref CutTimeCounter, CutTime, 1 ) ){

				WakuGenerator.Instance.CutTime = CutTimeCounter;
				OrigamiUpdateScript.SetState( OrigamiUpdate.STATE.END_MOVE );
				OrigamiObj.layer = (int)LayerEnum.layer_OrigamiUpdate;
				isClap = false;
			}
		}
		
		////////////
		if( Input.GetKeyDown(KeyCode.G) ){
			Clap();
		}
		////////////
		
		if( OrigamiObj.GetComponent<OrigamiUpdate>().GetState() == OrigamiUpdate.STATE.WAIT ){
			// 拍手されていない場合.
			if( !isClap ){
				Per = OrigamiObj.GetComponent<OrigamiCollider>().GetPercent();
				int Index = GetResultIndex(Per);
				WakuGenerator.Instance.SetResult( Index );
			}
			
			Destroy( OrigamiObj );
			OrigamiObj = null;
			Destroy( WakuObj );
			WakuObj = null;
			
			OrigamiActiveFlg = false;
			
			UI_TentionGauge.Instance.SetResult(Per);

//			UI_TimeCounter.Instance.SetResult(Per);
			UI_RateManager.Instance.SetRate(Per);
			UI_TimeCounter.Instance.StopTimer();
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

		OrigamiUpdateScript = OrigamiObj.GetComponent<OrigamiUpdate>();
		OrigamiUpdateScript.WakuObject = WakuObj;
		OrigamiActiveFlg = true;
		
		// タイマー始動
		UI_TimeCounter.Instance.StartTimer();
	}
	
	// 拍手された.
	public void Clap (){
		if( OrigamiObj == null || WakuObj == null ) return;
		if( OrigamiObj.layer == (int)LayerEnum.layer_OrigamiCut ){
			OrigamiUpdateScript.SetState( OrigamiUpdate.STATE.END_MOVE );
			OrigamiObj.layer = (int)LayerEnum.layer_OrigamiUpdate;

			WakuGenerator.Instance.CutTime = CutTimeCounter;
			
			Per = OrigamiObj.GetComponent<OrigamiCollider>().GetPercent();

			int Index = GetResultIndex(Per);
			WakuGenerator.Instance.SetResult( Index );
			Instantiate(ClapEffect[Index], OrigamiObj.transform.position - Vector3.forward * 0.1f,
			Quaternion.identity);
			isClap = true;
		}

	}
	
	private int GetResultIndex ( float Par ){
		int Index = 0;
		// Excellent.
		if(Per >= 95.0f){
			Index = 3;
		}
		// Good.
		else if(Per >= 85.0f){
			Index = 2;
		}
		// Safe.
		else if(Per >= 60.0f){
			Index = 1;
		}
		// Bad.
		else{
			Index = 0;
		}
		return Index;
	}
	
	
	void onGestureRecognized(EasyLeapGesture gesture){
		if(gesture.Type == EasyLeapGestureType.CLAP)
			Clap();
	}
	
	public void SetState ( OrigamiUpdate.STATE inState ){
		if( OrigamiObj == null || WakuObj == null ) return;
		OrigamiUpdateScript.SetState( inState );
	}
	public bool GetState ( ref OrigamiUpdate.STATE arState ){
		if( OrigamiObj == null || WakuObj == null ) return false;
		arState = OrigamiUpdateScript.GetState();
		return true;
	}
}
