using UnityEngine;
using UnityEngine.UI;

public class DisplayKeyOnStart : MonoBehaviour {


    public string action;
    Text text;
    PlayerControlsSettings controlsSettings;

    

	// Use this for initialization
	void Start () {
        text = GetComponentInChildren<Text>();
        controlsSettings = GetComponentInParent<PlayerControlsSettings>();
        text.text = controlsSettings.controls[action];
	}

    public void ResetText()
    {
        text.text = controlsSettings.controls[action];
    }
}
