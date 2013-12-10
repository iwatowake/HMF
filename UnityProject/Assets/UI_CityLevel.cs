using UnityEngine;
using System.Collections;

public class UI_CityLevel : SingletonMonoBehaviour<UI_CityLevel> {

	enum STATE{
		eCityLevel_Init = 0,
		eCityLevel_Wait,
		
		eUp_Init,
		eUp_Wait,
		
		eAllFadeOut,
		
		eEnumEnd
	}
	
	public	UI_Label_Fade	lbFd_CityLebel;
	public	UI_Label_Fade	lbFd_Up;
	private	STATE state = STATE.eEnumEnd;

	void Update () {
		switch(state)
		{
		case STATE.eCityLevel_Init:
			lbFd_CityLebel.FadeIn(0.5f,0.5f,"OnComplete",gameObject);
			state++;
			break;
		case STATE.eCityLevel_Wait:
			break;
			
		case STATE.eUp_Init:
			lbFd_Up.FadeOut(0.5f,0.0f);
			lbFd_Up.transform.localScale = new Vector3(120,120,1);
			iTweenEvent.GetEvent(lbFd_Up.gameObject,"Scaling").Play();
			iTweenEvent.GetEvent(lbFd_CityLebel.gameObject,"Scaling_1").Play();
			state++;
			break;
		case STATE.eUp_Wait:
			state++;
			break;
			
		case STATE.eAllFadeOut:
			lbFd_CityLebel.FadeOut(0.5f, 1.0f);
			state++;
			break;
			
		case STATE.eEnumEnd:
			break;
		}
			
	}
	
	public	void Play(){
		if(state == STATE.eEnumEnd)
			state = STATE.eCityLevel_Init;
	}
	
	void OnComplete(){
		state++;
	}
	
	void SetScaling_2(){
		iTweenEvent.GetEvent(lbFd_CityLebel.gameObject, "Scaling_2").Play();
	}
}
