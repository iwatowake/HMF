using UnityEngine;
using System.Collections;

public class OrigamiController : SingletonMonoBehaviour<OrigamiController>  {

	public GameObject 		OrigamiPrefab;
	public GameObject[]		WakuPrefab;
	public GameObject[]		ClapEffect;
	
	private	bool		OrigamiActiveFlg = false;
	private GameObject	WakuObj = null;
	private	GameObject	OrigamiObj = null;
	private	int			WakuCount = 0;
	private float		Per;
	private	bool		isClap = false;
	private bool		ClapFlg = true;
	
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
		
		if( Input.GetKeyDown(KeyCode.T) ){
			SetUpdateFlg( false );
		}
		if( Input.GetKeyDown(KeyCode.Y) ){
			SetUpdateFlg( true );
		}
		
		if( OrigamiUpdateScript.GetState() == OrigamiUpdate.STATE.STOP ) return;
		
		// 折る制限時間経過判定.
		if( OrigamiObj.layer == (int)LayerEnum.layer_OrigamiCut ){
//			if( StaticMath.Compensation( ref CutTimeCounter, CutTime, 1 ) ){
			if( UI_TimeCounter.Instance.timeOver ){
				UI_OKButton.Instance.Off();
				UI_RevertButton.Instance.Off();
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
		WakuObj.transform.localPosition = new Vector3( 0,0,4.5f );
		// プレハブ作る時にセットし忘れたからここで・・・.
		WakuObj.GetComponent<MeshCollider>().sharedMesh = WakuObj.GetComponent<MeshFilter>().mesh;
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
	
	// 折り紙破棄.
	public void OrigamiDestroy (){
		if( OrigamiObj != null ){
			Destroy( OrigamiObj );
			OrigamiObj = null;
		}
		if( WakuObj != null ){
			Destroy( WakuObj );
			WakuObj = null;
		}
		OrigamiActiveFlg = false;
	}
	
	// 拍手された.
	public void Clap (){
		if( !ClapFlg ) return;
		if( OrigamiObj == null || WakuObj == null ) return;
		if( OrigamiUpdateScript.GetState() == OrigamiUpdate.STATE.STOP ) return;
		if( OrigamiObj.layer == (int)LayerEnum.layer_OrigamiCut ){
			UI_OKButton.Instance.Off();
			UI_RevertButton.Instance.Off();
			
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
		if(Per >= Define_Rate.Excellent){
			Index = 3;
		}
		// Good.
		else if(Per >= Define_Rate.Good){
			Index = 2;
		}
		// Safe.
		else if(Per >= Define_Rate.Safe){
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
	public OrigamiUpdate.STATE GetState ( ref OrigamiUpdate.STATE arState ){
		if( OrigamiObj == null || WakuObj == null ) return OrigamiUpdate.STATE.STOP;
		return OrigamiUpdateScript.GetState();
	}
	
	public bool GetRevertFlg (){
		if( OrigamiObj == null || WakuObj == null ) return false;
		return OrigamiUpdateScript.RevertFlg;
	}
	
	public void Revert (){
		if( OrigamiObj == null || WakuObj == null ) return;
		if( OrigamiUpdateScript.RevertFlg ){
			OrigamiUpdateScript.SetState( OrigamiUpdate.STATE.REVERT );
			OrigamiObj.layer = (int)LayerEnum.layer_OrigamiUpdate;
			UI_RevertButton.Instance.Off();
		}
	}
	
	// 折り紙の一切の動きを停止・再生する.
	public void SetUpdateFlg ( bool flg ){
		if( OrigamiObj == null || WakuObj == null ) return;
		if( flg ){
			UI_TimeCounter.Instance.enabled = true;
			OrigamiUpdateScript.ReStart();
		}
		else{
			UI_TimeCounter.Instance.enabled = false;
			OrigamiUpdateScript.Stop();
		}
	}
	
	// 拍手有効.
	public void EnableClap (){
		ClapFlg = true;
	}
	
	// 拍手無効.
	public void DisableClap (){
		ClapFlg = false;
	}
	
}
