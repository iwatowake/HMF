using UnityEngine;
using System.Collections;

public class BreakEffect : MonoBehaviour {
	public Vector3 	target = new Vector3(0,0,0);
	public float 	convergence = 3;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.E))
		{
			ParticleSystem.Particle []ParticleList = new ParticleSystem.Particle[gameObject.particleSystem.particleCount];
			gameObject.particleSystem.emissionRate = 0;
			gameObject.particleSystem.GetParticles(ParticleList);
			for(int i = 0; i < ParticleList.Length; ++i)
			{
				ParticleList[i].lifetime /= convergence;
				ParticleList[i].velocity = (target - ParticleList[i].position) / ParticleList[i].lifetime;
			}
			gameObject.particleSystem.SetParticles(ParticleList,gameObject.particleSystem.particleCount);
		}
	}
}
