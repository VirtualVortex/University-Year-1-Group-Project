using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioScript : MonoBehaviour {

    TestAI ai;
    AudioSource audioSource;

    GameObject player;

    public float maxAudioDistance;

    [Header("")]
    public GameObject emptyAudio;

    [Header("Audio Clips")]
    public AudioClip alert;
    public AudioClip walking;
    public AudioClip readyAttack;
    public AudioClip attacking;
    public AudioClip stunned;
    public AudioClip stunnedLoopClip;

    private float distanceX;
    private float distanceY;
    private float fullDistance;

    private bool playedClip1 = false;
    private bool playedClip2 = false;

    GameObject stunnedLoop;
    AudioSource loopSource;

	// Use this for initialization
	void Start () {
        ai = GetComponent<TestAI>();
        audioSource = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player");

		if (walking != null && audioSource != null)
        {
            audioSource.clip = walking;
            audioSource.Play();
            audioSource.loop = true;
        }
	}
	
	// Update is called once per frame
	void Update () {

        distanceX =  player.transform.position.x - transform.position.x;
        distanceY =  player.transform.position.y - transform.position.x;
        
        fullDistance = Vector2.Distance(transform.position, player.transform.position);
        
        // Sets Volume to zero if player is too far away.
        if (fullDistance >= maxAudioDistance)
        {
            audioSource.volume = 0;
        } else
        {
            audioSource.volume = 1 - (fullDistance / maxAudioDistance);

            if (distanceX > 0)
            {
                audioSource.panStereo = Mathf.Abs(distanceX / maxAudioDistance) * -1;
            }
            else if (distanceX < 0)
            {
                audioSource.panStereo = Mathf.Abs(distanceX / maxAudioDistance);
            }
            else
            {
                audioSource.panStereo = 0;
            }
        }

        if (stunnedLoop != null)
        {
            if (fullDistance >= maxAudioDistance)
            {
                loopSource.volume = 0;
            }
            else
            {
                loopSource.volume = 1 - (fullDistance / maxAudioDistance);

                if (distanceX > 0)
                {
                    loopSource.panStereo = Mathf.Abs(distanceX / maxAudioDistance) * -1;
                }
                else if (distanceX < 0)
                {
                    loopSource.panStereo = Mathf.Abs(distanceX / maxAudioDistance);
                }
                else
                {
                    loopSource.panStereo = 0;
                }
            }
        }


        if (ai.stunned && stunned != null && !playedClip1)
        {
            playedClip1 = true;
            PlayNewSound(emptyAudio, transform, stunned);

            stunnedLoop = Instantiate(emptyAudio, transform);
            loopSource = stunnedLoop.GetComponent<AudioSource>();

            loopSource.clip = stunnedLoopClip;
            loopSource.loop = true;
            loopSource.Play();

            audioSource.Pause();
        }
        if (ai.foundPlayer && alert != null && !playedClip2)
        {
            playedClip2 = true;
            PlayNewSound(emptyAudio, transform, alert);
        }



        if (!ai.stunned)
        {
            playedClip1 = false;

            audioSource.UnPause();

            if (stunnedLoop != null && loopSource != null)
            {
                loopSource.Stop();
                Destroy(stunnedLoop);
            }
        }

        if (!ai.foundPlayer)
            playedClip2 = false;

	}

    public void PlayAboutToAttack()
    {
        PlayNewSound(emptyAudio, transform, readyAttack);
    }

    public void PlayAttack()
    {
        PlayNewSound(emptyAudio, transform, attacking);
    }

    public void PlayNewSound(GameObject emptyAudio, Transform position, AudioClip clip)
    {
        GameObject newAudioSource = Instantiate(emptyAudio, position);

        AudioSource source = newAudioSource.GetComponent<AudioSource>();

        if (source != null && !source.isPlaying)
        {
            source.clip = clip;
            source.Play();
            Destroy(newAudioSource, source.clip.length);
        } else
        {
            Debug.LogError("Instantiated Audio game object does not have an Audio Source Componant!");
        }
    }



}
