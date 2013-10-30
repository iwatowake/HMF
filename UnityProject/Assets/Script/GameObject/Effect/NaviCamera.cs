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
		// カメラが移動しているか確認.
		if(old_diastance == distance)
		{
		}
		old_diastance = distance;
		
		if(Input.GetKeyDown(KeyCode.Z))	
			LookTownPauseTween();
							
	}
	#region カメラの一時停止して世界の中心を注視する.
	void LookTownPauseTween()
	{
		iTween.Pause(gameObject);
		iTweenEvent.GetEvent(gameObject,"LookTown").Play();
	}
	#endregion
	
	#region 移動を再開する.
	void ResumeStreet()
	{
		iTween.Resume(gameObject);
	}
	#endregion
		
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
