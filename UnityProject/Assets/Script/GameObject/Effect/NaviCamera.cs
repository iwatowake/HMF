using UnityEngine;
using System.Collections;

public class NaviCamera : SingletonMonoBehaviour<NaviCamera> {
	public float fieldOfViewInMove = 40;
	public float fieldOfViewInLook = 60;
	public float fieldOfViewTime = 1.0f;
	
		// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	#region カメラの一時停止して世界の中心を注視する.
	public void LookTownPauseTween()
	{
		//iTween.Pause(gameObject);
		iTweenEvent.GetEvent(gameObject,"LookTown").Play();
		ChangeFieldOfView();
	}
	#endregion
	
	#region 移動を再開する.
	void ResumeStreet()
	{
		ResumeFieldOfView();
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

	#region CameraのFieldOfViewをTweenで変更.
	public void ChangeFieldOfView()
	{
		iTween.ValueTo(gameObject, iTween.Hash("from", fieldOfViewInMove, "to", fieldOfViewInLook, "time", fieldOfViewTime, "onupdate", "SetFieldOfView"));		
	}
	
	public void ResumeFieldOfView()
	{
		iTween.ValueTo(gameObject, iTween.Hash("from", fieldOfViewInLook, "to", fieldOfViewInMove, "time", fieldOfViewTime, "onupdate", "SetFieldOfView"));		
	}		
	
	private void SetFieldOfView(float value)
	{
		this.camera.fieldOfView = value;
	}
	#endregion
}
