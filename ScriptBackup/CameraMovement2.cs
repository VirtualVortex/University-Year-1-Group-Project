using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement2 : MonoBehaviour {

    public Transform player;
    public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector2.Lerp(transform.position, player.position,Time.deltaTime * speed);
        transform.position = new Vector3(transform.position.x,transform.position.y,-10);
	}
}
