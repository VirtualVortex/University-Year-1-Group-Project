using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoverment : MonoBehaviour {

    [Header("Moverment")]
    public float movementSpeed = 5f;
    public float speedBoost;


    [Header("Jump Variables")]
    public float jumpVelocity = 5f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    private int jumpsInTotal;

    [Header("Health")]
    public float health;
    public Image healthBar;

    private Vector3 healthNormal;
    private float gravity;
    private bool inAir = false;
    private Stealth stealthUI;
    private Vector2 lastPos;
    private float normalSpeed;
    private AnimalAI animal;

    private Vector3 direction;

    Rigidbody2D rb;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health *= 100;
    }

    void Start()
    {
        gravity = Physics2D.gravity.y;
        inAir = false;
        stealthUI = gameObject.GetComponent<Stealth>();
        jumpsInTotal = 0;
        normalSpeed = movementSpeed;
    }

    private float RoundToFloat (float number, int multiplier)
    {
        number = number * multiplier;
        number = Mathf.Round(number);
        number = number / multiplier;
        return number;
    }

    // Update is called once per frame
    void Update () {

        

        healthBar.rectTransform.sizeDelta = new Vector2(health, 98.26f);

        #region Movement
        if (Input.GetButton("Horizontal"))
        {
            rb.velocity += new Vector2(Input.GetAxisRaw("Horizontal"), 0) * movementSpeed;
        }

        if (AnimalAI.rabbitActive && Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = speedBoost;
            Debug.Log(movementSpeed);
        }
        else if ((AnimalAI.rabbitActive && !Input.GetKey(KeyCode.LeftShift)) || !AnimalAI.rabbitActive)
        {
            movementSpeed = normalSpeed;
        }

        #endregion

        #region jump
        /*
        if (Input.GetKeyDown(KeyCode.W) && jumpsInTotal < 1 && !AnimalAI.doubleJump)
        {
            jumpsInTotal++;
            if(!inAir)
                rb.velocity += Vector2.up * jumpVelocity;
        }

        if (jumpsInTotal < 2 && AnimalAI.doubleJump)
        {
            if (GetKeyDown(KeyCode.W))
            {
                rb.velocity += Vector2.up * jumpVelocity;
                jumpsInTotal++;
  
            }
        }
        */

        Debug.Log(inAir);

        if (Input.GetKeyDown(KeyCode.W) && !AnimalAI.birdActive)
        {
            if (!inAir)
            {
                //rb.velocity += Vector2.up * jumpVelocity;
                rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.W) && jumpsInTotal < 2 && AnimalAI.birdActive)
        {
            rb.velocity += Vector2.up * jumpVelocity;
            jumpsInTotal++;
        }

        //Switch statement for powers
        /*switch (animal)
        {
            case:
                break
        }
        */
        


        //Debug.Log(rb.velocity);
        // Realistic falling
        if (RoundToFloat(transform.position.y, 10000) != RoundToFloat(lastPos.y, 10000))
        {
            // Calculation for falling
            rb.velocity += Vector2.up * gravity * (fallMultiplier - 1) * Time.deltaTime;

            inAir = true;
        }/*
        else if (RoundToFloat(transform.position.y, 10000) > RoundToFloat(lastPos.y, 10000) && !Input.GetButton("Jump"))
        {
            // Calculation for low jumping
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            inAir = true;
            
        }*/

        if (RoundToFloat(transform.position.y, 10000) == RoundToFloat(lastPos.y, 10000))
        {
            inAir = false;
            jumpsInTotal = 0;
        }
        #endregion
        




        stealthUI.gemAmountText.text = stealthUI.gemAmount.ToString();
    }

    private void LateUpdate()
    {
        lastPos.y = transform.position.y;
    }

    



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player collides with an object with gem tag the gemAmount variable in the stealth script will be increased
        //
        if (collision.CompareTag("Gem"))
        {
            stealthUI.gemAmount++;
            stealthUI.chargeBar.rectTransform.localScale = new Vector3(1, 1, 1);
            Destroy(collision.gameObject);

            if (!stealthUI.isInvisible)
            {
                stealthUI.barScale = 1;
            }

        }

        if (collision.CompareTag("Bird"))
        {
            animal.haveBird = true;
        }
        else animal.haveBird = false;

        if (collision.CompareTag("Bear"))
        {
            animal.haveBear = true;
        }
        else animal.haveBear = false;

        if (collision.CompareTag("Rabbit"))
        {
            animal.haveRabbit = true;
        }
        else animal.haveRabbit = false;

        if (collision.CompareTag("Danger"))
        {
            Debug.Log("Hit");

            direction = collision.transform.position - transform.position;
            direction.Normalize();

            health -= 100;
            Destroy(collision.gameObject);
            rb.AddForce(direction);
        }
    }

   
}
