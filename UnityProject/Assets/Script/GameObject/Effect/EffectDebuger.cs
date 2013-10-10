using UnityEngine;
using System.Collections;

public class EffectDebuger : MonoBehaviour {
	bool 			openDebugWindow = true;
	int				effectMax		= 0;
	int 			selGridInt 		= 0;
	Vector2 		scrollVec2		= Vector2.zero;		
	
	public int 		windowNo 		= 10;
	public Rect 	windowSize 		= new Rect(0,270,300,270);
	public KeyCode	keyCode			= KeyCode.F1;
	
	// Use this for initialization
	void Start () {
		foreach (Transform child in transform) {
			effectMax++;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(keyCode))
			openDebugWindow = openDebugWindow ^ true;
	}

	void OnGUI()
	{
		if(openDebugWindow)
			windowSize.height = 50 + 20 * (effectMax + 4);
		else
			windowSize.height = 50;

		windowSize = GUI.Window(windowNo, windowSize, DebugWindow, "Effect");
	}
	
	void DebugWindow(int windowID)
	{
		GUI.DragWindow(new Rect(30, 0, windowSize.width, 50 + 20 * (effectMax + 2)));
		
		if(openDebugWindow)
		{
//			scrollVec2 = GUILayout.BeginScrollView(scrollVec2, GUILayout.Width(windowSize.height);
//			selGridInt = GUILayout.SelectionGrid(selGridInt, ), 1, "Toggle");
//			GUILayout.EndScrollView();
		}
		else
		{
			GUI.Label(new Rect(20,20,windowSize.width - 20,20),"Push" + keyCode.ToString() + "key To Open" );
		}
	}

}
