using UnityEngine;
using System.Collections;

public class UI_ResultManager : SingletonMonoBehaviour<UI_ResultManager>{
	
	public	UI_Label_Fade	lbFdScore;
	public	UI_Sprite_Fade	spFdNewRecord;
	public	UI_InputChars	inputChars;
	public	UI_InputArea	inputArea;
	public	UISprite		spNextButton;
	
	public	bool SetScore(){
		lbFdScore.gameObject.GetComponent<UILabel>().text = "Score " + StateController.Instance.score.ToString();
		
		return false;
	}
}
