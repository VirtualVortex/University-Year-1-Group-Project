using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameObject.tag == "Player")
        {
            transform.parent = transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
       if(gameObject.tag == "Player")
        {
            transform.parent = null;
        }
    }


}

