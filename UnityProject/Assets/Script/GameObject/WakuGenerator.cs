using UnityEngine;
using System.Collections;

public class WakuGenerator : SingletonMonoBehaviour<WakuGenerator> {
	
	enum STATE {
		CREATE,
		WAIT,
		INTERVAL,
	};
	STATE State = STATE.CREATE;
	
	OrigamiController OrigamiControllerScript = null;
	
	[HideInInspector]
	public int	NowDegree = 3;
	[HideInInspector]
	public int	CutTime;
	
	public	int	IntervalTime = 5;
	public	int	OriTime = 30;
	private readonly int[] DegreeAddTable = new int[4]{ -1,-1,1,2 };
	private int Timer = 0;
	
	public void Init (){
		NowDegree = 3;
		OrigamiControllerScript = GameObject.Find( "OrigamiController" ).GetComponent<OrigamiController>();
	}
	
	public void SetResult ( int Result ){
		if( Result >= DegreeAddTable.Length )	return;
		NowDegree += DegreeAddTable[Result];
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
				Define_WakuPattern.Table[Index].Pattern[SelectIndex].WakuLevel, OriTime );
			
			
			State = STATE.WAIT;
		}
		else if( State == STATE.WAIT ){
			if( OrigamiControllerScript.GetActiveFlg() ) return;
			
			Timer = 0;
			State = STATE.INTERVAL;
		}
		else{
			if( StaticMath.Compensation( ref Timer, IntervalTime, 1 ) ){
				State = STATE.CREATE;
			}
		}
	}
}
