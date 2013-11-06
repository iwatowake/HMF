using UnityEngine;
using System.Collections;

public enum WAKU{
	LEVEL_1_1,
	LEVEL_1_2,
	LEVEL_1_3,
	LEVEL_1_4,
	LEVEL_1_5,
	
	LEVEL_2_1,
	LEVEL_2_2,
	LEVEL_2_3,
	LEVEL_2_4,
	LEVEL_2_5,
	
	LEVEL_3_1,
	LEVEL_3_2,
	LEVEL_3_3,
	LEVEL_3_4,
	LEVEL_3_5,
	
	LEVEL_4_1,
	LEVEL_4_2,
	LEVEL_4_3,
	LEVEL_4_4,
	LEVEL_4_5,
	
	LEVEL_5_1,
	LEVEL_5_2,
	LEVEL_5_3,
	LEVEL_5_4,
	LEVEL_5_5,
	
	LEVEL_6_1,
	LEVEL_6_2,
	LEVEL_6_3,
	LEVEL_6_4,
	LEVEL_6_5,
	
	LEVEL_7_1,
	LEVEL_7_2,
	LEVEL_7_3,
	LEVEL_7_4,
	LEVEL_7_5,
	NUM
};

public class WakuPattern {
	public float	Incidence;
	public WAKU		WakuLevel;
	public float	OriTime;
	public float	IntervalTime;
	
	public void Set ( float arIncidence, WAKU arWakuLevel, float arOriTime, float arIntervalTime ){
		Incidence = arIncidence;
		WakuLevel = arWakuLevel;
		OriTime = arOriTime;
		IntervalTime = arIntervalTime;
	}
}

public class WakuPatternTable {
	public int				Degree;
	public WakuPattern[]	Pattern;
	
	public void CreatePattern ( uint Num ){
		Pattern = new WakuPattern[Num];
	}
	
	public void Set ( uint Index, float arIncidence, WAKU arWakuLevel, float arOriTime, float arIntervalTime ){
		if( Index >= Pattern.Length ){
			Debug.Log( "Out of Index" );
			return;
		}
		Pattern[Index].Set( arIncidence, arWakuLevel, arOriTime, arIntervalTime );
	}
}

public static class Define_WakuPattern {
	// テーブルのサイズ.
	public const int MaxRange = 45;
	public static WakuPatternTable[] Table = new WakuPatternTable[MaxRange];
	
	static Define_WakuPattern (){
		uint Index = 0;
		// 段階.
		Table[Index].Degree = 45;
		// パターンの数.
		Table[Index].CreatePattern( 1 );
		// 番号　出現率　レベル　折れる時間　インターバル.
		Table[Index].Set( 0, 100, WAKU.LEVEL_1_1, 12, 3 );
		
		Index++;
		Table[Index].Degree = 46;
		Table[Index].CreatePattern( 1 );
		Table[Index].Set( 0, 100, WAKU.LEVEL_1_3, 12, 3 );
		
		Index++;
		Table[Index].Degree = 47;
		Table[Index].CreatePattern( 1 );
		Table[Index].Set( 0, 100, WAKU.LEVEL_1_2, 12, 3 );
		
		Index++;
		Table[Index].Degree = 48;
		Table[Index].CreatePattern( 1 );
		Table[Index].Set( 0, 100, WAKU.LEVEL_1_1, 10, 5 );
		
		Index++;
		Table[Index].Degree = 49;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 50, WAKU.LEVEL_1_2, 10, 5 );
		Table[Index].Set( 1, 50, WAKU.LEVEL_1_4, 10, 5 );
	}
	
}
