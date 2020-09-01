using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour {
    public GameObject targetObject;
    public bool haveKey;
    

    void OnTriggerEnter2D(Collider2D Other)
    {

        if (targetObject == null)
            return;
        if (Other.tag != "Player")
            return;
        //targetObject.SetActive(false);
        haveKey = true;
    }
}
