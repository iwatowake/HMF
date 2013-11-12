using UnityEngine;
using System.Collections;

public class UI_Buttons : MonoBehaviour_Extends {

	public	string		onPressedTarget = "";
	public	string		onPressedMethod = "";
	public	string		onPressedParam  = "";
	private UISprite	sprite;
	private	bool		isPressed = false;
	private Vector3		originalScale;
	
	void Start(){
		sprite = GetComponent<UISprite>();
		originalScale = transform.localScale;
	}
	
	void Update () {
		Vector3 cursorPos = UI_DicisionGauge.Instance.transform.localPosition;

		if( (cursorPos.x > transform.localPosition.x) &&
			(cursorPos.y < transform.localPosition.y) &&
			(cursorPos.x < (transform.localPosition.x + originalScale.x)) && 
			(cursorPos.y > (transform.localPosition.y - originalScale.y)))
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
	}
	
	public void OnPressed(){
		GameObject.Find(onPressedTarget).SendMessage(onPressedMethod,onPressedParam);
	}
}
