using UnityEngine;
using System.Collections;

public class TutorialText : MonoBehaviour {
	public string[] text;
	public GameObject tutorialState;
	public GameObject tutorialMovie;
	
	private OrigamiController OrigamiControllerScript = null;

	int currentNo = 0;
	
	// Use this for initialization
	void Start () {
		OrigamiControllerScript = GameObject.Find( "OrigamiController" ).GetComponent<OrigamiController>();
		gameObject.transform.localScale = Vector3.zero;
		OpenText();
		
		MovieTexture tex = (MovieTexture)tutorialMovie.renderer.material.mainTexture;
		tex.loop = true;
		tex.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if(currentNo < text.Length)
		{
			TextMesh tm = (TextMesh)gameObject.GetComponent("TextMesh");
			tm.text = text[currentNo];
		}
	}
	
	void Restart()
	{
		currentNo = 1;
	}
	
	void OpenText()
	{
		iTweenEvent.GetEvent(gameObject,"OpenTextTween").Play();
	}
	
	void IdleText()
	{
		switch(currentNo)
		{
		// 折るための始点と終点の線を引く説明.
		case 5:
			OrigamiControllerScript.DisableClap();
			tutorialMovie.SetActive(false);
			if(OrigamiControllerScript.GetState() == OrigamiUpdate.STATE.FOLD_SELECT)
				NextText();
//			iTweenEvent.GetEvent(gameObject,"IdleTextTween").Play();		
			break;

		// 折る方向の選択の説明.
		case 6:
			tutorialMovie.SetActive(false);
			if(OrigamiControllerScript.GetState() == OrigamiUpdate.STATE.CUT)
				NextText();
//			iTweenEvent.GetEvent(gameObject,"IdleTextTween").Play();		
			break;

		// 戻る機能の説明.
		case 7:
			tutorialMovie.SetActive(false);
//			if(OrigamiControllerScript.GetState() == OrigamiUpdate.STATE.FOLD_SELECT)
//				NextText();
			iTweenEvent.GetEvent(gameObject,"IdleTextTween").Play();		
			break;
			
		// 拍手の説明.
		case 8:
			tutorialMovie.SetActive(false);
			iTweenEvent.GetEvent(gameObject,"IdleTextTween").Play();		
//			if(OrigamiControllerScript.GetActiveFlg() == false)
//				NextText();
			break;

		default:
			tutorialMovie.SetActive(true);
			iTweenEvent.GetEvent(gameObject,"IdleTextTween").Play();		
			break;
		}
	}
	
	void CloseText()
	{
		iTweenEvent.GetEvent(gameObject,"CloseTextTween").Play();		
	}
	
	void EndTutorial()
	{
		tutorialState.GetComponent<TutorialState>().EndText();
	}
	
	void PlayOrigami()
	{
//		OrigamiControllerScript.CreateOrigami(WAKU.LEVEL_1_1,OriTime);
	}
	
	void NextText()
	{
		currentNo++;
		if(currentNo >= text.Length)
			EndTutorial();
		else
			OpenText();
		
		if(currentNo == 5)
			OrigamiControllerScript.CreateOrigami(WAKU.LEVEL_1_1,1000);
	}
	
}
