using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioSourceSystem : MonoBehaviour {

    [SerializeField]
    private AudioSource audio;
    [SerializeField]
    private int music1;
    [SerializeField]
    private int music2;

    public static int songIndex;
    private bool canSwitch = false;
    private float x;
    private Vector2 direction;
    [SerializeField]
    private AudioManager audioMan;
    

    // Use this for initialization
    void Start () {
        
        //The last song that wa splaying is player at the start of the game
        songIndex = PlayerPrefs.GetInt("songIndex", songIndex);
        audio.clip = audioMan.sounds[songIndex].clip;
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //index of the music array saved whe nthe player collides with the trigger
        PlayerPrefs.SetInt("songIndex", songIndex);

        //The code below is used to get the vector between the player and the collider on the X-Axis
        direction = transform.position - collision.transform.position;
        direction.Normalize();
        x = direction.x;

        //if the player hits the collider on the right then the first song will be played
        if (x <= 0)
        {
            songIndex = music1;
            audio.clip = audioMan.sounds[music1].clip;
            audio.Play();
        }

        //if the player hits the collider on the left then the second song will be played
        if (x >= 0)
        {
            songIndex = music2;
            audio.clip = audioMan.sounds[music2].clip;
            audio.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        

    }
    
}
