using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class PrologueChangeScene : MonoBehaviour {

    [SerializeField]
    private string scene;
    [SerializeField]
    private VideoClip vc;
    float timer;

	// Use this for initialization
	void Start () {
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;
        if (timer >= vc.length)
            SceneManager.LoadScene(scene);

        if(Input.GetKey(KeyCode.Space))
            SceneManager.LoadScene(scene);

    }
}
