using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JumpScript : MonoBehaviour {

    public float jumpVelocity = 5f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    
    public bool inAir = false;

    private float gravity;
    
    private bool onLadder;
    private bool isClimbing;

    private bool onPlatform;
    private bool isJumping;


    [HideInInspector]
    public int jumpsInTotal;
    [HideInInspector]
    public bool isTripple = false;

    private bool hasPlayedLandingSound = false;
    private bool playAniOnce;

    Rigidbody2D rb;
    CircleCollider2D circle;
    PlayerMoverment1 player;
    

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
        player = GetComponent<PlayerMoverment1>();
    }
	
	// Update is called once per frame
	void Update () {
		

	}
    
}
