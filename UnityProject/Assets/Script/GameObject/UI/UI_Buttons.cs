using UnityEngine;
using System.Collections;

public class UI_Buttons : MonoBehaviour_Extends {

	public	string		onPressedTarget = "";
	public	string		onPressedMethod = "";
	public	string		onPressedParam  = "";
	private UISprite	sprite;
	private	bool		isPressed = false;
	private Vector3		originalScale;
	public	bool		active			= true;
	
	void Start(){
		sprite = GetComponent<UISprite>();
		originalScale = transform.localScale;
	}
	
	void Update () {
		if(active){
			Vector3 cursorPos = UI_DicisionGauge.Instance.transform.localPosition;
	
			if( (cursorPos.x > transform.localPosition.x - originalScale.x*0.5f) &&
				(cursorPos.y < transform.localPosition.y + originalScale.y*0.5f) &&
				(cursorPos.x < (transform.localPosition.x + originalScale.x*0.5f)) && 
				(cursorPos.y > (transform.localPosition.y - originalScale.y*0.5f)))
			{
				if(!isPressed)
				{
					isPressed = true;
					UI_DicisionGauge.Instance.isOnButton = true;
					UI_DicisionGauge.Instance.RelatedButton = this;
					EffectCamera.Instance.CreateButtonEffect();
				}
			}else{
				if(isPressed)
				{
					isPressed = false;
					UI_DicisionGauge.Instance.isOnButton = false;
					UI_DicisionGauge.Instance.RelatedButton = null;
					EffectCamera.Instance.DestroyButtonEffect();
				}
			}
		}else if(isPressed)
		{
			isPressed = false;
			if(UI_DicisionGauge.Instance.RelatedButton == this)
			{
				UI_DicisionGauge.Instance.isOnButton = false;
				UI_DicisionGauge.Instance.RelatedButton = null;
				EffectCamera.Instance.DestroyButtonEffect();
			}
		}
	}
	
	public void OnPressed(){
		GameObject.Find(onPressedTarget).SendMessage(onPressedMethod,onPressedParam);
	}
}
