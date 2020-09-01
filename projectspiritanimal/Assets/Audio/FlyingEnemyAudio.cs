using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyAudio : MonoBehaviour {

    public AudioClip Flying;
    public AudioClip Shooting;

    public float maxAudioDistance = 10;

    AudioSource[] sources;
    AudioSource shoot;

    AudioSource Source;

    FlyingAITest flying;

    GameObject player;

    private float distanceX;
    private float distanceY;
    private float fullDistance;


    // Use this for initialization
    void Start () {
        Source = GetComponent<AudioSource>();
        
        sources = GetComponentsInChildren<AudioSource>();
        foreach (AudioSource type in sources)
        {
            if (type.gameObject.GetInstanceID() != GetInstanceID())
            {
                shoot = type.GetComponent<AudioSource>();
            }
        }

        flying = GetComponent<FlyingAITest>();

        player = GameObject.FindGameObjectWithTag("Player");

        if (Flying != null)
        {
            Source.clip = Flying;
            Source.Play();
        }

        if (Shooting != null)
            shoot.clip = Shooting;
	}
	
	// Update is called once per frame
	void Update () {

        distanceX = player.transform.position.x - transform.position.x;
        distanceY = player.transform.position.y - transform.position.x;

        fullDistance = Vector2.Distance(transform.position, player.transform.position);

        // Sets Volume to zero if player is too far away.
        if (fullDistance >= maxAudioDistance)
        {
            shoot.volume = 0;
            Source.volume = 0;
        }
        else
        {
            shoot.volume = 1 - (fullDistance / maxAudioDistance);
            Source.volume = 1 - (fullDistance / maxAudioDistance);

            if (distanceX > 0)
            {
                shoot.panStereo = Mathf.Abs(distanceX / maxAudioDistance) * -1;
                Source.panStereo = Mathf.Abs(distanceX / maxAudioDistance) * -1;
            }
            else if (distanceX < 0)
            {
                shoot.panStereo = Mathf.Abs(distanceX / maxAudioDistance);
                Source.panStereo = Mathf.Abs(distanceX / maxAudioDistance);
            }
            else
            {
                shoot.panStereo = 0;
                Source.panStereo = 0;
            }
        }



        if (flying.isFiring)
            shoot.Play();
	}
}
