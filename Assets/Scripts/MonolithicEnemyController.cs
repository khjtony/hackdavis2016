using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monolithic : MonoBehaviour {
	public List<GameObject>	enemies;
	public float 	speed = 10f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tempPos;

		foreach (GameObject enemy in enemies) {
			tempPos = enemy.transform.position;
			switch (enemy.name) {
			case "EnemyGO":
				tempPos.y -= speed * Time.deltaTime;
				break;
			case "EnemyZigGO":
				tempPos.x = 4 * Mathf.Sin (Time.time * Mathf.PI * 2);
				tempPos.y -= speed * Time.deltaTime;
				break;
			}
			enemy.transform.position = tempPos;
		}
	}
}
