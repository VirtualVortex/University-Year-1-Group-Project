using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAITest : AI {

    public Transform player;
    public Transform Bow;
    public Transform FirePoint;
    public GameObject arrows;
    public float fireArrowsDelay;
    public float rotateSpeed;
    public bool noDestination = true;
    public Rigidbody2D rb;
    [SerializeField]
    private Animator ani;

    private Vector2 direction;
    private Vector2 Enemy;
    private Vector2 Player;
    Vector2 newPos;
    float DestLength;
    Quaternion angle;
    private float FireNext;
    private Vector2 areaOfMovement;

    [HideInInspector]
    public bool isFiring = false;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();

        states = States.patrolling;
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	protected override void Update () {

        base.Update();

        Player = player.position ;
        Enemy = transform.position;

        direction = Player - Enemy;
        target = direction;
        range = Mathf.Infinity;

        
        Debug.DrawLine(Enemy, Player, Color.red);

        //Ain bow at player
        Bow.rotation = Quaternion.Slerp(Bow.rotation, angle, Time.deltaTime * rotateSpeed);

        
        switch (states)
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


                //Resets the bow to original rotation
                angle = Quaternion.Euler(0, 0, 0);

                //switch between left and right sprite when play moves left or right
                Rotate();

                break;

            case States.attacking:

                

                //Calculates an angle for the enemy's bow to rotate to
                angle = Quaternion.Euler(0, 0, Mathf.Atan2(direction.normalized.y, direction.normalized.x) * Mathf.Rad2Deg);

                //Arrows will be fired depending on the fire rate
                if (Time.time > FireNext)
                {
                    isFiring = true;

                    FireNext = Time.time + fireArrowsDelay;

                    //The created object is stored into a object variable called arrow
                    GameObject arrow = Instantiate(arrows, FirePoint.position, Quaternion.identity);
                    
                    //If the arrow gameobject exists it will apply force in the direction of the firepoint object
                    if (arrow != null)
                    {
                        arrow.GetComponent<Rigidbody2D>().AddForce(FirePoint.right * 5, ForceMode2D.Impulse);
                    }

                    Destroy(arrow, 5);
                } else
                {
                    isFiring = false;
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

        ani.SetBool("Attacking", isFiring);
	}

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        noDestination = true;

        if (collision.transform.name == "Tilemap")
        {

        }
    }


}
