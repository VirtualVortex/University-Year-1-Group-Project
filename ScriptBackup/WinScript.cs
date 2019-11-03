using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour {
    public string LevelToLoad;

	
    void OnTriggerEnter2D(Collider2D other)
    {
        if (string.IsNullOrEmpty(LevelToLoad))
        {
            Debug.Log("eMPTY");
            return;
        }
               

        if (other.tag == "Player")
            SceneManager.LoadScene(LevelToLoad);
    }
}
