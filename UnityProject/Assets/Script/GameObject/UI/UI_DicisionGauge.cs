using UnityEngine;
using System.Collections;

public class UI_DicisionGauge : SingletonMonoBehaviour<UI_DicisionGauge> {
	
	public	float			fillSpeed = 1.0f;
	public	bool			mouseMode = false;
	
	private UIFilledSprite	sprite;
	private	GameObject[]	palmObj;
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
		palmObj = new GameObject[2];
		
		LeapUnityHandController hands = GameObject.Find("Leap Hands").GetComponent<LeapUnityHandController>();
		
		palmObj[0] = hands.m_palms[0];
		palmObj[1] = hands.m_palms[0];
	}
	
	void Update () {
		if(mouseMode)
		{
			transform.localPosition = Input.mousePosition - new Vector3(Screen.width/2, Screen.height/2, 0);
		}else{
			for(int i=0; i<2; i++)
			{
				if(palmObj[i].GetComponent<LeapOrigamiCollider>().enabled)
				{
					Vector2 pos = palmObj[i].GetComponent<HandObjectController>().GetScreenPos();
					transform.localPosition = new Vector3(pos.x, pos.y, 0) - new Vector3(Screen.width/2, Screen.height/2, 0);
				}
			}
		//	transform.localPosition = GameObject.Find() - new Vector3(Screen.width/2, Screen.height/2, 0);
		}
		
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
