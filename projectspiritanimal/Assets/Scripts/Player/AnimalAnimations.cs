using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Animal
{
    Bear,
    Bird,
    Hear
}

public class AnimalAnimations : MonoBehaviour {

    public string level2name = "Level2.4";

    // The UI indicators.
    [Header("UI Indicators")]
    public Animator bearAnim;
    public Animator birdAnim;
    public Animator hearAnim;

    private AnimalAI ai;

    // public for AnimalAI for bear to defend.
    [HideInInspector]
    public Animator animator;


    private SpriteRenderer playerSprite;
    private SpriteRenderer spriteAnimal;

    private GameObject player;
    private GameObject wings;
    private GameObject dash;

    private PlayerMoverment1 movement;
    
    private bool set = false;
    private bool changing = false;

    // Use this for initialization
    void Start()
    {
        ai = GetComponentInParent<AnimalAI>();
        animator = GetComponent<Animator>();
        spriteAnimal = GetComponent<SpriteRenderer>();

        wings = GameObject.FindGameObjectWithTag("Wings");
        wings.SetActive(false);

        dash = GameObject.FindGameObjectWithTag("Dash");
        dash.SetActive(false);
        

        playerSprite = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoverment1>();

        bearAnim.SetBool("isCollect", false);
        birdAnim.SetBool("isCollect", false);
        hearAnim.SetBool("isCollect", false);

        if (SceneManager.GetActiveScene().name == level2name)
        {
            // Grants all animals when enter level 2.
            hearAnim.SetBool("isCollect", true);
            bearAnim.SetBool("isCollect", true);
            birdAnim.SetBool("isCollect", true);

            /*
            hearAnim.SetBool("isL2", true);
            bearAnim.SetBool("isL2", true);
            birdAnim.SetBool("isL2", true);
            */
        } else
        {
            if (ai.haveBear)
            {
                bearAnim.SetBool("isCollect", true);
            }
            if (ai.haveBird)
            {
                birdAnim.SetBool("isCollect", true);

            }
            if (ai.haveRabbit)
            {
                hearAnim.SetBool("isCollect", true);
            }
        }


    }


    // Update is called once per frame
    void Update()
    {
        // Flips Animal to face the same way as player
        float distance = playerSprite.transform.position.x - transform.position.x;

        if (animator.GetInteger("AnimState") == 0)
        {
            if (distance < 0)
                spriteAnimal.flipX = true;
            else
                spriteAnimal.flipX = false;
        }

        // Plays the summoning animation.
        if (Input.GetKeyDown(KeyCode.Alpha1) && !changing && ai.haveBear)
        {
            wings.SetActive(false);
            dash.SetActive(false);
            ChangeIndicator(true, false, false);
            ChangeAnimal(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && !changing && ai.haveBird)
        {
            wings.SetActive(true);
            dash.SetActive(false);
            ChangeIndicator(false, true, false);
            ChangeAnimal(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && !changing && ai.haveRabbit)
        {
            wings.SetActive(false);
            dash.SetActive(true);
            ChangeIndicator(false, false, true);
            ChangeAnimal(3);
        }


        // Calls animation functions for active animal.
        if (AnimalAI.bearActive)
        {
            BearAnimations();
        }
        else if (AnimalAI.rabbitActive)
        {
            HareAnimations();
        }
        else if (AnimalAI.birdActive)
        {
            BirdAnimations();
        }
        
    }

    void BearAnimations()
    {
        if (ai.behaviour == AnimalAI.States.Still)
        {
            animator.SetInteger("AnimState", 0);
        }
        if (ai.behaviour == AnimalAI.States.Moving)
            animator.SetInteger("AnimState", 1);



        if (ai.behaviour == AnimalAI.States.Charging)
        {
            animator.SetInteger("AnimState", 2);
        }
        if (ai.behaviour == AnimalAI.States.BearAttack)
        {
            animator.SetInteger("AnimState", 3);
        }
        if (ai.behaviour == AnimalAI.States.Still && Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetInteger("AnimState", 4);
        }
    }

    void HareAnimations()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetButton("Horizontal") && movement.canBoost)
        {
            animator.SetInteger("AnimState", 6);
        }
        else if (!movement.canBoost)
        {
            animator.SetInteger("AnimState", 5);
        }
    }

    void BirdAnimations()
    {
        if (movement.isTripple)
        {
            animator.SetInteger("AnimState", 8);
        }
        else
        {
            animator.SetInteger("AnimState", 7);
        }
    }
    
    public void ChangeAnimal(int animal)
    {
        if (animal == 0)
            changing = false;
        else
            changing = true;

        animator.SetInteger("GetAnimal", animal);
    }

    void ChangeIndicator(bool bear, bool bird, bool hear)
    {
        string act = "isActive";
        bearAnim.SetBool(act, bear);
        birdAnim.SetBool(act, bird);
        hearAnim.SetBool(act, hear);
    }

    public void OnFinishAnimation()
    {
        animator.SetInteger("AnimState", 0);
    }

    void InteruptCharge()
    {
        animator.SetInteger("AnimationState", 4);
    }


    public void PlaySound(int animalNumber)
    {
        player.GetComponentInChildren<AudioPlayer>().PlayAnimalSound(animalNumber);
    }
}
