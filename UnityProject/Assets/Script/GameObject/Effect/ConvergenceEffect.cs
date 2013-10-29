using UnityEngine;
using System.Collections;

public class ConvergenceEffect : MonoBehaviour {
	public float rotateSpeed = 2.0f;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Rotate(0,rotateSpeed,0);
	}
}
