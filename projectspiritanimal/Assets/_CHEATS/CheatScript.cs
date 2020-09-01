using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatScript : MonoBehaviour {

    public Canvas canvas;

    PlayerMoverment1 move;
    BoxCollider2D box;
    Rigidbody2D rb;

    public Animator bearAnim;
    public Animator birdAnim;
    public Animator hearAnim;

    bool animals = false;
    bool lives = false;
    bool noclip = false;

    bool toggle = false;

    int animal = 0;
    int live = 0;
    int clip = 0;


	// Use this for initialization
	void Start () {
        move = GetComponent<PlayerMoverment1>();
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        bearAnim = GameObject.Find("BearImage").GetComponent<Animator>();
        birdAnim = GameObject.Find("BirdImage").GetComponent<Animator>();
        hearAnim = GameObject.Find("HearImage").GetComponent<Animator>();

        if (canvas != null)
            canvas.enabled = false;
        else
            Debug.LogWarning("_CHEAT canvas not set in Cheat Script");

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.F1) && !toggle)
        {
            canvas.enabled = true;

            toggle = true;

        } else if (Input.GetKeyDown(KeyCode.F1) && toggle)
        {
            canvas.enabled = false;

            toggle = false;
        }
        

        if (lives)
        {
            move.health = 300;
        }
	}

    public void AllAnimals ()
    {
        animal++;

        if (animal == 1)
        {
            move.freedBear = true;
            move.freedBird = true;
            move.freedHare = true;
            
            bearAnim.SetBool("isCollect", true);
            birdAnim.SetBool("isCollect", true);
            hearAnim.SetBool("isCollect", true);


        } else
        {
            animal = 0;

            move.freedBear = false;
            move.freedBird = false;
            move.freedHare = false;
        }


    }

    public void Lives ()
    {
        live++;

        if (live == 1)
        {
            lives = true;
        } else
        {
            live = 0;
            lives = false;
        }
    }

    public void NoClip ()
    {
        clip++;

        if (clip == 1)
        {
            box.enabled = false;
            move.noClip = true;
            rb.gravityScale = 0;
        } else
        {
            clip = 0;

            box.enabled = true;
            move.noClip = false;
            rb.gravityScale = 1;
        }


    }
}
