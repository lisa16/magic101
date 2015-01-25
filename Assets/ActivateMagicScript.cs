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
			lightening.transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
			lightening.Play();
		}
		if(Input.GetKey(KeyCode.W))
		{
			meteorite.transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
			meteorite.Play();
		}
		if(Input.GetKey(KeyCode.E))
		{
			tornado.transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
			tornado.Play();
		}
		if(Input.GetKey(KeyCode.R))
		{
			holyblast.transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
			holyblast.Play();
		}
	}
}
