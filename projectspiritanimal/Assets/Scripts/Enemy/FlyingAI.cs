using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAI : AI {

    public Transform player;
    public Transform Bow;
    public Transform FirePoint;
    public GameObject arrows;
    public float fireArrowsDelay;
    public float rotateSpeed;
    public bool noDestination = true;
    public Rigidbody2D rb;

    private Vector2 direction;
    private Vector2 Enemy;
    private Vector2 Player;
    private RaycastHit2D hit;
    Vector2 newPos;
    float DestLength;
    Quaternion angle;
    private float FireNext;
    private Vector3 lastPos;
    private Vector2 areaOfMovement;
    
    AI.States Behaviours;

    // Use this for initialization
    protected override void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        Behaviours = new States();
        Behaviours = States.patrolling;
        //areaOfMovement = Random.insideUnitCircle * 2 + Enemy;
    }
	
	// Update is called once per frame
	protected override void Update () {

        

        Player = player.position ;
        Enemy = transform.position;

        direction = Player - Enemy;

        

       
        hit = Physics2D.Raycast(Enemy, direction);

        
        Debug.DrawLine(Enemy, Player, Color.red);

        //Ain bow at player
        Bow.rotation = Quaternion.Slerp(Bow.rotation, angle, Time.deltaTime * rotateSpeed);

        

        switch (Behaviours)
        {
            case States.patrolling:
                
                //Telling the enemy where to move to at a speed of 4
                transform.position = Vector3.MoveTowards(Enemy, newPos, Time.deltaTime);

                DestLength = Vector2.Distance(Enemy, newPos);

                //In the if statement the enemy will find one position to move to if it hasn't alread found one
                if (noDestination == true)
                {
                    //Setting boundaries for where the enemy can go within a sphere with a radius of 2
                    newPos = Random.insideUnitCircle * 5 + Enemy;

                    noDestination = false;
                }

                //find another position to move to once it's at the previous selected position
                if (Vector3.Distance(newPos, Enemy) < 0.1f)
                {
                    
                    noDestination = true;
                }

                /*if (transform.position.y < player.position.y)
                {
                    noDestination = true;
                }
                */

                //Resets the bow to original rotation
                angle = Quaternion.Euler(0, 0, 0);

                //switch between left and right sprite when play moves left or right
                if (transform.position.x > lastPos.x)
                {
                    lastPos.x = transform.position.x + 0.01f;
                    //transform.eulerAngles = new Vector3(0, 180, 0);
                }
                else if (transform.position.x < lastPos.x)
                {
                    lastPos.x = transform.position.x - 0.1f;
                    //transform.eulerAngles = new Vector3(0, 0, 0);
                }

                break;

            case States.attacking:

                

                //Calculates an angle for the enemy's bow to rotate to
                angle = Quaternion.Euler(0, 0, Mathf.Atan2(direction.normalized.y, direction.normalized.x) * Mathf.Rad2Deg);

                //Arrows will be fired depending on the fire rate
                if (Time.time > FireNext)
                {
                    FireNext = Time.time + fireArrowsDelay;

                    //The created object is stored into a object variable called arrow
                    GameObject arrow = Instantiate(arrows, FirePoint.position, Quaternion.identity);
                    
                    //If the arrow gameobject exists it will apply force in the direction of the firepoint object
                    if (arrow != null)
                    {
                        arrow.GetComponent<Rigidbody2D>().AddForce(FirePoint.right * 5, ForceMode2D.Impulse);
                    }
                }

                

                //Switch between left and right sprite depending on the angle of the bow
                if (Bow.eulerAngles.z > 270)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
                else if (Bow.eulerAngles.z < 270)
                {
                   transform.eulerAngles = new Vector3(0, 0, 0);
                }

                break;
        }


        //The transform of the object that was hit is stored in the transform variable object
        Transform Object = hit.transform;

        if (Object != null)
        {
            //When finding the player the enemy will attack else the enemy will continue to patrol
            if (Object.CompareTag("Player"))
            {
                Behaviours = States.attacking;

                //If the enemy is invisiable the enemy will patrol
                if (Object.GetComponent<Stealth>().isInvisible == true)
                {
                    Behaviours = States.patrolling;
                }
            }
            else
                Behaviours = States.patrolling;

            
        }

        

	}

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        noDestination = true;

        if (collision.transform.name == "Tilemap")
        {

        }
    }


}
