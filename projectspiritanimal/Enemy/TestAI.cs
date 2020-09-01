using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestAI : AI {

    [Header("Speed")]
    public float speed;

    [Header("Damage")]
    public float damage;

    [Header("Sight Range")]
    public float sightRange;

    [Header("Attack Range")]
    public float AttackRange;
    [SerializeField]
    private float attackRate;

    [Header("Animation")]
    [SerializeField]
    private Animator animation;

    Transform player;

    [Header("")]
    public Transform RaycastPoint;
    public Transform pointEnd;
    public Renderer materal;
    public bool foundPlayer;

    public bool canTurn;
    public float nextScan;
    public float stunTime;

    private float constantYPos;
    private PlayerMoverment1 healthbar;
    private float attackDelay;
    private float delay;
    private bool isAttacking;

    [HideInInspector]
    public bool stunned;


    // Use this for initialization
    //The start method below will run the code in start method in the AI class
    //as well as that the start method will be overriden so that the code below can run as well;
    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        constantYPos = transform.position.y;
        range = sightRange;
        attackRange = AttackRange;
        delay = stunTime;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        target = transform.right;

        //The Rayast will be pointing down in front of the enemy allowing it to detect the edge of a platform
        RaycastHit2D groundDetect = Physics2D.Raycast(pointEnd.position, Vector2.down);
        //The line below ise used to visualise where the raycast is fired from
        Debug.DrawLine(pointEnd.position, groundDetect.point, Color.blue);

        // hit object for edge detection stored in transform variable 
        Transform Object = groundDetect.transform;

        //The y axis of the enemy will always equal its y axis at the start
        //This is to prevenet the enemy moving upwards when following the player
        transform.position = new Vector3(transform.position.x, constantYPos, 0);

        if (stunned)
        {
            states = States.stunned;
        }

        //Note: Maximum sight range may be at 5 since 10 may have case problems
        //The Ai's states are inherated by the AI class
        switch (states)
        {
            //The AI will move to the right and have it material colour set to green
            case States.patrolling:

                isAttacking = false;
                foundPlayer = false;
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                //materal.GetComponent<Renderer>().material.color = Color.green;
                break;

            //When the enemy spots the player it will follow the player and change it's material colour to yellow
            //Also the enemy will rotate 180s depending on its position
            case States.attacking:

                foundPlayer = true;
                //materal.GetComponent<Renderer>().material.color = Color.yellow;
                transform.position = Vector2.Lerp(transform.position, player.position, Time.deltaTime);

                //The rotate method from the AI class will run so that the enemy can turn towards the player when moving
                Rotate();

                //when the enemy is attacking its material colour will change to red
                //and it will access the health variable in the playermovement script
                if (DetectedObject != null)
                {
                    if (Vector2.Distance(DetectedObject.position, t.position) < AttackRange)
                    {

                        //materal.GetComponent<Renderer>().material.color = Color.red;

                        //The Raycast in the AI class accesses the playermovement script
                        healthbar = hit.transform.GetComponent<PlayerMoverment1>();

                        isAttacking = true;
                        //The player's health will reduced every three seconds
                        if (Time.time > attackDelay && hitting)
                        {
                            hitting = false;
                            attackDelay = Time.time + attackRate;
                            healthbar.health -= 100;
                            healthbar.PlayDamageSound();
                        }
                        else if (Time.time <= attackDelay)
                            isAttacking = false;
                    }
                    else
                    {
                        hitting = false;
                        isAttacking = false;
                    }
                }
                break;

            //When the enemy is stuned it will stay still for five seconds
            case States.stunned:

                isAttacking = false;
                //materal.GetComponent<Renderer>().material.color = Color.blue;
                transform.position = Vector2.Lerp(transform.position, transform.position, Time.deltaTime);
                delay -= Time.deltaTime;

                if (delay <= 0)
                {
                    states = States.patrolling;
                    stunned = false;
                }
                break;
        }



        RotateOnEdge();

        //If the enemy loses sight of the player then it will go back to patrolling
        if (playerFound == false)
        {
            states = States.patrolling;
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
                if (playerFound == true)
                {
                    states = States.patrolling;

                }

            }
        }

        if (animation != null)
        {
            animation.SetBool("attacking", isAttacking);
            animation.SetBool("isStunned", stunned);
        }
    }

    bool hitting = false;

    public void hitPlayer()
    {
        hitting = true;
    }

    public void RotateOnEdge()
    {
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "Tilemap")
        {
            canTurn = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<AnimalAI>() != null)
        {
            if (collision.transform.GetComponent<AnimalAI>().behaviour == AnimalAI.States.BearAttack)
            {
                Debug.Log("Trigger void: Stunned");
                stunned = true;
                delay = stunTime;
            }
        }
        
    }

}
