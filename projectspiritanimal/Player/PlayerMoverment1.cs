using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerMoverment1 : MonoBehaviour {

    [Header("CHEAT SETTING")]
    public bool cheatEnabled = true;
    [HideInInspector]
    public bool noClip;


    [Header("Moverment")]
    public float movementSpeed = 5f;
    public float speedBoost;
    public float boostTimer;
    public float boostCoolDown;
    private float delay;


    [Header("Jump Variables")]
    public float jumpVelocity = 5f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public int jumpsInTotal;

    [Header("Health")]
    public float health;
    public Image healthBar;

    [Header("Animal")]
    public AnimalAI animal;

    [Header("Animator")]
    public Animator ani;

    [Header("Wings Animator")]
    public Animator wingsAnim;

    public Animator dashAnim;

    public bool freedBear, freedBird, freedHare;
    public float openDoor;
    public bool SwitchToScene2;

    private Vector3 healthNormal;
    private float gravity;
    [HideInInspector]
    public  bool inAir = false;
    private Stealth stealthUI;
    private Vector2 lastPos;
    private float normalSpeed;
    private bool onLadder;
    private bool isClimbing;
    public float level;
    SaveLoadSystem load = new SaveLoadSystem();
    private bool onPlatform;
    private bool isJumping;
    float horizontal;
    bool isDead = false;
    [HideInInspector]
    public bool canBoost;

    [HideInInspector]
    public bool isFalling = false;
    [HideInInspector]
    public bool isTripple = false;

    private bool hasPlayedLandingSound = false;
    private bool playAniOnce;
    private float rbVel;

    [Tooltip("To Indicator that sprint is on cooldown")]
    public SpriteRenderer Animal;

    Rigidbody2D rb;
    SpriteRenderer sprite;

    AudioPlayer PlayerAudio;

    CircleCollider2D circle;

    PauseMenu pause;

    GameObject dialopueBox;

    Animator sporeCollect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        circle = GetComponent<CircleCollider2D>();
        PlayerAudio = GetComponentInChildren<AudioPlayer>();
        pause = GameObject.Find("EventSystem").GetComponent<PauseMenu>();
        dialopueBox = GameObject.Find("DialogBoxSprite");
        sporeCollect = GameObject.Find("SporeAnimationCollect").GetComponent<Animator>();
    }

    void Start()
    {
        gravity = Physics2D.gravity.y;
        stealthUI = gameObject.GetComponent<Stealth>();
        jumpsInTotal = 0;
        normalSpeed = movementSpeed;

        //lastPos.y = transform.position.y;
        //InvokeRepeating("GetPos", 0.7f, 0.3f);
        //health *= 100;
        //load.LoadData();
        ani.SetBool("isDead", isDead);
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
        rbVel = RoundToFloat(rb.velocity.y, 1000);

        //currentPos.text = transform.position.y.ToString();
        //lastPosition.text = lastPos.y.ToString();
        //TotalofJumps.text = jumpsInTotal.ToString();
        level = SceneManager.GetActiveScene().buildIndex;
        
        healthBar.rectTransform.sizeDelta = new Vector2(health, 78.97f);
        horizontal = Input.GetAxisRaw("Horizontal");
        
        if (!isDead && !pause.GameIsPaused && !dialopueBox.activeInHierarchy)
        {
            #region Movement
            if (Input.GetButton("Horizontal"))
            {


                if (stealthUI.isInvisible == true)
                {
                    Debug.Log("Slowing down");
                    movementSpeed = 0.5f;
                    Debug.Log(movementSpeed);
                }

                rb.velocity += new Vector2(horizontal, 0) * movementSpeed;


            }

            if (Time.time < boostTimer)
            {
                canBoost = true;
                Animal.color = new Color(1, 1, 1, 1);
            }
            else
            {
                canBoost = false;
                Animal.color = new Color(1, 1, 1, 0.5f);
            }

            if (Time.time > delay)
            {
                boostTimer = Time.time + 0.1f;
            }

            //Apply force to boost player forwards
            if (AnimalAI.rabbitActive && Input.GetKey(KeyCode.LeftShift))
            {
                //movementSpeed = speedBoost;
                if (canBoost && Input.GetAxis("Horizontal") != 0)
                {
                    dashAnim.SetBool("dash", true);
                    rb.AddForce(new Vector2((Input.GetAxis("Horizontal") * speedBoost), 0), ForceMode2D.Impulse);
                    Debug.Log("Boost");
                    //canBoost = false;
                    delay = Time.time + boostCoolDown;
                }
                

            }
            else if ((AnimalAI.rabbitActive && !Input.GetKey(KeyCode.LeftShift)) || !AnimalAI.rabbitActive)
            {
                movementSpeed = normalSpeed;
                //boostTimer = Time.time + 0.1f;
                
            }

            if (AnimalAI.rabbitActive && Input.GetKeyUp(KeyCode.LeftShift))
            {
                dashAnim.SetBool("dash", false);
            }

            if (transform.position.x < lastPos.x)
            {
                lastPos.x = transform.position.x + 0.01f;
                //transform.eulerAngles = new Vector3(0, 180, 0);
                sprite.flipX = true;

            }
            else if (transform.position.x > lastPos.x)
            {
                lastPos.x = transform.position.x - 0.01f;
                //transform.eulerAngles = new Vector3(0, 0, 0);
                sprite.flipX = false;
            }

            if (Input.GetButtonDown("Horizontal"))
                ani.SetFloat("Speed", Mathf.Abs(horizontal));
            else if (Input.GetButtonUp("Horizontal"))
                ani.SetFloat("Speed", Mathf.Abs(horizontal));





            #endregion
            //stealthUI.canBeInvis = false;
        }
        

        if (cheatEnabled && noClip)
        {
            if (Input.GetButton("Vertical"))
            {
                rb.velocity += new Vector2(0, Input.GetAxisRaw("Vertical")) * movementSpeed;
            }
        } else
        {
            if (!isDead && !pause.GameIsPaused && !dialopueBox.activeInHierarchy)
            {
                #region Jump
                if (Input.GetKeyDown(KeyCode.W) && !AnimalAI.birdActive)
                {
                    if (!onLadder)
                    {
                        if (!inAir || onPlatform)
                        {
                            PlayerAudio.PlayJumpSound();
                            inAir = true;
                            isJumping = true;
                            ani.SetBool("isJumping", true);

                            rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.W) && AnimalAI.birdActive)
                {
                    if (jumpsInTotal <= 1)
                    {
                        isTripple = true;
                        wingsAnim.SetBool("Flap", true);

                        isJumping = true;
                        ani.SetBool("isJumping", isJumping);

                        rb.velocity += Vector2.up * jumpVelocity;
                        jumpsInTotal++;
                    }
                }
                else if (Input.GetKeyUp(KeyCode.W) && AnimalAI.birdActive)
                {
                    isTripple = false;
                    wingsAnim.SetBool("Flap", false);
                    isJumping = false;
                }
                else if (onLadder)
                {
                    transform.Translate(0, Input.GetAxis("Vertical") / 5, 0);
                    rb.gravityScale = 0;
                    isClimbing = true;
                    playAniOnce = true;
                }
                else if (!onLadder)
                {
                    rb.gravityScale = 1;
                    playAniOnce = true;
                    isClimbing = false;
                }

                if (Input.GetKeyUp(KeyCode.W) && !AnimalAI.birdActive)
                {
                    isJumping = false;
                }

                
                // Realistic falling
                if (RoundToFloat(transform.position.y, 100) != RoundToFloat(lastPos.y, 100) && !onLadder)
                {
                    // Calculation for falling
                    rb.velocity += Vector2.up * gravity * (fallMultiplier - 1) * Time.deltaTime;

                    isFalling = true;
                        
                }
                if (rbVel == 0)
                {
                    isFalling = false;

                    isJumping = false;
                    jumpsInTotal = 0;

                    PlayerAudio.PlayLanding();
                }
                
                if (playAniOnce)
                {
                    playAniOnce = false;
                    ani.SetBool("isClimbing", isClimbing);
                }

                //Debug.Log(rbVel);
                
                if (rbVel < 0 && !GetComponentInChildren<GroundCheck>().onPlatform)
                {
                    ani.SetBool("isFalling", true);
                    ani.SetBool("isJumping", false);
                }
                else
                    ani.SetBool("isFalling", false);

                #endregion
            }
        }

        stealthUI.gemAmountText.text = stealthUI.gemAmount.ToString();

        if (health <= 0)
        {
            if (!isDead)
                ani.SetBool("isDead", true);
            GetComponent<Rigidbody2D>().simulated = false;
            isDead = true;
        }
        
        if (animal != null)
        {
            

        }

        if (freedBird)
        {
            animal.haveBird = true;
        }
        //else animal.haveBird = false;

        if (freedBear)
        {
            animal.haveBear = true;
        }
        //else animal.haveBear = false;

        if (freedHare)
        {
            animal.haveRabbit = true;
        }
        //else animal.haveRabbit = false;

        /*if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            openDoor = 0;

        }
        */
    }

    public void BoolIsFalse(string boolean)
    {
        ani.SetBool(boolean, false);
    }

    public void PlayDeathSounds()
    {
        PlayerAudio.PlayDeadSound();
    }

    public void PlayDamageSound()
    {
        PlayerAudio.PlayHurt();
    }

    private void LateUpdate()
    {
        //lastPos.y = transform.position.y;
        GetPos();
        sporeCollect.SetBool("isCollect", false);
    }

    
    public void GetPos()
    {
        lastPos.y = transform.position.y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player collides with an object with gem tag the gemAmount variable in the stealth script will be increased
        //
        if (collision.CompareTag("Gem"))
        {
            PlayerAudio.PlayItemPickUp();
            sporeCollect.SetBool("isCollect", true);

            stealthUI.gemAmount++;
            stealthUI.chargeBar.rectTransform.localScale = new Vector3(1, 1, 1);
            Destroy(collision.gameObject);

            if (!stealthUI.isInvisible)
            {
                stealthUI.barScale = 1;
            }

        }

        if (collision.CompareTag("Danger"))
        {
            Debug.Log("Hit");

            health -= 100;
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Ladder"))
        {
            onLadder = true;

            ani.SetBool("isJumping", false);

            if (collision.GetComponent<AudioSource>())
            {
                PlayerAudio.PlayClimbLadder(collision.GetComponent<AudioSource>(), true);
            }
        }

        if (collision.CompareTag("Health"))
        {
            if (health < 300)
            {
                PlayerAudio.PlayHealthUp();

                health += 100;
                Destroy(collision.gameObject);
            }
        }

        if (collision.CompareTag("Door"))
        { 
            SwitchToScene2 = true;
            load.SaveData();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            onLadder = false;

            if (collision.GetComponent<AudioSource>())
            {
                PlayerAudio.PlayClimbLadder(collision.GetComponent<AudioSource>(), false);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Bird"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                freedBird = true;
            }
        }
        //else animal.haveBird = false;

       if (collision.CompareTag("Bear"))
        {
            
            if (Input.GetKey(KeyCode.E))
            {
                freedBear = true;
            }
        }
        

        if (collision.CompareTag("Rabbit"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                freedHare = true;
            }
        }
        //else animal.haveRabbit = false;

        /*if (collision.CompareTag("Ladder"))
        {
            onLadder = true;
            ani.SetBool("isClimbing", isClimbing);
        }
        */

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Platform"))
        {
            onPlatform = true;
            inAir = false;
            transform.parent = collision.transform;
        }


        if (collision.transform.CompareTag("Danger"))
        {
            health -= 100;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Platform"))
        {
            onPlatform = false;
            transform.parent = null;
        }
    }
    
    // Fade the spirit animal on death
    public void AnimalFade(float amount)
    {
        Animal.color = new Color(1, 1, 1, amount);
    }

    public void Death()
    {
        SceneManager.LoadScene("DeathScreen");
    }
}




