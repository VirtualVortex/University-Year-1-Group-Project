using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[Serializable]
public class Controls
{
    // Each control has to be stored as a seperate string.
    public string Up;
    public string Down;
    public string Left;
    public string Right;
    public string Jump;
    public string Bird;
    public string Bear;
    public string Hare;
    public string Activate;
    public string Stealth;
}

public class PlayerControlsSettings : MonoBehaviour {

    public Button saveChangesButton;
    public Button resetChangesButton;
    public GameObject eventSystem;
    Text inputText;
    private bool getKey = false;
    private string newKeyInput;
    private string _action;

    // Save/Load Data
    [HideInInspector]
    public Controls localControls;
    string dataPath;

    // The controls dictionary that is accesed from other scripts.
    [HideInInspector, SerializeField]
    public Dictionary<string, string> controls = new Dictionary<string, string>
    {
        {"Up", "w" },
        {"Down", "s" },
        {"Left", "a" },
        {"Right", "d" },
        {"Jump",  "space"},
        {"Bird", "1" },
        {"Bear", "2" },
        {"Hare", "3" },
        {"Activate", "left shift" },
        {"Stealth", "f"}
    };
    


    private void Start()
    {
        // Disables the "Apply Changes" button.
        saveChangesButton.interactable = false;
        eventSystem.SetActive(true);

        // Retrives the path where the data is stored.
        dataPath = Path.Combine(Application.persistentDataPath, "Controls.txt");

        try
        {
            // Tries to store all the controls from the file into seperate strings. 
            // Unfortunatally, dictionaries cannot be saved into a file. I hope I'm wrong, but it's too late now.
            localControls = LoadControls(dataPath);

            // To allow the player to make changes, they get stored into a dictionary.
            controls["Up"] = localControls.Up;
            controls["Down"] = localControls.Down;
            controls["Left"] = localControls.Left;
            controls["Right"] = localControls.Right;
            controls["Jump"] = localControls.Jump;
            controls["Bird"] = localControls.Bird;
            controls["Bear"] = localControls.Bear;
            controls["Hare"] = localControls.Hare;
            controls["Activate"] = localControls.Activate;
            controls["Stealth"] = localControls.Stealth;
        }
        catch (FileNotFoundException)
        {
            Debug.LogWarning("Controls file not found. Creating new controls file...");

            // If no file was found then the default values are asigned to the seperate strings.
            localControls.Up = controls["Up"];
            localControls.Down = controls["Down"];
            localControls.Left = controls["Left"];
            localControls.Right = controls["Right"];
            localControls.Jump = controls["Jump"];
            localControls.Bird = controls["Bird"];
            localControls.Bear = controls["Bear"];
            localControls.Hare = controls["Hare"];
            localControls.Activate = controls["Activate"];
            localControls.Stealth = controls["Stealth"];

            // The seperate strings are saved to a new file.
            SaveControls(localControls, Path.Combine(Application.persistentDataPath, "Controls.txt"));

            throw;
        }
    }

    
    private void Update()
    {
        // For entering a new key input.
        if (getKey)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                getKey = false;
                inputText.text = controls[_action];
                eventSystem.SetActive(true);
            }
            else
            {
                // Detect what key is being pressed.
                // For each entery in the KeyCode enum,
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    // If the player has pressed a key. 
                    if (Input.GetKeyDown(keyCode))
                    {
                        // The pressed key is converted into the key name used by Unity.
                        // Eg KeyCode.Alpha1 into the string "1"
                        newKeyInput = KeycodeToKeyname(keyCode.ToString());
                    
                        try
                        {
                            // Tries to use the key name as an input. 
                            Input.GetKeyDown(newKeyInput);
                        }
                        catch (System.ArgumentException)
                        {
                            // If it can't then it is rejected.
                            eventSystem.SetActive(true);
                            inputText.text = "<Invalid Key!>";
                            getKey = false;
                            throw;
                        }

                        // The "Apply Changes" button is enabled.
                        saveChangesButton.interactable = true;

                        // The Controls dictionary is updated.
                        controls[_action] = newKeyInput;

                        // The text on the button shows the new key
                        inputText.text = newKeyInput;
                        getKey = false;
                        eventSystem.SetActive(true);
                        break;
                    }
                }
            }
        }
    }

    public void ResetControls()
    {
        // Just sets everything to the default values.
        controls["Up"] = "w";
        controls["Down"] = "s";
        controls["Left"] = "a";
        controls["Right"] = "d";
        controls["Jump"] = "space";
        controls["Bird"] = "1";
        controls["Bear"] = "2";
        controls["Hare"] = "3";
        controls["Activate"] = "left shift";
        controls["Stealth"] = "f";

        // The "Apply Changes" button is enabled.
        saveChangesButton.interactable = true;
    }

    // When player wants to change the controls, this function is called,
    // with the paramater (internal only) of what the action is.
    public void EnterNewKey (string action)
    {
        // If the action exists within the game.
        if (controls[action] != null)
        {
            // the internal string action is made global.
            _action = action;

            // The event system is disabled, we don't want the enter/space button to reactivate the new input button.
            eventSystem.SetActive(false);

            // The text on the button displays:
            inputText.text = "<Enter New Key>";

            // The game is now listening for a new key to be pressed.
            getKey = true;
        }
        else
        {
            // !!!!!!!!!!!! ----------- List of available actions are at the top of this script! ----------- !!!!!!!!!!!! //
            Debug.LogError(action + " is not an action. Double click this meassage to view available actions. Be sure to check spelling and upper/lower case.");
        }
    }

    // This function is called when ever a button is clicked,
    // because I can't have more than one argument in the EnterNewKey method and call it from a button click.
    public void InputDisplayText(GameObject displayText)
    {
        inputText = displayText.GetComponent<Text>();
    }

    // Called when the player clicks "Apply Changes".
    public void SaveChanges()
    {
        // Assigns all controls from the dictioary into the appropiate strings.
        localControls.Up = controls["Up"];
        localControls.Down = controls["Down"];
        localControls.Left = controls["Left"];
        localControls.Right = controls["Right"];
        localControls.Jump = controls["Jump"];
        localControls.Bird = controls["Bird"];
        localControls.Bear = controls["Bear"];
        localControls.Hare = controls["Hare"];
        localControls.Activate = controls["Activate"];
        localControls.Stealth = controls["Stealth"];

        // Saves these new inputs to a file.
        SaveControls(localControls, dataPath);

        // The "Apply Changes" button is disabled as a way of telling the player that the changes have been saved.
        saveChangesButton.interactable = false;
    }

    // Converts KeyCode.<keycode> into the key names.
    private string KeycodeToKeyname (string keycode)
    {
        // Alpha1 key name is 1
        if (keycode.Contains("Alpha"))
            return keycode.Remove(0, 5);

        // Single letters set to lower case.
        else if (keycode.Length == 1)
            return keycode.ToLower();


        else if (keycode.Length > 1)
        {
            // This block of code works for most keys.

            // The keycode is stored into a tempary veriable so that it can be edited.
            string editKey = keycode;

            // It goes through each letter and if it's upper case, it set's it to lower and adds a space in front.
            // If it passed a number it just adds a space in front.
            // This is because most of the key names are the same as the KeyCode name, but without spaces and capitalized.
            for (int i = 0; i < keycode.Length; i++)
            {
                if (char.IsUpper(keycode[i]))
                {
                    editKey = editKey.Replace(keycode[i].ToString(), " " + keycode[i].ToString().ToLower());
                }
                else if (char.IsDigit(keycode[i]))
                    editKey = editKey.Insert(i + 1, " ");
            }

            // For the CTRL keys
            if (editKey.Contains("control"))
                editKey = editKey.Replace("control", "ctrl");

            // For the Arrow keys
            else if (editKey.Contains("arrow"))
            {
                // The arrow key names are in front of 'Arrow', so I have to state what characters I don't want.
                char[] trim = { 'a', 'r', 'r', 'o', 'w' };
                editKey = editKey.TrimEnd(trim);
            }

            // For the num. Pad. This one's easy, 'Keypad' is at the begining of all num pad keys.
            else if (editKey.Contains("keypad"))
            {
                editKey = editKey.Remove(0, 8);
                editKey = editKey.Insert(0, "[");
                editKey += "]";
            }
            
            // Returns the final product.
            return editKey.Trim();
        }
        else
        {
            Debug.Log("No changes where made to the keycode: " + keycode);
            return keycode;
        }

    }


    #region SaveLoad

    // Save player controls
    static void SaveControls (Controls data, string path)
    {
        // Converts data in file to a json string.
        string jasonString = JsonUtility.ToJson(data, true); 


        // Opens a stream to write data to the file then automatically closes the stream afterwards.
        using (StreamWriter stream = File.CreateText(path))
        {
            // Writes the data to the file.
            stream.Write(jasonString);
        }
    }

    // Load player controls
    static Controls LoadControls (string path)
    {
        // Opens a stream to read data from the file.
        using (StreamReader reader = File.OpenText(path))
        {
            // Reads everything in the file and returns it as one string.
            string jsonString = reader.ReadToEnd();

            // Returns the string and converts it back into the Controls object.
            return JsonUtility.FromJson<Controls>(jsonString);
        }
    }
    #endregion
}