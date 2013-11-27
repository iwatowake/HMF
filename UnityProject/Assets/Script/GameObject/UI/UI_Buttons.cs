using UnityEngine;
using System.Collections;

public class UI_Buttons : MonoBehaviour_Extends {

	public	string		onPressedTarget = "";
	public	string		onPressedMethod = "";
	public	string		onPressedParam  = "";
	private UISprite	sprite;
	private	bool		isPressed = false;
	private Vector3		originalScale;
	private	UI_DicisionGauge	dicisionGauge;
	public	bool		active			= true;
	
	void Start(){
		sprite = GetComponent<UISprite>();
		originalScale = transform.localScale;
	}
	
	void Update () {
		if(active){
			if(dicisionGauge == null)
			{
				dicisionGauge = GameObject.Find("UI_Cursor").GetSafeComponent<UI_DicisionGauge>();
			}
			
			Vector3 cursorPos = dicisionGauge.transform.localPosition;
	
			if( (cursorPos.x > transform.localPosition.x - originalScale.x*0.5f) &&
				(cursorPos.y < transform.localPosition.y + originalScale.y*0.5f) &&
				(cursorPos.x < (transform.localPosition.x + originalScale.x*0.5f)) && 
				(cursorPos.y > (transform.localPosition.y - originalScale.y*0.5f)))
			{
				if(!isPressed)
				{
					InstantiateGameObjectAsChild("Prefabs/Test_Yanagisawa/GaugeAppearEffect", dicisionGauge.gameObject, -Vector3.forward*2, true);
					isPressed = true;
					dicisionGauge.isOnButton = true;
					dicisionGauge.RelatedButton = this;
					EffectCamera.Instance.CreateButtonEffect();
				}
			}else{
				if(isPressed)
				{
					isPressed = false;
					dicisionGauge.isOnButton = false;
					dicisionGauge.RelatedButton = null;
					EffectCamera.Instance.DestroyButtonEffect();
				}
			}
		}else if(isPressed)
		{
			isPressed = false;
			if(dicisionGauge.RelatedButton == this)
			{
				dicisionGauge.isOnButton 	= false;
				dicisionGauge.RelatedButton = null;
				EffectCamera.Instance.DestroyButtonEffect();
			}
		}
	}
	
	public void OnPressed(){
		if(GetComponentInChildren<StringSelectEffect>() != null)
			GetComponentInChildren<StringSelectEffect>().Play();
		GameObject.Find(onPressedTarget).SendMessage(onPressedMethod,onPressedParam);
	}
}
