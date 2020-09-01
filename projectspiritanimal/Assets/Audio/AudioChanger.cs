using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioChanger : MonoBehaviour {
    //The code for this audio changer came from Paul's snippets folder in the learning spaceand has been modified


    [SerializeField]
    private float fadeTime;
    [SerializeField]
    private float pauseTime;
    [SerializeField]
    private AudioClip songToSwitchTo;
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioMixerSnapshot silentSnapshot;
    [SerializeField]
    private AudioMixerSnapshot fullSnapshot;

    bool isTransitioning = false;

    public void Start()
    {
        //silentSnapshot.TransitionTo(fadeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!isTransitioning)
                StartCoroutine(AudioFade());
        }
    }

    IEnumerator AudioFade()
    {
        Debug.Log("Audio fade");
        isTransitioning = true;
        ///fade out current music.
        silentSnapshot.TransitionTo(fadeTime);
        yield return new WaitForSeconds(fadeTime + pauseTime);

        ///switch tracks
        source.clip = songToSwitchTo;
        source.Play();

        ///fade in new music
        fullSnapshot.TransitionTo(fadeTime);
        yield return new WaitForSeconds(fadeTime + pauseTime);
        isTransitioning = false;
        yield return null;
    }

    
}
