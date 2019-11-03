using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum AI_STATE { STATE_IDLE = 0, STATE_ATTACK = 1, STATE_LOST_SIGHT = 2 } 

public class GroundAI : AI {


    AI ai1;
    

    [Header("Speed")]
    public float speed;

    [Header("Damage")]
    public float damage;

    [Header("Sight Range")]
    public float sightRange;

    [Header("Attack Range")]
    public float AttackRange;

    [Header("")]

    public Transform player;
    public Transform RaycastPoint;
    public Transform pointEnd;
    public Renderer materal;
    public bool foundPlayer;



    //AI_STATE state = AI_STATE.STATE_IDLE;

    public bool canTurn;
    public float nextScan;
    
    AI.States behaviours;
    private float constantYPos;
    private Vector3 lastPos;
    private PlayerMoverment1 healthbar;
    private float attackDelay;


    // Use this for initialization
    protected override void Start () {

        ai1 = gameObject.AddComponent<AI>();

        behaviours = new States();
        behaviours = States.patrolling;

        constantYPos = transform.position.y;
        ai1.t = transform;

    }
	
	// Update is called once per frame
	protected override void Update ()
    {

        /*switch (state)
        {
            case AI_STATE.STATE_IDLE:

                /// do idle state stuff
                /// // IdleState();
                break;

            case AI_STATE.STATE_ATTACK:

                //AttackState();
                break;

                // etc
        }
        */


        //Setting raycast1 so the enemy can detect the player
        /*ai1.DetectingPlayer(pointEnd.position,transform.right, player, sightRange, AttackRange, behaviours);
        Debug.DrawRay(transform.position, transform.right, Color.red);
        */

        //To detect player
        RaycastHit2D hit = Physics2D.Raycast(RaycastPoint.position, transform.right, sightRange);
        Debug.DrawLine(RaycastPoint.position, hit.point, Color.red);
        
        

        //Edge detection raycast
        RaycastHit2D groundDetect = Physics2D.Raycast(pointEnd.position, Vector2.down);
        Debug.DrawLine(pointEnd.position, groundDetect.point, Color.blue);

        // hit object for edge detection stored in transform variable 
        Transform Object = groundDetect.transform;
        // hit object for player detection stored in transform variable 
        Transform DetectedObject = hit.transform;

        transform.position = new Vector3(transform.position.x, constantYPos, 0);
        //behaviours = ai1.DetectingPlayer(RaycastPoint.position, transform.right, sightRange, AttackRange, foundPlayer);

        
        //Note: Maximum sight range may be at 5 since 10 may have case problems
        switch (behaviours)
        {
            //The AI will move to the right and have it material colour set to green
            case States.patrolling:
               
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                materal.GetComponent<Renderer>().material.color = Color.green;
                break;
            
            //When the enemy spots the player it will follow the player and change it's material colour to yellow
            //Also the enemy will rotate 180s depending on its position
            case States.moving:
                
                materal.GetComponent<Renderer>().material.color = Color.yellow;
                transform.position = Vector2.Lerp(transform.position, player.position, Time.deltaTime);

                //Use rotate method in AI class 
                if (transform.position.x < lastPos.x)
                {
                    lastPos.x = transform.position.x + 0.01f;
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
                else if (transform.position.x > lastPos.x)
                {
                    lastPos.x = transform.position.x - 0.01f;
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }

                break;

            //when the enemy is attacking its material colour will change to red
            //and it will access the health variable in the playermovement script
            case States.attacking:
                
                materal.GetComponent<Renderer>().material.color = Color.red;
                healthbar = hit.transform.GetComponent<PlayerMoverment1>();

                //The player's health will reduced every three seconds
                if (Time.time > attackDelay)
                {
                    attackDelay = Time.time + 3;
                    healthbar.health -= 100;
                }
                break;

            
        }

        //Turning left if the enemy is moving right
        if (transform.eulerAngles.y == 0 && canTurn == true)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            canTurn = false;
        }

        //Turning right if the enemy is moving left
        if (transform.eulerAngles.y == 180 && canTurn == true)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            canTurn = false;
        }

        //If the enemy loses sight of the player then it will go back to patrolling
        if (foundPlayer == false)
        {
            behaviours = States.patrolling;
        }


        //The if statement will see if the detected Object exists 
        if (DetectedObject != null)
        {

            //Debug.Log(DetectedObject.name);

            //The if statement below will see if the detected object has the Player tag
            if (DetectedObject.CompareTag("Player"))
            {
                //Debug.Log("Found Player");
                behaviours = States.moving;
                
                foundPlayer = true;

                if (Vector2.Distance(DetectedObject.position, transform.position) < AttackRange)
                {
                    behaviours = States.attacking;
                }

                //If the player is invisible then the enem's state will be patrolling
                if (DetectedObject.GetComponent<Stealth>().isInvisible == true)
                {
                    behaviours = States.patrolling;
                }
            }
            else
                foundPlayer = false;


            /*if (DetectedObject.name == "Tilemap")
            {
                canTurn = true;
            }
            */
            

        }
        

        

        if (groundDetect.collider != null)
        {
            if (groundDetect.distance > 2)
            {
                
                //The enemy will scan for the edge of the platform once every second
                if (Time.time > nextScan)
                {
                    nextScan = Time.time + 0.2f;
                    canTurn = true;
                }

                //If the enemy has found the player but is on or moving to another platofrm 
                //then the enemy will go back to patrolling
                if (foundPlayer == true)
                {
                    behaviours = States.patrolling;
                }

            }
        }

        
        


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "Tilemap")
        {
            canTurn = true;
        }

        
    }

    

}
