using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {
    
    PlayerMoverment1 player;

    BoxCollider2D circle;

    [HideInInspector]
    public bool onPlatform;

	void Start () {
        player = GetComponentInParent<PlayerMoverment1>();
        circle = GetComponent<BoxCollider2D>();
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Platform"))
        {
            Debug.Log("To infinity and BEYOND!");
            player.ani.SetBool("Land", false);
            player.ani.SetBool("LandConst", false);
            onPlatform = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Platform"))
        {
            Debug.Log("Touch Down!");
            player.inAir = false;
            player.ani.SetBool("Land", true);
            player.ani.SetBool("LandConst", true);
            player.ani.SetBool("isJumping", false);
            player.ani.SetBool("isFalling", false);
            onPlatform = true;
        }
    }
}
