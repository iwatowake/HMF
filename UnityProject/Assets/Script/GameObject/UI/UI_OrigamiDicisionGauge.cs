using UnityEngine;
using System.Collections;

public class UI_OrigamiDicisionGauge : SingletonMonoBehaviour<UI_OrigamiDicisionGauge> {
	
	public enum FILLMODE 
	{
		AutoIncrease,
		AutoDecrease,
		Fixed
	}
	
	[HideInInspector]
	public			float			fillTime  	= 1.0f;
	
	private 		UIFilledSprite	sprite;
	private			bool			bFilled 	= false;
	
	private			FILLMODE		fillMode  	= FILLMODE.Fixed;
	
	public	float	GetNowTime(){
		return sprite.fillAmount * fillTime;
	}
	
	public	void	SetNowTime(float time){
		sprite.fillAmount = time / fillTime;
	}
	
	public	void	Stop(){
		sprite.fillAmount = 0;
		fillMode = FILLMODE.Fixed;
	}
	
	public	void	Pause(){
		fillMode = FILLMODE.Fixed;
	}
	
	public	void	PlayAtZero(){
		sprite.fillAmount = 0;
		fillMode = FILLMODE.AutoIncrease;
	}
	
	public	void	Play(){
		fillMode = FILLMODE.AutoIncrease;
	}
	
	public	void	PlayInverse(){
		fillMode = FILLMODE.AutoDecrease;
	}
	
	void Start(){
		sprite = gameObject.GetComponent<UIFilledSprite>();
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.F1))
			Play();
		if(Input.GetKeyDown(KeyCode.F2))
			Pause();
		if(Input.GetKeyDown(KeyCode.F3))
			Stop();
		if(Input.GetKeyDown(KeyCode.F4))
			PlayAtZero();
		if(Input.GetKeyDown(KeyCode.F5))
			PlayInverse();
		
		switch(fillMode)
		{
		case FILLMODE.AutoIncrease:
			sprite.fillAmount += Time.deltaTime / fillTime;
			if(sprite.fillAmount >= 1)
			{
				fillMode = FILLMODE.Fixed;
			}
			break;
		case FILLMODE.AutoDecrease:
			sprite.fillAmount -= Time.deltaTime / fillTime;
			if(sprite.fillAmount <= 0)
			{
				fillMode = FILLMODE.Fixed;
			}
			break;
		case FILLMODE.Fixed:
			break;
		}
	}
	
	public bool isFilled(){
		if(sprite.fillAmount >= 1.0f)
		{
			return true;
		}else{
			return false;
		}
	}
}
