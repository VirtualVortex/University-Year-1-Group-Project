using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScriptOld : MonoBehaviour {
    public GameObject targetObject;
    public bool haveKey;

    void Start()
    {
        if (haveKey)
        {
            targetObject.SetActive(false);
            Destroy(gameObject);
        }
            
    }

    void OnTriggerEnter2D(Collider2D Other)
    {

        if (targetObject == null)
            return;
        if (Other.tag != "Player")
            return;
        targetObject.SetActive(false);
        Destroy(gameObject);
        haveKey = true;
    }
}
