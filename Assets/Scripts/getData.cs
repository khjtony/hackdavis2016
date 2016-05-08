using UnityEngine;
using System.Collections;

public class getData : MonoBehaviour {
	// Use this for initialization
	void Start () {
		datasource pick = GetComponent<datasource> ();
		Debug.Log("position x: "+pick.position);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
