using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InitialVelocity : MonoBehaviour {

	public Vector3 initVel;
	public float directionChangeInterval = 1f;
	private float changeHorizontal = 1f;
	private float balloonRadius = 0.5f;
	public int balloonValue;
	LevelManager levelManager;

	// Use this for initialization
	public void Start () {
		GetComponent<Rigidbody2D>().velocity = initVel;
		levelManager = FindObjectOfType<LevelManager> ();
	}

	public void FixedUpdate(){

		//Make the balloon appear to float randomly left to right ---- balloon bobs in the air
		changeHorizontal -= Time.deltaTime;
		Vector3 vel = GetComponent<Rigidbody2D> ().velocity;
		if(changeHorizontal<0){
			int coin = Random.Range (1, 5);
			if(coin==4){
				GetComponent<Rigidbody2D>().velocity = new Vector3 (-vel.x, vel.y, vel.z);
			}
			changeHorizontal = directionChangeInterval;
		}
		Vector3 pos = transform.position;
		float screenRatio = (float) Screen.width / (float)Screen.height;
		float widthOrtho = Camera.main.orthographicSize * screenRatio;

		//Keep balloons from going off the left or right side of the screen
		if(pos.x + balloonRadius > widthOrtho){
			if(vel.x>0){
				GetComponent<Rigidbody2D>().velocity = new Vector3 (-vel.x, vel.y, vel.z);
			}
		}
		if(pos.x - balloonRadius < -widthOrtho){
			if(vel.x<0){
				GetComponent<Rigidbody2D>().velocity = new Vector3 (-vel.x, vel.y, vel.z);
			}
		}

		//Delete the object if it goes off the bottom of the screen
		if(pos.y +balloonRadius < -Camera.main.orthographicSize){
			levelManager.activeBalloons.Remove (gameObject);
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		float force = 20;
		Vector2 dir = coll.contacts[0].point - (new Vector2(transform.position.x,transform.position.y));
		dir = -dir.normalized;
		Vector3 dir3 = new Vector3(dir.x,dir.y,0);
		GetComponent<Rigidbody2D> ().AddForce (dir3 * force);

	}

}
