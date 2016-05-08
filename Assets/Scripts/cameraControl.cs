using UnityEngine;
using System.Collections;

public class cameraControl : MonoBehaviour {
    public float speed;
    public float rotationSpeed;
    private Rigidbody rb;

    void Start()
    {

    }

    void Update()
    {

        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);
    }
}