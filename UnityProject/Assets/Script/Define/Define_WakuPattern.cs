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
		Pattern[Index] = new WakuPattern();
		Pattern[Index].Set( arIncidence, arWakuLevel, arOriTime, arIntervalTime );
	}
}

public static class Define_WakuPattern {
	// テーブルのサイズ.
	public const int MaxRange = 45;
	public static WakuPatternTable[] Table;
	
	public const int MIN_DEGREE = 45;
	public const int MAX_DEGREE = 89;
	
	static Define_WakuPattern (){
		Table = new WakuPatternTable[MaxRange];
		uint Index = 0;
		Table[Index] = new WakuPatternTable();
		// 段階.
		Table[Index].Degree = 45;
		// パターンの数.
		Table[Index].CreatePattern( 1 );
		// 番号　出現率　レベル　折れる時間　インターバル.
		Table[Index].Set( 0, 100, WAKU.LEVEL_1_1, 12, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 46;
		Table[Index].CreatePattern( 1 );
		Table[Index].Set( 0, 100, WAKU.LEVEL_1_3, 12, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 47;
		Table[Index].CreatePattern( 1 );
		Table[Index].Set( 0, 100, WAKU.LEVEL_1_2, 12, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 48;
		Table[Index].CreatePattern( 1 );
		Table[Index].Set( 0, 100, WAKU.LEVEL_1_1, 10, 5 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 49;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 50, WAKU.LEVEL_1_2, 10, 5 );
		Table[Index].Set( 1, 50, WAKU.LEVEL_1_4, 10, 5 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 50;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 50, WAKU.LEVEL_1_3, 10, 5 );
		Table[Index].Set( 1, 50, WAKU.LEVEL_1_1,  8, 5 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 51;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 40, WAKU.LEVEL_1_2, 10, 5 );
		Table[Index].Set( 1, 60, WAKU.LEVEL_1_4, 10, 5 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 52;
		Table[Index].CreatePattern( 1 );
		Table[Index].Set( 0, 100, WAKU.LEVEL_1_4, 8, 5 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 53;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 70, WAKU.LEVEL_1_5, 10, 5 );
		Table[Index].Set( 1, 30, WAKU.LEVEL_1_1,  7, 5 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 54;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 80, WAKU.LEVEL_2_3, 10, 5 );
		Table[Index].Set( 1, 20, WAKU.LEVEL_1_2,  7, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 55;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 50, WAKU.LEVEL_2_1,  8, 5 );
		Table[Index].Set( 1, 50, WAKU.LEVEL_2_2,  8, 5 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 56;
		Table[Index].CreatePattern( 3 );
		Table[Index].Set( 0, 20, WAKU.LEVEL_2_4,  5, 5 );
		Table[Index].Set( 1, 40, WAKU.LEVEL_1_1,  7, 5 );
		Table[Index].Set( 2, 40, WAKU.LEVEL_2_5,  7, 5 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 57;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 80, WAKU.LEVEL_2_3,  7, 5 );
		Table[Index].Set( 1, 20, WAKU.LEVEL_3_1,  10, 5 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 58;
		Table[Index].CreatePattern( 3 );
		Table[Index].Set( 0, 10, WAKU.LEVEL_3_2,   7, 3 );
		Table[Index].Set( 1, 50, WAKU.LEVEL_3_3,  10, 5 );
		Table[Index].Set( 2, 40, WAKU.LEVEL_2_2,   5, 5 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 59;
		Table[Index].CreatePattern( 1 );
		Table[Index].Set( 0, 100, WAKU.LEVEL_3_4,  7, 5 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 60;
		Table[Index].CreatePattern( 3 );
		Table[Index].Set( 0, 10, WAKU.LEVEL_4_4,  5, 5 );
		Table[Index].Set( 1, 40, WAKU.LEVEL_3_5,  7, 3 );
		Table[Index].Set( 2, 50, WAKU.LEVEL_4_1,  8, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 61;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 50, WAKU.LEVEL_4_2,  7, 5 );
		Table[Index].Set( 1, 50, WAKU.LEVEL_4_3,  7, 5 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 62;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 50, WAKU.LEVEL_3_2,  5, 5 );
		Table[Index].Set( 1, 50, WAKU.LEVEL_2_1,  7, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 63;
		Table[Index].CreatePattern( 3 );
		Table[Index].Set( 0, 30, WAKU.LEVEL_3_1,  5, 3 );
		Table[Index].Set( 1, 40, WAKU.LEVEL_4_4,  8, 5 );
		Table[Index].Set( 2, 30, WAKU.LEVEL_4_5,  8, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 64;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 80, WAKU.LEVEL_5_1, 10, 5 );
		Table[Index].Set( 1, 20, WAKU.LEVEL_5_1,  7, 5 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 65;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 50, WAKU.LEVEL_5_2,  7, 3 );
		Table[Index].Set( 1, 50, WAKU.LEVEL_4_1,  7, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 66;
		Table[Index].CreatePattern( 3 );
		Table[Index].Set( 0, 20, WAKU.LEVEL_5_3,  6, 3 );
		Table[Index].Set( 1, 60, WAKU.LEVEL_4_2,  5, 2 );
		Table[Index].Set( 2, 20, WAKU.LEVEL_5_4, 10, 5 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 67;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 60, WAKU.LEVEL_5_3,  7, 3 );
		Table[Index].Set( 1, 40, WAKU.LEVEL_5_4,  7, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 68;
		Table[Index].CreatePattern( 3 );
		Table[Index].Set( 0, 20, WAKU.LEVEL_1_4,  4, 3 );
		Table[Index].Set( 1, 40, WAKU.LEVEL_3_5,  5, 2 );
		Table[Index].Set( 2, 40, WAKU.LEVEL_5_5,  7, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 69;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 50, WAKU.LEVEL_3_3,  4, 3 );
		Table[Index].Set( 1, 50, WAKU.LEVEL_5_2,  5, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 70;
		Table[Index].CreatePattern( 1 );
		Table[Index].Set( 0, 100, WAKU.LEVEL_6_1,  7, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 71;
		Table[Index].CreatePattern( 3 );
		Table[Index].Set( 0, 10, WAKU.LEVEL_4_3,  5, 3 );
		Table[Index].Set( 1, 50, WAKU.LEVEL_6_2, 10, 3 );
		Table[Index].Set( 2, 40, WAKU.LEVEL_1_5,  4, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 72;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 50, WAKU.LEVEL_6_3, 10, 3 );
		Table[Index].Set( 1, 50, WAKU.LEVEL_6_4, 10, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree =73;
		Table[Index].CreatePattern( 3 );
		Table[Index].Set( 0, 30, WAKU.LEVEL_5_5,  7, 3 );
		Table[Index].Set( 1, 40, WAKU.LEVEL_6_5, 10, 5 );
		Table[Index].Set( 2, 30, WAKU.LEVEL_6_2,  7, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 74;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 50, WAKU.LEVEL_5_1,  5, 5 );
		Table[Index].Set( 1, 50, WAKU.LEVEL_5_3,  5, 5 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 75;
		Table[Index].CreatePattern( 3 );
		Table[Index].Set( 0, 20, WAKU.LEVEL_3_1,  5, 3 );
		Table[Index].Set( 1, 60, WAKU.LEVEL_7_1, 10, 3 );
		Table[Index].Set( 2, 20, WAKU.LEVEL_7_1,  7, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 76;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 80, WAKU.LEVEL_5_1,  5, 5 );
		Table[Index].Set( 1, 20, WAKU.LEVEL_7_2, 10, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 77;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 50, WAKU.LEVEL_7_2,  8, 5 );
		Table[Index].Set( 1, 50, WAKU.LEVEL_6_2,  7, 5 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 78;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 70, WAKU.LEVEL_7_3, 10, 3 );
		Table[Index].Set( 1, 30, WAKU.LEVEL_6_5,  7, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 79;
		Table[Index].CreatePattern( 3 );
		Table[Index].Set( 0, 20, WAKU.LEVEL_6_3,  7, 5 );
		Table[Index].Set( 1, 40, WAKU.LEVEL_5_5,  6, 3 );
		Table[Index].Set( 2, 40, WAKU.LEVEL_7_4, 10, 5 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 80;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 50, WAKU.LEVEL_4_5,  7, 2 );
		Table[Index].Set( 1, 50, WAKU.LEVEL_7_5, 10, 5 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 81;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 60, WAKU.LEVEL_6_4,  7, 3 );
		Table[Index].Set( 1, 40, WAKU.LEVEL_4_2,  6, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 82;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 80, WAKU.LEVEL_7_1,  7, 3 );
		Table[Index].Set( 1, 20, WAKU.LEVEL_2_1,  5, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 83;
		Table[Index].CreatePattern( 1 );
		Table[Index].Set( 0, 100, WAKU.LEVEL_7_3,  7, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 84;
		Table[Index].CreatePattern( 1 );
		Table[Index].Set( 0, 100, WAKU.LEVEL_7_4, 7, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 85;
		Table[Index].CreatePattern( 3 );
		Table[Index].Set( 0, 10, WAKU.LEVEL_7_1,  5, 5 );
		Table[Index].Set( 1, 70, WAKU.LEVEL_7_5,  7, 3 );
		Table[Index].Set( 2, 20, WAKU.LEVEL_7_2,  6, 3 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 86;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 50, WAKU.LEVEL_1_2,  4, 3 );
		Table[Index].Set( 1, 50, WAKU.LEVEL_6_5,  6, 5 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 87;
		Table[Index].CreatePattern( 2 );
		Table[Index].Set( 0, 70, WAKU.LEVEL_7_2,  6, 2 );
		Table[Index].Set( 1, 30, WAKU.LEVEL_7_3,  6, 2 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 88;
		Table[Index].CreatePattern( 1 );
		Table[Index].Set( 0, 100, WAKU.LEVEL_7_5,  6, 2 );
		
		Index++;
		Table[Index] = new WakuPatternTable();
		Table[Index].Degree = 89;
		Table[Index].CreatePattern( 1 );
		Table[Index].Set( 0, 100, WAKU.LEVEL_7_4,  6, 2 );
		
	}
	
}
