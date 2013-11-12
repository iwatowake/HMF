using UnityEngine;
using System.Collections;

public class WakuGenerator : MonoBehaviour {
	
	enum STATE {
		CREATE,
		WAIT,
		INTERVAL,
	};
	STATE State = STATE.CREATE;
	
	OrigamiController OrigamiControllerScript = null;
	
	private float[] OriTimeParTable = new float[3]{ 0.4f,0.4f,0.2f };
	private float[] OriTimePar = new float[3]{ 0,0,0 };
	private int[] DegreeAddTable = new int[3]{ 2,1,-1 };
	private int	NowDegree = 50;
	private int	IntervalTime = 0;
	private int	OriTime = 0;
	private int Timer = 0;
	static public int CutTime;
	
	public void Init (){
		NowDegree = 50;
		OrigamiControllerScript = GameObject.Find( "OrigamiController" ).GetComponent<OrigamiController>();
	}
	
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if( State == STATE.CREATE ){
			int	Index = NowDegree - Define_WakuPattern.MIN_DEGREE;
			if( Index < 0 )	Index = 0;
			else if( Index >= Define_WakuPattern.MaxRange )	Index = Define_WakuPattern.MaxRange;
			int NumPattern = Define_WakuPattern.Table[Index].Pattern.Length;
			float RandPar = Random.Range( 0.0f, 100.0f );
			float Par = 0;
			int	SelectIndex = 0;
			for( int i = 0; i < NumPattern; i++ ){
				Par += Define_WakuPattern.Table[Index].Pattern[i].Incidence;
				if( Par > RandPar ){
					SelectIndex = i;
					break;
				}
			}
			// 折り紙生成.
			OrigamiControllerScript.CreateOrigami( 
				Define_WakuPattern.Table[Index].Pattern[SelectIndex].WakuLevel, 
				(int)Define_WakuPattern.Table[Index].Pattern[SelectIndex].OriTime );
			OriTime = (int)Define_WakuPattern.Table[Index].Pattern[SelectIndex].OriTime * 60;
			IntervalTime = (int)Define_WakuPattern.Table[Index].Pattern[SelectIndex].IntervalTime * 60;
			
			// 秒数の割合を求める.
			Par = 0;
			for( int i = 0; i < OriTimeParTable.Length; i++ ){
				Par += OriTime * OriTimeParTable[i];
				OriTimePar[i] = Par;
			}
			
			State = STATE.WAIT;
		}
		else if( State == STATE.WAIT ){
			if( OrigamiControllerScript.GetActiveFlg() ) return;
			
			// 加算する段階を計算.
			for( int i = 0; i < OriTimeParTable.Length; i++ ){
				if( CutTime <= OriTimePar[i] ){
					NowDegree += DegreeAddTable[i];
					Timer = 0;
					State = STATE.INTERVAL;
					break;
				}
			}
		}
		else{
			if( StaticMath.Compensation( ref Timer, IntervalTime, 1 ) ){
				State = STATE.CREATE;
			}
		}
	}
}
