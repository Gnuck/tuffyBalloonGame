using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonDropper : MonoBehaviour {

	public GameObject[] balloonPrefabs;
	public LevelManager levelManager;

	//public float fireDelay = 3f;
	float nextFire = 1f;
	int nextColor = 0;
	int nextPosition = 0;

	public float fireVelocity = 10f;

	void Start(){
		nextFire = (float)Random.Range (1, 4);
		levelManager = FindObjectOfType<LevelManager> ();
	}
	// Update is called once per frame
	void FixedUpdate () {
		nextFire -= Time.deltaTime;

		if(nextFire <= 0){

			//Randomly decide the color,number, and position for the balloon drop

			nextColor = (int)Random.Range (0, 8);
			nextPosition = (int)Random.Range (-4, 5);
			string balloonNumber = (Random.Range (1,levelManager.goalSum)).ToString();

			//Assign the position to spawn the balloon at
			Vector3 pos = transform.position;
			pos.x = pos.x + nextPosition;

			//Generate the balloon game object and add it to the level manager's list of active balloons.
			GameObject balloonGO = (GameObject)Instantiate(balloonPrefabs [nextColor],pos, Quaternion.identity);
			levelManager.activeBalloons.Add (balloonGO);
			TextMesh textMeshComponent=balloonGO.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
			textMeshComponent.text = balloonNumber;
			balloonGO.GetComponent<Rigidbody2D> ().velocity = transform.rotation * new Vector2 (0, fireVelocity);
			balloonGO.GetComponent<InitialVelocity>().balloonValue = int.Parse(balloonNumber);


			//Randomly decide the time delay for the next balloon drop
			nextFire = (float)Random.Range (1,3);
		}

	}
		
}
