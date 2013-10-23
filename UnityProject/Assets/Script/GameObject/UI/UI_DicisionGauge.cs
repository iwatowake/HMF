using UnityEngine;
using System.Collections;

public class UI_DicisionGauge : SingletonMonoBehaviour<UI_DicisionGauge> {
	
	public	float			fillSpeed = 1.0f;

	private UIFilledSprite	sprite;
	private	bool			bIsOnButton = false;
	private	bool			bFilled = false;
	private	UI_Buttons		relatedButton;
	
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
		transform.localPosition = Input.mousePosition - new Vector3(Screen.width/2, Screen.height/2, 0);
		
		if(bIsOnButton)
		{
			if( (sprite.fillAmount >= 1.0f) && !bFilled)
			{
				bFilled = true;
				relatedButton.OnPressed();
			}else{
				sprite.fillAmount += (Time.deltaTime * fillSpeed);
			}
		}else{
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
