using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AI : MonoBehaviour
{

    public float Speed { get; set; }
    public float Damage { get; set; }
    public Transform t;
    protected float range;
    protected float attackRange;
    protected bool playerFound;
    protected Vector2 target;
    
    Vector2 startPoint;
    
    
    
    
    protected Transform DetectedObject;
    protected RaycastHit2D hit;


    protected enum States { Still, patrolling, moving, attacking, stunned };
    protected States states;

    protected Vector3 lastPos;
   
    

    

    // Start is called before the first frame update
    protected virtual void Start()
    {
        t = transform;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //The raycast below will be used to detect the player
        hit = Physics2D.Raycast(t.position, target, range);
        DetectedObject = hit.transform;

        //The drawray method is used to viualise where the raycast is
        Debug.DrawRay(t.position, t.right, Color.red);

        if (DetectedObject != null)
        {
            //The if statement below will see if the detected object has the Player tag
            if (DetectedObject.CompareTag("Player"))
            {
                //If the object has the player tag then the enemy's state will switch to moving
                //and playerFound will be set to true
                states = States.attacking;
                playerFound = true;

                //If the player is invisible then the enemy will continue or switch to patrol
                if (DetectedObject.GetComponent<Stealth>().isInvisible == true)
                {
                    states = States.patrolling;
                }
            }
            else
               playerFound = false;
            
        }
    }
    
    //The Rotate method will rotate the enemy depending on it current position compared to its last position
    public void Rotate()
    {
        //switch between left and right sprite when play moves left or right
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
    }

    

}
