using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlippDashSprites : MonoBehaviour {
    
    public SpriteRenderer playerSprt;

    // Update is called once per frame
    void Update () {
		
        if (playerSprt.flipX && AnimalAI.rabbitActive)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (!playerSprt.flipX && AnimalAI.rabbitActive)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
	}
}
