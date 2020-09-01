using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialSound : MonoBehaviour {

    [SerializeField]
    protected AudioSource audioSource;

    [SerializeField]
    protected float maxAudioDistance;

    protected GameObject Player
    {
        get { return Player; }
        set { }
    }

    private float distanceX;
    private float distanceY;
    private float fullDistance;


    // Use this for initialization
    void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        
        distanceX = Player.transform.position.x - transform.position.x;
        distanceY = Player.transform.position.y - transform.position.x;

        fullDistance = Vector2.Distance(transform.position, Player.transform.position);

        // Sets Volume to zero if player is too far away.
        if (fullDistance >= maxAudioDistance)
        {
            audioSource.volume = 0;
        }
        else
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
    }
}
