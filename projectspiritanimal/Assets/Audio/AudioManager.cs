using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;
    [SerializeField]
    Sound s;


    // Use this for initialization
    void Awake() { //Same as void start, but is just called before
        /*foreach (Sound s in sounds)//s is the vairable for the sound system
        {
          s.source = gameObject.AddComponent<AudioSource>();
          s.source.clip = s.clip;

          s.source.volume = s.volume;
          s.source.pitch = s.pitch;
        }
        */
    }

    public void Play (string name)
    {
        s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    
    

}
