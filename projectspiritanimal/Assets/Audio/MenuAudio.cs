using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour {

    public AudioClip defaultMenuClickSound;

    public AudioClip menuPopUpSound;

    private AudioSource source;

    private void Start()
    {
        if (!gameObject.GetComponent<AudioSource>())
        {
            gameObject.AddComponent<AudioSource>();
        }

        source = GetComponent<AudioSource>();

        source.playOnAwake = false;
        source.loop = false;

        source.clip = defaultMenuClickSound;
    }

    public void PlayClickSoundDefault()
    {
        source.clip = defaultMenuClickSound;
        source.Play();
    }

    public void PlayClickSound(AudioClip buttonClickSound)
    {
        source.clip = buttonClickSound;
        source.Play();
    }

    public void PlayMenuPopUpSound()
    {
        if (menuPopUpSound != null)
        {
            source.clip = menuPopUpSound;
            source.Play();
        }
    }
}
