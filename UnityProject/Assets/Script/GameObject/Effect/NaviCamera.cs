using UnityEngine;
using System.Collections;

public class NaviCamera : MonoBehaviour {
	float distance = 0.0f;
	float old_diastance = 0.0f;
	
//	public float speedAtLookForMove = 1.0f;
//	public float speedAtLookForWait = 60.0f;
//	public float cameraSpeed = 0.1f;
	
	// Use this for initialization
	void Start () {
//		iTween.MoveTo(gameObject,iTween.Hash("path",iTweenPath.GetPath("StreetPath"),"time",10,"easetype",iTween.EaseType.easeOutSine));
	}
	
	// Update is called once per frame
	void Update () {
/*        if(Input.GetKey(KeyCode.W))
            distance += cameraSpeed;
        if(Input.GetKey(KeyCode.S))
            distance -= cameraSpeed;
				
		// 進行中の距離と全体の距離の割合を算出.
		float pathLength = iTween.PathLength(iTweenPath.GetPath("StreetPath"));
	    float percent = distance / pathLength;
	    if(percent < 0.0f) percent = 0.0f;
	    if(percent > 1.0f) percent = 1.0f;
	    iTween.PutOnPath(gameObject ,iTweenPath.GetPath("StreetPath"),percent);
	    Vector3 nextPathPosition = iTween.PointOnPath(iTweenPath.GetPath("StreetPath"),percent+0.1f);

*/
		// カメラが移動しているか確認.
		if(old_diastance == distance)
		{
		}
		
		old_diastance = distance;
		
	}

	#region iTweenのOnCompleteで使う関数一覧.
	void SetStreet1()
	{
		iTweenEvent.GetEvent(gameObject,"StreetEvent1").Play();
	}
	void SetStreet2()
	{
		iTweenEvent.GetEvent(gameObject,"StreetEvent2").Play();
	}
	void SetStreet3()
	{
		iTweenEvent.GetEvent(gameObject,"StreetEvent3").Play();
	}
	void SetStreet4()
	{
		iTweenEvent.GetEvent(gameObject,"StreetEvent4").Play();
	}
	#endregion
}
