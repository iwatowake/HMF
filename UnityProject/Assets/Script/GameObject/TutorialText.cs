using UnityEngine;
using System.Collections;

public class TutorialText : MonoBehaviour {
	public string[] text;
	public GameObject tutorialState;
	
	int currentNo = 0;
	
	// Use this for initialization
	void Start () {
		gameObject.transform.localScale = Vector3.zero;
		OpenText();
	}
	
	// Update is called once per frame
	void Update () {
		if(currentNo < text.Length)
		{
			TextMesh tm = (TextMesh)gameObject.GetComponent("TextMesh");
			tm.text = text[currentNo];
		}
	}

	void OpenText()
	{
		iTweenEvent.GetEvent(gameObject,"OpenTextTween").Play();
	}
	
	void IdleText()
	{
		iTweenEvent.GetEvent(gameObject,"IdleTextTween").Play();				
	}
	
	void CloseText()
	{
		iTweenEvent.GetEvent(gameObject,"CloseTextTween").Play();		
	}
	
	void EndTutorial()
	{
		tutorialState.GetComponent<TutorialState>().EndText();
	}

	void NextText()
	{
		currentNo++;
		if(currentNo >= text.Length)
			EndTutorial();
		else
			OpenText();
	}
	
}
