using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class DialogSystem : MonoBehaviour
{

    [SerializeField]
    private string npcName;
    [SerializeField]
    private Sprite expression;
    [SerializeField]
    private Sprite[] NPCImages;
    /*[SerializeField]
    private Sprite[] prologImages;
    [SerializeField]
    private Image prologImageDisplay;
    [SerializeField]
    private bool forProlog;*/
    [SerializeField]
    private Image dialogimage;
    [SerializeField]
    private string storedName;
    [SerializeField]
    private string storedSentance;
    [SerializeField]
    private int setIndex;
    [SerializeField]
    private Text nameTag;
    [SerializeField]
    private Text Sentance;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject dialogBox;
    [SerializeField]
    private GameObject icon;
    [SerializeField]
    private TextAsset pathInBuild;

    private Sprite[] expressions;
    private string pathInEditro;
    private string txtPath;
    private bool canContinue;

    int number = -1;
    string[] words;
    StreamReader sr;
    string[] sentance;
    string file;
    List<DialogListClass> dialog;
    private bool endOfSentance;
    private bool outOfTalkRange;
    private bool openDialogBox;
    private bool inTalkingSpace;
    string[] characters;
    int startIndex;

    [Header("Trigger dialog")]
    [SerializeField]
    private bool onTrigger;
    [SerializeField]
    private float delayBetweenSentances;
    private float delay;

    [Header("Shrine Stuff")]
    public bool isShrine = false;
    private bool activated = false;

    [Header("Shrine Animation")]
    [SerializeField]
    private Animator ani;
    private bool runAnimation;

    // Use this for initialization
    void Start()
    {
        //At the start the dialogue will be broken down to be display and the icon and dialogue box will be hidden
        Dialog();
        dialogimage.GetComponent<Image>();
        dialogBox.SetActive(false);
        icon.SetActive(false);
        startIndex = setIndex;

    }

    // Update is called once per frame
    void Update()
    {
        //if the player is in talking space and onTrigger is false
        //then the system will display the dialogue and go onto the next sentance when E is pressed
        if (!onTrigger)
        {
            if (inTalkingSpace && !outOfTalkRange)
            {
                icon.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    endOfSentance = false;
                    WordSearch();
                }
            }
            else
                endOfSentance = false;

            if (endOfSentance)
            {
                Debug.Log("end of sentance");
                nameTag.text = npcName;
                Sentance.text = "";
                setIndex = startIndex;
                dialogBox.SetActive(false);
            }
        }

        //if the player is in talking space and onTrigger is true
        //then the system will run through the dialogue at a set pace until it detects the end
        if (onTrigger)
        {
            if (inTalkingSpace && !activated)
            {
                if (Time.time > delay && !endOfSentance)
                {
                    Debug.Log("Showing Text");
                    Sentance.text = dialog[setIndex++].Name;
                    WordSearch();
                    delay = Time.time + delayBetweenSentances;
                }
            }
            else
                endOfSentance = false;

            if (endOfSentance)
            {
                nameTag.text = npcName;
                Sentance.text = "";
                dialogBox.SetActive(false);
            }

            if (endOfSentance)
            {
                delay = 0;
                endOfSentance = false;
            }
        }

        
        
            
        //Debug.Log("End of sentance: " + endOfSentance);
    }

    void Dialog()
    {
        file = pathInBuild.text;

        //If Unity can't find anything in the text file is will convert the bytes in the file to string
        //making it readable for the system
        if (file == "")
            file = System.Text.Encoding.Default.GetString(pathInBuild.bytes);


        words = file.Split('|');
        sentance = file.Split('/');
        dialog = new List<DialogListClass>();

        //The foreach loop will break down the sentance and apply it to a list along with a number
        //this will allow the system to move onto the next sentance.
        foreach (string word in words)
        {
            dialog.Add(new DialogListClass(word, number += 1));

            foreach (DialogListClass set in dialog)
            {
                if (set.Name.Contains(npcName))
                {
                    storedName = set.Name;
                    setIndex = set.Index;
                    storedSentance = dialog[setIndex++].Name;
                }
            }
        }
    }

    //The function below will run different things depending on the word or letter it detects
    public void WordSearch()
    {
        dialogBox.SetActive(true);
        openDialogBox = true;
        if(!onTrigger)
            Sentance.text = dialog[setIndex++].Name;

        if (Sentance.text.Contains("@"))
        {
            nameTag.text = npcName;
            dialogimage.sprite = expression;
            Sentance.text = dialog[setIndex++].Name;
        }

        if (Sentance.text.Contains("/"))
        {
            endOfSentance = true;
            delay = 0;
            if (onTrigger)
                Destroy(gameObject);
        }

        if (Sentance.text.Contains("Destroy"))
        {
            endOfSentance = true;

            if (isShrine)
            {
                activated = true;
                BroadcastMessage("AnimalCollectMusic");
                runAnimation = true;
                ani.SetBool("Deactivate", runAnimation);
            }
        }

        if (Sentance.text.Contains("Aro:"))
        {
            nameTag.text = "Aro";
            dialogimage.sprite = NPCImages[0];
            Sentance.text = dialog[setIndex++].Name;
        }

        if (Sentance.text.Contains("Bjorn:"))
        {
            dialogimage.sprite = NPCImages[1];
            nameTag.text = "Bjorn";
            Sentance.text = dialog[setIndex++].Name;
        }

        if (Sentance.text.Contains("Quiyll:"))
        {
            dialogimage.sprite = NPCImages[2];
            nameTag.text = "Quiyll";
            Sentance.text = dialog[setIndex++].Name;
        }

        if (Sentance.text.Contains("Theo:"))
        {
            nameTag.text = "Theo";
            dialogimage.sprite = NPCImages[3];
            Sentance.text = dialog[setIndex++].Name;
        }

        if (Sentance.text.Contains("CEP:"))
        {
            nameTag.text = "Cep";
            dialogimage.sprite = NPCImages[4];
            Sentance.text = dialog[setIndex++].Name;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When the player collides with the trigger and onTrigger is true
        //then the dialogue box will appear
        if (onTrigger)
        {
            dialogBox.SetActive(true);
        }
        

        nameTag.text = npcName;
        dialogimage.sprite = expression;

        //Debug.Log("Delay: " + delay);

        //If the player collides with the trigger then theinteract icon will appear
        //and the player will be able to run the dialogue
        if (collision.CompareTag("Player"))
        {
            inTalkingSpace = true;
            outOfTalkRange = false;
            icon.SetActive(true);
            endOfSentance = false;
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //The dialogue box will remain open whether in or out of the collider until it detects the end of the sentance
        if (collision.CompareTag("Player") && onTrigger)
        {
            if (endOfSentance)
            {
                nameTag.text = npcName;
                Sentance.text = "";
                delay = 0;
                setIndex = startIndex;
                dialogBox.SetActive(false);
                icon.SetActive(false);
                endOfSentance = true;
                inTalkingSpace = false;
            }
        }
        
        //If ontrigger is false when the dialoge box will close when the player leaves
        if (collision.CompareTag("Player") && !onTrigger)
        {
            outOfTalkRange = true;
            inTalkingSpace = false;
            icon.SetActive(false);
            dialogBox.SetActive(false);
        }
    }
    
}
