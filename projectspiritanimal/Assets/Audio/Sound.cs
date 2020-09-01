using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]//To make the function appear in the Editor this function has to be used
public class Sound {

    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]//To affect the sliders in-Game
    public float volume;
    [Range(.1f, 3f)]//The pitch range
    public float pitch;

    [HideInInspector]//Though this function is public, it won't show in the inspector
    public AudioSource source;
	
}

