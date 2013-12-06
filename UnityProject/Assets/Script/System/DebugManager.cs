﻿using UnityEngine;
using System.Collections;

public class DebugManager : SingletonMonoBehaviour<DebugManager> {

	public	bool	mouseMode = false;
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.F1))
		{
			mouseMode = !mouseMode;
		}
	}
}
