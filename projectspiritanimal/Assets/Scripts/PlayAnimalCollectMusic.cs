using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAnimalCollectMusic : MonoBehaviour {
    public Animal animal;

    [Header("Indicators")]
    public Animator bearAnim;
    public Animator birdAnim;
    public Animator hearAnim;

    [Header("Shine Effect")]
    public Animator bearShineAnim;
    public Animator birdShineAnim;
    public Animator rabbitShineAnim;

    [Header("")]
    public Animator fadeScreen;

    [Header("Collect Music")]
    public AudioClip bearClip;
    public AudioClip birdClip;
    public AudioClip hearClip;


    private bool bearCollected = false;
    private bool birdCollected = false;
    private bool hearCollected = false;

    AnimalAI ai;
    AudioSource Audio;
    DialogSystem Dialog;
    AnimalAnimations animalAnimations;

	// Use this for initialization
	void Start () {
        ai = GameObject.Find("Animal_").GetComponent<AnimalAI>();
        Audio = GetComponent<AudioSource>();
        Dialog = GetComponent<DialogSystem>();
        animalAnimations = GameObject.FindGameObjectWithTag("Animal").GetComponentInChildren<AnimalAnimations>();

        Audio.loop = false;
        Audio.Stop();
        
    }
	
    // Reciever a Broadcast from DialogSystem
	public void AnimalCollectMusic ()
    {
        OnAnimalCollect(animal);
    }



    public void OnAnimalCollect(Animal animal)
    {
        if (animal == Animal.Bear)
        {
            //bearCollected = true;
            animalAnimations.ChangeAnimal(1);
            AnimalAI.bearActive = true;
            AnimalAI.birdActive = false;
            AnimalAI.rabbitActive = false;

            Audio.clip = bearClip;
            Audio.Play();
            bearAnim.SetBool("isCollect", true);
            bearShineAnim.SetBool("isCollect", true);

            fadeScreen.SetBool("Fade", true);

        }
        if (animal == Animal.Bird && !birdCollected)
        {
            //birdCollected = true;
            animalAnimations.ChangeAnimal(2);
            AnimalAI.bearActive = false;
            AnimalAI.birdActive = true;
            AnimalAI.rabbitActive = false;

            Audio.clip = birdClip;
            Audio.Play();
            birdAnim.SetBool("isCollect", true);
            birdShineAnim.SetBool("isCollect", true);

            fadeScreen.SetBool("Fade", true);
        }
        if (animal == Animal.Hear && !hearCollected)
        {
            //birdCollected = true;
            animalAnimations.ChangeAnimal(3);
            AnimalAI.bearActive = false;
            AnimalAI.birdActive = false;
            AnimalAI.rabbitActive = true;

            Audio.clip = hearClip;
            Audio.Play();
            hearAnim.SetBool("isCollect", true);
            rabbitShineAnim.SetBool("isCollect", true);

            fadeScreen.SetBool("Fade", true);
        }
    }


    private void LateUpdate()
    {
        /*
        fadeScreen.SetBool("Fade", false);
        */
    }
}
