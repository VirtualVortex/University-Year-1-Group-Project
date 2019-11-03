using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CrystalCount : MonoBehaviour {

    public string LevelToLoad;
    public Text crystalsLeft;
    public int numberToWin = 1;
    private int crystalsDestroird = 0;
    BoxCollider2D box;


	// Use this for initialization
	void Start () {
        box = GetComponent<BoxCollider2D>();
        box.isTrigger = false;

        crystalsLeft.text = numberToWin.ToString();
	}

    void Update()
    {
        if ((numberToWin - crystalsDestroird) == 0)
        {
            SceneManager.LoadScene(LevelToLoad);
        }
    }

    public void CrystalBroken ()
    {
        crystalsDestroird++;
        crystalsLeft.text = (numberToWin - crystalsDestroird).ToString();
    }
}
