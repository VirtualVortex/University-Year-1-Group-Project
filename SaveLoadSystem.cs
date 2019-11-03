
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;




public class SaveLoadSystem : MonoBehaviour
{

    public AnimalAI animal;
    public PlayerMoverment1 player;
    public Stealth stealth;
    public KeyScript[] keys;
    KeyScript key;

    [Header("Reset")]
    public bool Reset = false;
    public Vector2 startPos;

    [Header("Scene 2 Start pos")]
    public Vector2 Pos;

    Data characterData = new Data();
    string path;
    int arrayLength;

    private int sceneNumber;
    
    // Use this for initialization
    void Start () {
        
        animal.GetComponent<AnimalAI>();
        stealth.GetComponent<Stealth>();
        //player.GetComponent<PlayerMoverment1>();
        //path = Path.Combine(Application.persistentDataPath, "Data.txt");

        //Debug.Log(player.SwitchToScene2);

        LoadData();

        if (player != null)
        {
            
            if (player.freedBear)
            {
                //Debug.Log("Found bear");
                Destroy(GameObject.FindGameObjectWithTag("Bear"));
                animal.haveBear = true;
            }



            if (player.freedBird)
            {
                //Debug.Log("Found bird");
                Destroy(GameObject.FindGameObjectWithTag("Bird"));
                animal.haveBird = true;
            }


            if (player.freedHare)
            {
                //Debug.Log("Found hare");
                Destroy(GameObject.FindGameObjectWithTag("Rabbit"));
                animal.haveRabbit = true;
            }
                
        }
        
        
        
	}
	
    
	// Update is called once per frame
	void Update () {

        

        //Set the fields in the data class to the values from the bool variables in the animalAI script 
        if (animal != null)
        {
            characterData.bear = animal.haveBear;
            characterData.bird = animal.haveBird;
            characterData.hare = animal.haveRabbit;
            
        }

        if (player != null)
        {
            characterData.Health = player.health;
            characterData.x = player.transform.position.x;
            characterData.y = player.transform.position.y;
            characterData.sceneNumber = player.level;
            characterData.bearFreeded = player.freedBear;
            characterData.birdFreeded = player.freedBird;
            characterData.hareFreeded = player.freedHare;

            //Debug.Log(characterData.sceneNumber);

            if (player.SwitchToScene2 == true)
            {
                //Debug.Log("Scene2 Saving");
                characterData.x = Pos.x;
                characterData.y = Pos.y;
                player.transform.position = new Vector2(Pos.x, Pos.y);
                SaveData();
            }
        }

        if (player != null)
        {
            characterData.gemAmount = player.GetComponent<Stealth>().gemAmount;
        }
        
        if (keys.Length != 0)
        {
            //Debug.Log("key data");
            characterData.haveKey1 = keys[0].haveKey;
            characterData.haveKey2 = keys[1].haveKey;
            characterData.haveKey3 = keys[2].haveKey;
        }
        

        if (Reset)
        {
            characterData.bear = false;
            characterData.bird = false;
            characterData.hare = false;
            characterData.Health = 300;
            characterData.x = startPos.x;
            characterData.y = startPos.y;
            characterData.sceneNumber = 1;
            characterData.gemAmount = 0;
            
            PlayerPrefs.SetInt("Bear", Convert.ToInt16(characterData.bear));
            PlayerPrefs.SetInt("Bird", Convert.ToInt16(characterData.bird));
            PlayerPrefs.SetInt("Hare", Convert.ToInt16(characterData.hare));
            PlayerPrefs.SetFloat("Scene", characterData.sceneNumber);
            PlayerPrefs.SetFloat("Health", characterData.Health);
            PlayerPrefs.SetFloat("X", characterData.x);
            PlayerPrefs.SetFloat("Y", characterData.y);
            PlayerPrefs.SetInt("GemAmount", characterData.gemAmount);
            PlayerPrefs.SetInt("AnimalFreeded1", Convert.ToInt16(characterData.bearFreeded = false));
            PlayerPrefs.SetInt("AnimalFreeded2", Convert.ToInt16(characterData.birdFreeded = false));
            PlayerPrefs.SetInt("AnimalFreeded3", Convert.ToInt16(characterData.hareFreeded = false));

            characterData.haveKey1 = false;
            characterData.haveKey2 = false;
            characterData.haveKey3 = false;
        }
    }
    
    

    
    public void SaveData()
    {


        
        //player prefs saving method
        //Save which animals the player has and conver the booleans to int 
        //true = 1 and false = 0;
        PlayerPrefs.SetInt("Bear", Convert.ToInt16(characterData.bear));
        PlayerPrefs.SetInt("Bird", Convert.ToInt16(characterData.bird));
        PlayerPrefs.SetInt("Hare", Convert.ToInt16(characterData.hare));
        PlayerPrefs.SetFloat("Scene", characterData.sceneNumber);
        PlayerPrefs.SetFloat("Health", player.health);
        PlayerPrefs.SetFloat("X", player.transform.position.x);
        PlayerPrefs.SetFloat("Y", player.transform.position.y);
        PlayerPrefs.SetInt("GemAmount", player.GetComponent<Stealth>().gemAmount);
        PlayerPrefs.SetInt("AnimalFreeded1", Convert.ToInt16(characterData.bearFreeded));
        PlayerPrefs.SetInt("AnimalFreeded2", Convert.ToInt16(characterData.birdFreeded));
        PlayerPrefs.SetInt("AnimalFreeded3", Convert.ToInt16(characterData.hareFreeded));

        if (keys != null)
        {
            Debug.Log("save key data");
            PlayerPrefs.SetInt("key1", Convert.ToInt16(characterData.haveKey1));
            PlayerPrefs.SetInt("key2", Convert.ToInt16(characterData.haveKey2));
            PlayerPrefs.SetInt("key3", Convert.ToInt16(characterData.haveKey3));

            /*foreach (KeyScript key in keys)
            {
                Debug.Log("Saved data: Can access keys");
                if (characterData.haveKey != null)
                {
                    Debug.Log("Saved data: Accessed keys");
                    PlayerPrefs.SetInt("keys", Convert.ToInt16(characterData.haveKey[keys.Length]));
                }
            }
            */
        }
        

        //write all the information saved above to disk
        PlayerPrefs.Save();
       
    }

        
    

    public void LoadData()
    {
        
        //player prefs way of saving data
        //Load which Animals the player have and convert the ints to boolean
        if (animal != null)
        {
            //Debug.Log("Have Animals");
            animal.haveBear = Convert.ToBoolean(PlayerPrefs.GetInt("Bear", Convert.ToInt16(characterData.bear)));
            animal.haveBird = Convert.ToBoolean(PlayerPrefs.GetInt("Bird", Convert.ToInt16(characterData.bird)));
            animal.haveRabbit = Convert.ToBoolean(PlayerPrefs.GetInt("Hare", Convert.ToInt16(characterData.hare)));
            
        }

        if (player != null)
        {
            //Debug.Log("Have Player");
            player.health = PlayerPrefs.GetFloat("Health", player.health);
            player.transform.position = new Vector2(PlayerPrefs.GetFloat("X", player.transform.position.x), PlayerPrefs.GetFloat("Y", player.transform.position.y));
            player.GetComponent<Stealth>().gemAmount = PlayerPrefs.GetInt("GemAmount", player.GetComponent<Stealth>().gemAmount);
            player.freedBear = Convert.ToBoolean(PlayerPrefs.GetInt("AnimalFreeded1", Convert.ToInt16(characterData.bearFreeded)));
            player.freedBird = Convert.ToBoolean(PlayerPrefs.GetInt("AnimalFreeded2", Convert.ToInt16(characterData.birdFreeded)));
            player.freedHare = Convert.ToBoolean(PlayerPrefs.GetInt("AnimalFreeded3", Convert.ToInt16(characterData.hareFreeded)));
        }


        if (keys != null)
        {
            Debug.Log("Load key data");
            keys[0].haveKey = Convert.ToBoolean(PlayerPrefs.GetInt("key1", Convert.ToInt16(characterData.haveKey1)));
            keys[1].haveKey = Convert.ToBoolean(PlayerPrefs.GetInt("key2", Convert.ToInt16(characterData.haveKey2)));
            keys[2].haveKey = Convert.ToBoolean(PlayerPrefs.GetInt("key3", Convert.ToInt16(characterData.haveKey3)));

        }
        /*foreach (KeyScript key in keys)
            {
                Debug.Log("load data: Can access keys");
                if (key)
                {
                    Debug.Log("load data: Accessed keys");
                    key.haveKey = Convert.ToBoolean(PlayerPrefs.GetInt("keys", Convert.ToInt16(characterData.haveKey[arrayLength])));
                }
            }
        }
        */
        
    }

    public void SavedScene()
    {
        Debug.Log(characterData.sceneNumber);
        SceneManager.LoadScene(Convert.ToInt16(PlayerPrefs.GetFloat("Scene", characterData.sceneNumber)));
    }

    public void NewGame()
    {
        Reset = true;
        SaveData();
        LoadData();
        SavedScene();
    }
    
}
