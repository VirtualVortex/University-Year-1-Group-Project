using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stealth : MonoBehaviour
{

    public bool isInvisible;
    public bool canBeInvis;
    public SpriteRenderer material;
    public float barScale;
    Color alpha;
    Color outLineAlpha;

    [Header("Invisibility UI")]
    public Image stealthOutLine;
    public Image chargeBar;
    public Text gemAmountText;
    public int gemAmount;

    public Animator ani;

    private float nextPress;

    AudioPlayer PlayerAudio;

    // Use this for initialization
    void Start()
    {
        PlayerAudio = GetComponentInChildren<AudioPlayer>();

        alpha.a = 1;
        outLineAlpha.a = 1;
        alpha = new Color(255, 255, 255);
        gemAmount = 0;
        barScale = 0;
        stealthOutLine.enabled = false;
        //Debug.Log(barScale);
    }

    // Update is called once per frame
    void Update()
    {

        

        material.GetComponent<SpriteRenderer>().color = alpha;
        stealthOutLine.GetComponent<Image>().color = outLineAlpha;

        //When the player presses the space button the player turns invisible for 5 seconds
        if (Input.GetKeyDown(KeyCode.Q) && isInvisible == false)
        {
            //If the player has more than zero gems then the player can become invisible
            //and a gem will be used
            if (gemAmount > 0)
            {
                PlayerAudio.PlayStealthSound();

                gemAmount--;
                canBeInvis = true;
                barScale = 1;
                ani.SetBool("isInvisible", canBeInvis);
                StartCoroutine(phaseIn());
            }
            //Else if the player has none then the player can't be invisible
            else if (gemAmount <= 0)
            {
                canBeInvis = false;
                PlayerAudio.PlayFailedStealth();
            }
        }

        //If the player can be invisible then the player will be transparent
        //until the charge bar is empty 
        if (canBeInvis == true)
        {
            if (Time.time > nextPress)
            {
                nextPress = Time.time + 5;
                alpha.a = 0.5f;
                isInvisible = true;
            }

            if (Time.time < nextPress)
            {
                barScale -= Time.deltaTime * 0.1f;

                if (barScale <= 0)
                {
                    barScale = 0;
                }
            }
            chargeBar.rectTransform.localScale = new Vector3(barScale, 1, 1);
        }

        //When the bar is empty the player will be made visible again
        if (barScale <= 0)
        {
            alpha.a = 1f;
            StartCoroutine(phaseOut());
            isInvisible = false;
            canBeInvis = false;

            ani.SetBool("isInvisible", false);
        }



    }

    public void GoingInvisible()
    {
        
    }

    IEnumerator phaseIn()
    {
        stealthOutLine.enabled = true;
        yield return new WaitForSeconds(0.02f);
        outLineAlpha.a = Mathf.Lerp(1, 0.2f, (Time.deltaTime / 0.25f));
    }

    IEnumerator phaseOut()
    {
        outLineAlpha.a = Mathf.Lerp(0.2f, 1, (Time.deltaTime / 0.25f));
        yield return new WaitForSeconds(0.02f);
        stealthOutLine.enabled = false;
    }
}
