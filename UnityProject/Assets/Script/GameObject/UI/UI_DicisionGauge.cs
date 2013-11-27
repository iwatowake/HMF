using UnityEngine;
using System.Collections;

public class UI_DicisionGauge : MonoBehaviour_Extends {
	
	private const	float			fillSpeed = 0.5f;
	
	private 		UIFilledSprite	sprite;
	private			bool			bIsOnButton = false;
	private			bool			bFilled = false;
	private			UI_Buttons		relatedButton;
	
	public			GameObject		GaugeLasrWaveEffect;
	
	public bool isOnButton{
		get{return bIsOnButton;}
		set{bIsOnButton = value;}
	}
	
	public UI_Buttons RelatedButton{
		get{return relatedButton;}
		set{relatedButton = value;}
	}
	
	
	void Start(){
		sprite = gameObject.GetComponent<UIFilledSprite>();

	}
	
	void Update () {
		if(bIsOnButton)
		{
			if( (sprite.fillAmount >= 1.0f) && !bFilled)
			{
				bFilled = true;
				relatedButton.OnPressed();
				EffectCamera.Instance.DestroyButtonEffect();
				
				Instantiate(GaugeLasrWaveEffect,gameObject.transform.position,Quaternion.identity);
			}else{
				sprite.fillAmount += (Time.deltaTime * fillSpeed);
			}
		}else{
			bFilled = false;
			sprite.fillAmount = 0;
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
