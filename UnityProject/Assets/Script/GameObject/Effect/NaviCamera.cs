using UnityEngine;
using System.Collections;

public class NaviCamera : MonoBehaviour {
	float distance = 0.0f;
	float old_diastance = 0.0f;
	
	public float speedAtLookForMove = 1.0f;
	public float speedAtLookForWait = 60.0f;
	public float cameraSpeed = 0.1f;
	
	// Use this for initialization
	void Start () {
//		iTween.MoveTo(gameObject,iTween.Hash("path",iTweenPath.GetPath("StreetPath"),"time",10,"easetype",iTween.EaseType.easeOutSine));
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKey(KeyCode.W))
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
		
		// カメラが移動しているか確認.
		if(old_diastance == distance)
		{
//			iTween.LookUpdate(gameObject,iTween.Hash("looktarget",nextPathPosition,"time",speedAtLookForWait));//,"easetype", iTween.EaseType.easeInOutBack));
		}
		else
		{
			// 進行方向にカメラを向ける.
			iTween.LookUpdate(gameObject,iTween.Hash("looktarget",nextPathPosition,"time",speedAtLookForMove/*));*/,"easetype", iTween.EaseType.easeInOutBack));
//			gameObject.transform.LookAt(nextPathPosition);
//			iTween.LookUpdate(gameObject,iTween.Hash("looktarget",nextPathPosition,"time",1));//,"easetype", iTween.EaseType.easeInOutBack));
		}
		
		old_diastance = distance;
		
		// 以下はカメラを揺らす処理の残骸.
//		iTween.PunchRotation(gameObject,iTween.Hash("z",45,"time",5, "easetype", iTween.EaseType.easeInOutCubic));
//		iTween.ShakeRotation(gameObject,iTween.Hash("z",5,"time",5, "easetype", iTween.EaseType.easeInOutCubic));
	}
}
