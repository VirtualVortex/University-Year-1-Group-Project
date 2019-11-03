using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAI : MonoBehaviour
{

    [Header("EmptyAudio Prefab from project")]
    public GameObject emptyAudio;

    [Header("")]
    public Transform player;
    [SerializeField]
    private PlayerMoverment1 playerCom;
    public Stealth stealth;

    public static bool noPowers;
    public static bool birdActive;
    public static bool rabbitActive;
    public static bool bearActive;
    public static bool isAttacking;

    public float bearAttackSpeed = 5f;
    public float maxBaerAttackDistance = 5;
    public float DistanceFromPlayer;
    public bool haveBird;
    public bool haveRabbit;
    public bool haveBear;

    private float delay;
    private float bearDelay;
    private bool bearReset = true;
    private float timer;
    float floating;
    float Distance;
    float chargeDistance;

    float currHealth;
    float prevHealth;

    PlayerMoverment1 healthTest;
    AnimalAnimations animalAnim;
    private void Start()
    {
        healthTest = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoverment1>();
        animalAnim = GetComponentInChildren<AnimalAnimations>();
    }

    // The Different states of the beasts
    public enum States
    {
        Still, Moving, Charging, BearAttack
    }

    public States behaviour;

    // Update is called once per frame
    void Update()
    {
        currHealth = healthTest.health;

        behaviour = new States();
        Debug.DrawLine(transform.position, player.position, Color.red);

        // Calculates the distance between the player and the beast
        Distance = Vector2.Distance(player.position, transform.position);

        // Delay for after the bear attacks to when it returns to normal possition.
        if (Time.time > bearDelay)
        {
            isAttacking = false;
        }

        // Resets the defend bool on the animal Animator to false.
        if (currHealth == prevHealth)
            animalAnim.animator.SetBool("Defend", false);


        // If the bear is selected and shift is held down
        if (bearActive && Input.GetKey(KeyCode.LeftShift) && bearReset)
        {
            // The distance between the bear and the player is calculated.
            chargeDistance = player.position.x - transform.position.x;

            if (Mathf.Abs(chargeDistance) < maxBaerAttackDistance)
            {
                // The bear charges up
                behaviour = States.Charging;
            }
            else
            {
                behaviour = States.BearAttack;
                bearReset = false;
            }
        }
        // When shift is released
        else if (bearActive && Input.GetKeyUp(KeyCode.LeftShift))
        {
            // Attacking is set to true because the release of Shift happens for one frame,
            // I need the new state to be constant.
            isAttacking = true;

            bearReset = true;
        }
        else if (bearActive && currHealth != prevHealth)
        {
            animalAnim.animator.SetBool("Defend", true);
            bearDelay = Time.time + 0.5f;
            isAttacking = true;
        }
        
        else if (isAttacking)
        {
            // The state is set to BearAttack
            behaviour = States.BearAttack;
        }
        else
        {
            // Normal delay for when the player starts to move.
            if (Time.time > delay)
            {
                behaviour = States.Moving;
            }

            // If the beast is close enough to the player...
            if (Distance < DistanceFromPlayer)
            {
                // Adds a slight delay for after the player moves again.
                delay = Time.deltaTime + 0.03f;

                // sets beast's state to stand still
                behaviour = States.Still;

                // The beast bobs up and down when idle.

                //floating = Mathf.Sin(timer += Time.deltaTime);
                //transform.position = new Vector3(transform.position.x, floating/3,transform.position.z);
            }
            else
            {
                // If non of the above, the beast moves towards the player.
                behaviour = States.Moving;
            }
        }




        switch (behaviour)
        {
            case States.Still:
                delay = Time.time + 0.1f;
                transform.position = Vector2.Lerp(transform.position, transform.position, Time.deltaTime);
                break;

            case States.Moving:
                transform.position = Vector2.Lerp(transform.position, player.position + new Vector3(0, 0.5f), Time.deltaTime * 2);
                floating = 0;
                break;

            case States.Charging:
                bearDelay = Time.time + 0.5f;
                transform.position = Vector2.Lerp(transform.position, transform.position, Time.deltaTime);
                break;

            case States.BearAttack:
                transform.position = Vector2.Lerp(transform.position, player.position + new Vector3(chargeDistance, 0), Time.deltaTime * bearAttackSpeed);
                break;
        }

        if (stealth != null)
        {
            if(!stealth.isInvisible)
            {
                //when the first animal has been freeded then the sprite render on the animal object will be enabled
                if (haveBear || haveBird || haveRabbit)
                {
                    //GetComponentInChildren<SpriteRenderer>().enabled = true;
                }

                if (Input.GetKey(KeyCode.Alpha1) && haveBear)
                {
                    bearActive = true;
                    birdActive = false;
                    rabbitActive = false;
                    if (playerCom != null)
                    {
                        playerCom.jumpsInTotal = 0;
                    }
                }


                if (Input.GetKey(KeyCode.Alpha2) && haveBird)
                {
                    bearActive = false;
                    birdActive = true;
                    rabbitActive = false;
                }

                if (Input.GetKey(KeyCode.Alpha3) && haveRabbit)
                {
                    bearActive = false;
                    birdActive = false;
                    rabbitActive = true;
                    if (playerCom != null)
                    {
                        playerCom.jumpsInTotal = 0;
                    }
                }

                if (Input.GetKey(KeyCode.Alpha4))
                {

                    GetComponentInChildren<Renderer>().material.color = Color.white;
                    bearActive = false;
                    birdActive = false;
                    rabbitActive = false;
                }
            }
        }
    }

    private void LateUpdate()
    {
        prevHealth = currHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttacking && collision.CompareTag("breakableObject"))
        {
            if (collision.GetComponent<BreakSound>() != null)
            {
                GameObject newAudioSource = Instantiate(emptyAudio, transform);

                AudioSource source = newAudioSource.GetComponent<AudioSource>();

                if (source != null && !source.isPlaying)
                {
                    source.clip = collision.GetComponent<BreakSound>().BreakSoundClip;
                    source.Play();
                    Destroy(newAudioSource, source.clip.length);
                    Destroy(collision.gameObject, 0.1f);
                }
                else
                {
                    Debug.LogError("Instantiated Audio game object does not have an Audio Source Componant!");
                }
            } else
            {
                Debug.Log("No Breaking Auidio clip was set");
                Destroy(collision.gameObject, 0.1f);
            }


        }
        else if (isAttacking && !collision.CompareTag("breakableObject"))
        {
            Debug.LogWarning("Object does not have 'breakableObject' tag.");
        }
    }
}