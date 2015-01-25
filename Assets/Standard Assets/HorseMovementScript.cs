using UnityEngine;
using System.Collections;

public class HorseMovementScript : MonoBehaviour {

	float changeDirectionTicker = 0;
	Animator animator;

	Vector3 speed = Vector3.zero;

	// Use this for initialization
	void Start () {
		this.rigidbody.velocity = Vector3.zero;
		animator = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {
		rigidbody.velocity = speed;
		changeDirectionTicker += Time.deltaTime;
		animator.SetFloat ("Speed", rigidbody.velocity.magnitude);
		if(changeDirectionTicker >=3)
		{
			changeDirectionTicker = 0;
			speed = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
		}

		RotateTowardVelocity ();
	}

	void RotateTowardVelocity()
	{
		transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
	}
}
