using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectChildScript : MonoBehaviour {

    public float fallSpeed = 0.5f;

    public FallingObjectParentScript fall;

    public Sprite[] sprites;
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y <= fall.endY)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, 1)];
            transform.position = new Vector2(Random.Range(fall.xRandPos1, fall.xRandPos2), fall.startY);
        } else
        {
            transform.position -= new Vector3(0, fallSpeed * Time.deltaTime);
        }
    }
}
