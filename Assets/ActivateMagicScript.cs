using UnityEngine;
using System.Collections;

public class ActivateMagicScript : MonoBehaviour {

	public ParticleSystem lightening, meteorite, tornado, holyblast;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Q))
		{
			lightening.Play();
		}
		if(Input.GetKey(KeyCode.W))
		{
			meteorite.Play();
		}
		if(Input.GetKey(KeyCode.E))
		{
			tornado.Play();
		}
		if(Input.GetKey(KeyCode.R))
		{
			holyblast.Play();
		}
	}
}
