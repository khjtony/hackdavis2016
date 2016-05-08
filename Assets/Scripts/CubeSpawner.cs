using UnityEngine;
using System.Collections;


public class CubeSpawner : MonoBehaviour {
	public GameObject cubeprefabVar;

	// Use this for initialization
	void Start () {
//		Instantiate (cubeprefabVar);
	}
	
	// Update is called once per frame
	void Update () {
		Instantiate (cubeprefabVar);
	}
}
