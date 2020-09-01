using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOpendoor : MonoBehaviour {


    public KeyScript keyScript;
    public GameObject door;
    Animator Dooranim;


    private void Start()
    {
        
        Dooranim = GetComponentInParent<Animator>();

        Debug.Log(door);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && keyScript.haveKey)
        {

            Destroy(door);
            
        }
    }
}
