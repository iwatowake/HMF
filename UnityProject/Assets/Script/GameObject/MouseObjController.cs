using UnityEngine;
using System.Collections;

public class MouseObjController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3	Pos = Input.mousePosition;
		Pos.z = 6.5f;
		Pos = Camera.main.ScreenToWorldPoint( Pos );
		gameObject.transform.localPosition = Pos;
	}
}
