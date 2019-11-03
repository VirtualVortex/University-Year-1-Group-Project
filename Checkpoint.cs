using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Checkpoint : MonoBehaviour {

    public SaveLoadSystem save;

    Animator saveAnim;

    private void Start()
    {
        saveAnim = GameObject.Find("SaveLoadAnimation").GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        saveAnim.SetBool("isPlaying", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("Check point");

            saveAnim.SetBool("isPlaying", true);

            save.SaveData();
        }
    }
}
