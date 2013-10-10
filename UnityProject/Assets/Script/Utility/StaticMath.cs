using UnityEngine;
using System.Collections;
using System.Collections.Generic;

static public class StaticMath{
	
	static public bool BetweenSet(ref float subject, float FirstVal, float EndVal)
	{
		if(subject < FirstVal){
			// 押し込んだ場合false
			subject = FirstVal;
			return false;
		}
		if(subject > EndVal){
			// 押し込んだ場合false
			subject = EndVal;
			return false;
		}
	
		// 押し込まなかった場合true
		return true;
	}
	
	static public bool BetweenSet(ref int subject, int FirstVal, int EndVal)
	{
		if(subject < FirstVal){
			// 押し込んだ場合false
			subject = FirstVal;
			return false;
		}
		if(subject > EndVal){
			// 押し込んだ場合false
			subject = EndVal;
			return false;
		}
	
		// 押し込まなかった場合true
		return true;
	}
	
	// subjectの値がtargetになるようalteration分だけ変化させる 
	static public bool Compensation(ref float subject, float target, float arAlteration)
	{	
		// 変位を正規化しておく 
		float alteration = Mathf.Abs(arAlteration);
	
		if(subject == target){
			// 値が同じ場合はtrueを返す
			return true;
		}
		else if(subject > target){
			subject -= alteration;
			if(subject < target){
				// 目標の値を通り越せば、目標値に値を変更させtrueを返す
				subject = target;
				return true;
			}
		}
		else if(subject < target){
			subject += alteration;
			if(subject > target){
				// 目標の値を通り越せば、目標値に値を変更させtrueを返す
				subject = target;
				return true;
			}
		}
		return false;
	}
	
	// subjectの値がtargetになるようalteration分だけ変化させる 
	static public bool Compensation(ref int subject, int target, int arAlteration)
	{	
		// 変位を正規化しておく 
		int alteration = Mathf.Abs(arAlteration);
	
		if(subject == target){
			// 値が同じ場合はtrueを返す
			return true;
		}
		else if(subject > target){
			subject -= alteration;
			if(subject < target){
				// 目標の値を通り越せば、目標値に値を変更させtrueを返す
				subject = target;
				return true;
			}
		}
		else if(subject < target){
			subject += alteration;
			if(subject > target){
				// 目標の値を通り越せば、目標値に値を変更させtrueを返す
				subject = target;
				return true;
			}
		}
		return false;
	}
	
	static public int RandomPlusOrMinus(){
		if(Random.value < 0.5f){
			return -1;
		}
		return 1;
	}
	
	
	// 多角形との当たり判定(2D).
	// 当たっている場合はtrueを返す.
	static public bool CheckInPolygon2D ( Vector2[] Polygon, int NumPolygon, Vector2 Pos ){
		
		float	L_max, L_min;
		float	R_max, R_min;
		L_max = R_max = -float.MaxValue;
		L_min = R_min = float.MaxValue;
		
		List<float> L_tanList = new List<float>();
		List<float> R_tanList = new List<float>();
		
		// 転写した点の最大最小値を調べ、Tangentを保存しておく.
		Vector2	V;
		float Tangent;
		for( int i = 0; i < NumPolygon; i++ )
		{
			V = Pos - Polygon[i];
			if( V.x < 0.0f )
			{
				Tangent = V.y / -V.x;
				if( L_max < Tangent )
					L_max = Tangent;
				if( L_min > Tangent )
					L_min = Tangent;
				L_tanList.Add( -Tangent );
			}
			else if( V.x > 0.0f )
			{
				Tangent = V.y / V.x;
				if( R_max < Tangent )
					R_max = Tangent;
				if( R_min > Tangent )
					R_min = Tangent;
				R_tanList.Add( -Tangent );
			}
		}

		// 範囲内に含まれているか.
		for( int i = 0; i < L_tanList.Count; i++ )
		{
			if( L_tanList[i] < R_max && L_tanList[i] > R_min )
			{
				L_tanList.Clear();
				R_tanList.Clear();
				return true;
			}
		}
		for( int i = 0; i < R_tanList.Count; i++ )
		{
			if( R_tanList[i] < L_max && R_tanList[i] > L_min )
			{
				L_tanList.Clear();
				R_tanList.Clear();
				return true;
			}
		}

		L_tanList.Clear();
		R_tanList.Clear();
	
		return false;
	}
	
	
	// 多角形と多角形の当たり判定(2D).
	// 当たっている場合はtrueを返す.
	static public bool CheckInPolygon2D ( Vector2[] Polygon1, int NumPolygon1, Vector2[] Polygon2, int NumPolygon2 ){
		
		float	L_max, L_min;
		float	R_max, R_min;
		L_max = R_max = -float.MaxValue;
		L_min = R_min = float.MaxValue;
		
		List<float> L_tanList = new List<float>();
		List<float> R_tanList = new List<float>();
		
		// 転写した点の最大最小値を調べ、Tangentを保存しておく.
		Vector2	V;
		float Tangent;
		for( int i = 0; i < NumPolygon1; i++ )
		{
			for( int j = 0; j < NumPolygon2; j++ )
			{
				V = Polygon2[j] - Polygon1[i];
				if( V.x < 0.0f )
				{
					Tangent = V.y / -V.x;
					if( L_max < Tangent )
						L_max = Tangent;
					if( L_min > Tangent )
						L_min = Tangent;
					L_tanList.Add( -Tangent );
				}
				else if( V.x > 0.0f )
				{
					Tangent = V.y / V.x;
					if( R_max < Tangent )
						R_max = Tangent;
					if( R_min > Tangent )
						R_min = Tangent;
					R_tanList.Add( -Tangent );
				}
			}
		}

		// 範囲内に含まれているか.
		for( int i = 0; i < L_tanList.Count; i++ )
		{
			if( L_tanList[i] < R_max && L_tanList[i] > R_min )
			{
				L_tanList.Clear();
				R_tanList.Clear();
				return true;
			}
		}
		for( int i = 0; i < R_tanList.Count; i++ )
		{
			if( R_tanList[i] < L_max && R_tanList[i] > L_min )
			{
				L_tanList.Clear();
				R_tanList.Clear();
				return true;
			}
		}

		L_tanList.Clear();
		R_tanList.Clear();
	
		return false;
	}
	
	// 引数にint型の値と,最大の桁数を渡せば金額用のカンマをつけたstring型を右詰めで返す.
	static public string MoneyString(int arMoney, int arDigit){
		string	DrawStr			= "";
		string	StrMoney		= arMoney.ToString ();
		int		Digit			= arDigit + (int)(arDigit/3);
		int		StrNo			= 0;
		int 	CommaSpaceCount	= 0;
		
		for(int i=0; i<Digit; i++){
			if(StrNo < StrMoney.Length){
				string OneStr = StrMoney.Substring(StrMoney.Length-StrNo-1, 1);
				DrawStr = OneStr + DrawStr;
				StrNo++;
				
				if(StrNo%3==0
				&& StrMoney.Length-StrNo-1 >= 0){
					if(i<Digit-2){
						CommaSpaceCount++;
						DrawStr = "," + DrawStr;
					}
					else{
						DrawStr = " " + DrawStr;
					}
					i++;
				}
			}
			else{
				DrawStr = " " + DrawStr;
			}
		}
		for(int i=0; i<CommaSpaceCount; i++){
		//	DrawStr = "Z" + DrawStr;
		}
		
		return DrawStr;
	}
	
	// 引数にint型の値を渡せば金額用のカンマをつけたstring型を左詰めで返す.
	static public string MoneyStringLeft(int arMoney){
		string	DrawStr			= "";
		string	StrMoney		= arMoney.ToString ();
		
		for(int i=0; i<StrMoney.Length; i++){
			string OneStr = StrMoney.Substring(i, 1);
			DrawStr = DrawStr + OneStr;
			
			if((StrMoney.Length-i-1)%3==0
			&& i < StrMoney.Length-1){
				DrawStr = DrawStr + ",";
			}
		}
		return DrawStr;
	}
	
	// 円と円の当たり判定を取る.
	static public bool CheckHitCircle(Vector3 p1, float r1, Vector3 p2, float r2){
		Vector3 Temp = p1;
		Temp.z = p2.z;
		p1 = Temp;
		
		float distance = Vector3.Distance(p1, p2);
		if(distance < r1+r2){
			return true;
		}
		return false;
	}
	
	static public float RayDistancePoint( Vector3 Start, Vector3 End, Vector3 Point, ref Vector3 CrossPoint )
	{
		Vector3	LineVec = End - Start;
		Vector3	PointToStartVec = Point - Start;
		float	r2, t;
		
	    if ( Vector3.Distance(End,Start) == 0.0f )
	        return 0.0f;
		
	    r2 = Vector3.Dot( LineVec, LineVec );
	    t = Vector3.Dot( LineVec, PointToStartVec ) / r2;
		
	    CrossPoint.x = (1 - t) * Start.x + t * End.x;
	    CrossPoint.y = (1 - t) * Start.y + t * End.y;
	    CrossPoint.z = (1 - t) * Start.z + t * End.z;
	    return Vector3.Distance( CrossPoint, Point );
	}
	
	// 線分の交点 xz平面上.
	static public bool RayCrossPoint( Vector3 Start1, Vector3 End1, Vector3 Start2, Vector3 End2, ref Vector3 CrossPoint )
	{
		Vector3	LineVec1 = End1-Start1;
		Vector3	LineVec2 = End2-Start2;
		Vector3 v = Start2 - Start1;
		float Crs_v1_v2 = Vector3.Cross( LineVec1, LineVec2 ).y;
		if ( Crs_v1_v2 == 0.0f ) {
		  // 平行状態.
		  return false;
		}
		
		float Crs_v_v1 = Vector3.Cross( v, LineVec1 ).y;
		float Crs_v_v2 = Vector3.Cross( v, LineVec2 ).y;
		
		float t1 = Crs_v_v2 / Crs_v1_v2;
		float t2 = Crs_v_v1 / Crs_v1_v2;
		
		const float eps = 0.00001f;
		if ( t1 + eps < 0 || t1 - eps > 1 || t2 + eps < 0 || t2 - eps > 1 ) {
		  // 交差していない.
		  return false;
		}
		
		CrossPoint = Start1 + LineVec1 * t1;
	
	   return true;
	}
	
	static public bool Vector3Comparing	( Vector3 in1, Vector3 in2 ){
		if( in1.x == in2.x && in1.y == in2.y && in1.z == in2.z ){
			return true;
		}
		return false;
	}
	
}
