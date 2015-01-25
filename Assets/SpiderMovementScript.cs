using UnityEngine;
using System.Collections;

public class SpiderMovementScript : MonoBehaviour {

	float changeDirectionTicker = 0;
	int speed2 = 5;
	GameObject p1;
	float varTime =2;
//	Animator animator;
	
	Vector3 speed = Vector3.zero;
	
	// Use this for initialization
	void Start () {
		this.rigidbody.velocity = Vector3.zero;
		p1 = GameObject.FindGameObjectWithTag ("Player");
//		animator = GetComponent<Animator> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody.velocity = speed;
		changeDirectionTicker += Time.deltaTime;
//		animator.SetFloat ("Speed", rigidbody.velocity.magnitude);

		if(changeDirectionTicker >=varTime)
		{
			varTime = Random.Range(1f, 5f);
			changeDirectionTicker = 0;
			Vector3 q = p1.transform.position - this.rigidbody.position;
			speed = new Vector3( q.x/speed2 ,0,q.z/speed2);
		}


		RotateTowardVelocity ();
	}
	
	void RotateTowardVelocity()
	{
		transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
	}
}
