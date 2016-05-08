using UnityEngine;
using System.Collections;

public class positionDrive : MonoBehaviour {
    GameObject hand;
    calPosition myPosition;
    public float sensitive = 0;

	// Use this for initialization
	void Start () {
        hand = GameObject.Find("UnitySerialPort");
        myPosition = hand.GetComponent<calPosition>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = myPosition.getPosition();
        transform.position = new Vector3(pos.x/10.0f, pos.y,(pos.z-1000)*sensitive);
        print("X: " + pos.x+
           " Y: " + pos.y+
            " Z: " + pos.z);
	}
}
