using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {
    

    [Header("Movement Sounds")]
    public AudioSource FootAudio;

    [Header("Jumping and landing Sorces")]
    public AudioSource JumpAudio;
    public AudioSource JumpSource;
    public AudioSource FallSource;
    public AudioSource LandSource;

    [Header("Jumping and Landing Clips")]
    public AudioClip JumpClip;
    public AudioClip FallingClip;
    public AudioClip LandingClip;
    
    [Header ("Audio Sources on Animal")]
    public AudioSource Summoning;
    public AudioSource animalAudio;

    [Header("Summon Animal Sounds (can be same)")]
    public AudioClip summonBear;
    public AudioClip summonBird;
    public AudioClip summonHear;

    
    [Header("Animal Abilities Sound clips")]
    public AudioClip bearAttack;
    public AudioClip birdFlap;
    public AudioClip rabbitDash;

    [Header("Stealth")]
    public AudioClip StealthClip;
    public AudioClip failedStealth;

    [Header("Collectables")]
    public AudioClip PickUpClip;
    public AudioClip HealthPickUpClip;

    [Header("Hurt and Death")]
    public AudioClip HurtClip;
    public AudioClip dyingClip;

    [Header("")]
    public AudioClip ladderClip;


    AnimalAI animal;

    PlayerMoverment1 movement;

    AudioSource publicAudio;

    float fadeTimer = 0.1f;

    float startVolume;

    bool hasPlayedLanding = false;

    private void Awake()
    {
        FootAudio.playOnAwake = false;
        JumpAudio.playOnAwake = false;
    }

    void Start () {
        animal = GameObject.FindGameObjectWithTag("Animal").GetComponent<AnimalAI>();
        animalAudio = GameObject.FindGameObjectWithTag("Animal").GetComponentInChildren<AudioSource>();

        startVolume = FootAudio.volume;

        publicAudio = GetComponent<AudioSource>();
        movement = GetComponentInParent<PlayerMoverment1>();
	}

    // Update is called once per frame
    void Update()
    {
        PlayFootstep();

        PlayFalling();

        PlaySummon();
    }
    
    public void PlayAnimalSound(int animal)
    {
        if (animal == 1)
        {
            animalAudio.clip = bearAttack;
            animalAudio.Play();
        }
        else if (animal == 2)
        {
            animalAudio.clip = birdFlap;
            animalAudio.Play();
        }
        else if (animal == 3)
        {
            animalAudio.clip = rabbitDash;
            animalAudio.Play();
        }
    }

    public void PlayDeadSound()
    {
        publicAudio.clip = dyingClip;
        publicAudio.Play();
    }
    
    public void PlayHurt()
    {
        publicAudio.clip = HurtClip;
        publicAudio.Play();
    }

    void PlaySummon()
    {
        if (Summoning != null)
        {
            if (Input.GetKey(KeyCode.Alpha1) && animal.haveBear)
            {
                if (summonBear != null)
                    Summoning.clip = summonBear;
                Summoning.Play();
            }
            else if (Input.GetKey(KeyCode.Alpha2) && animal.haveBird)
            {
                if (summonBird != null)
                    Summoning.clip = summonBird;
                Summoning.Play();
            }
            else if (Input.GetKey(KeyCode.Alpha3) && animal.haveRabbit)
            {
                if (summonHear != null)
                    Summoning.clip = summonHear;
                Summoning.Play();
            }
        }
    }

    void PlayFootstep ()
    {
        if (FootAudio != null && FootAudio.clip != null)
        {
            if (!movement.isFalling)
            {
                if (Input.GetButton("Horizontal"))
                {
                    FootAudio.volume = startVolume;
                    if (!FootAudio.isPlaying)
                        FootAudio.Play();
                }
            }
            if (!Input.GetButton("Horizontal") || movement.isFalling)
            {
                if (FootAudio.volume > 0 )
                {
                    FootAudio.volume -= startVolume * Time.deltaTime / fadeTimer;
                }
                if (FootAudio.volume <= 0)
                {
                    FootAudio.Stop();
                }
            }
        }
    }

    void PlayFalling()
    {
        if (JumpAudio != null && JumpAudio.clip != null)
        {
            if (movement.isFalling && !FallSource.isPlaying && FallingClip != null)
            {
                FallSource.clip = FallingClip;
                FallSource.Play();

                hasPlayedLanding = false;
            }
            if (!movement.isFalling)
            {
                JumpAudio.Stop();
                JumpAudio.time = 0;
            }
        }
    }

    public void PlayJumpSound()
    {
        JumpSource.clip = JumpClip;
        JumpSource.Play();
    }

    public void PlayLanding()
    {
        if (!hasPlayedLanding)
        {
            LandSource.clip = LandingClip;
            LandSource.Play();
            hasPlayedLanding = true;
        }
    }

    public void PlayStealthSound()
    {
        publicAudio.clip = StealthClip;
        publicAudio.Play();
    }

    public void PlayFailedStealth()
    {
        publicAudio.clip = failedStealth;
        publicAudio.Play();
    }

    public void PlayItemPickUp()
    {
        publicAudio.clip = PickUpClip;
        publicAudio.Play();
    }

    public void PlayHealthUp()
    {
        publicAudio.clip = HealthPickUpClip;
        publicAudio.Play();
    }

    public void PlayClimbLadder(AudioSource ladderSource, bool isOnLadder)
    {
        ladderSource.clip = ladderClip;
        ladderSource.loop = true;

        if (!ladderSource.isPlaying && isOnLadder)
            ladderSource.Play();
        if (!isOnLadder)
            ladderSource.Stop();
    }
}
