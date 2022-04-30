using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//This will be placed on every control to allow it to function together
[ExecuteInEditMode]
public class Control : MonoBehaviour
{
    #region Variables
    [HideInInspector]
    public string uniqueName = "";

    public enum ControlType
    {
        None,
        Text,
        Image,
        RawImage,
        Button,
        Toggle,
        Dropdown,
        InputField,
        Window
    }
    public ControlType control_Type = ControlType.None;

    public Text text_Script = null;
    public Image image_Script = null;
    public RawImage rawImage_Script = null;
    public Button button_Script = null;
    public Toggle toggle_Script = null;
    public InputField inputField_Script = null;
    public Dropdown dropdown_Script = null;
    public Window Window_Script = null;

    public Window controlled_by_window = null;
    public string group = null;
    public int order = 0;
    public bool skip = false;
    public bool active = true;
    public bool loop = true;
    public bool nextControlOnSubmit = true;

    public ControlKeys submit_Control_Key = new ControlKeys(KeyCode.Return);
    public ControlKeys prev_Control_Key = new ControlKeys(KeyCode.LeftShift,KeyCode.Tab);
    public ControlKeys next_Control_Key = new ControlKeys(KeyCode.Tab);

    float next_Select = 0;
    #endregion

    void Start()
    {
        FindConnectedControl();
        CreateUniqueName();

        prev_Control_Key.keysList.Add(new List<KeyCode>() { KeyCode.RightShift, KeyCode.Tab });
        prev_Control_Key.keysList.Add(new List<KeyCode>() { KeyCode.LeftShift,KeyCode.LeftArrow});
        prev_Control_Key.keysList.Add(new List<KeyCode>() { KeyCode.RightShift, KeyCode.LeftArrow });

        next_Control_Key.keysList.Add(new List<KeyCode>() { KeyCode.LeftShift, KeyCode.RightArrow });
        next_Control_Key.keysList.Add(new List<KeyCode>() { KeyCode.RightShift, KeyCode.RightArrow });

        if(control_Type == ControlType.Window)
        {
            BroadcastMessage("ControlledByWindow", Window_Script, SendMessageOptions.DontRequireReceiver);
        }
    }

    void Update()
    {
        if(Selected() && next_Select < Time.time)
        {
            CheckForKeyPressed();
        }

        if(control_Type == ControlType.Image || control_Type == ControlType.RawImage ||
            control_Type == ControlType.Text || control_Type == ControlType.None)
        {
            skip = true;
        }

        if (control_Type == ControlType.Window)
        {
            submit_Control_Key = new ControlKeys();
            prev_Control_Key = new ControlKeys();
            next_Control_Key = new ControlKeys();

            if (Window_Script.currentChildCount != transform.childCount)
            {
                BroadcastMessage("ControlledByWindow", Window_Script, SendMessageOptions.DontRequireReceiver);

                Window_Script.currentChildCount = transform.childCount;
            }
        }

        if(controlled_by_window != null)
        {
            if (controlled_by_window.auto_group)
            {
                group = controlled_by_window.innerGroup;
            }
        }
    }

    bool Selected()
    {
        bool result = false;

        EventSystem eS = EventSystem.current;

        if(eS != null)
        {
            if (eS.currentSelectedGameObject == gameObject)
            {
                result = true;
            }
        }

        return result;
    }

    void CheckForKeyPressed()
    {
        if (submit_Control_Key.KeysPressed() > 0)
        {
            if (nextControlOnSubmit)
            {
                MoveControlItem(1);
            }
        }
        else if (prev_Control_Key.KeysPressed() > 0 && next_Control_Key.KeysPressed() > 0)
        {
            if(prev_Control_Key.KeysPressed() > next_Control_Key.KeysPressed())
            {
                MoveControlItem(-1);
            }
            else
            {
                MoveControlItem(1);
            }
        }
        else if (prev_Control_Key.KeysPressed() > 0)
        {
            MoveControlItem(-1);
        }
        else if (next_Control_Key.KeysPressed() > 0)
        {
            MoveControlItem(1);
        }
    }

    public void MoveControlItem(int dir)
    {
        EventSystem eS = EventSystem.current;
        eS.SetSelectedGameObject(null);

        List<Control> aCs = GetAllControls();

        //Remove all from a different window or no Order number or inactive or SkipTab
        if (aCs.Count > 0)
        {
            for (int i = aCs.Count - 1; i >= 0; i--)
            {
                if (aCs[i].order < 0 || aCs[i].group != group || !aCs[i].active || aCs[i].skip)
                {
                    aCs.RemoveAt(i);
                }
            }

            //Checks if it is alone or if none was found
            if (aCs.Count > 1)
            {
                //Sort by Order number
                aCs.Sort((p1, p2) => p1.order.CompareTo(p2.order));

                //Get Position of itself in the list
                int p = aCs.FindIndex(x => x.uniqueName == uniqueName);

                //If itself is not found start from begining
                if (p < 0)
                {
                    p = -1;
                }

                if (p >= -1)
                {
                    if (dir > 0)
                    {
                        if (p + dir >= aCs.Count)
                        {
                            if(loop)
                            {
                                //Get the remainder and go to that selection
                                int remander = (p + dir) % aCs.Count;
                                //Debug.Log(remander + " from (" + p + " + " + dir + ") % " + aCs.Count);

                                aCs[remander].SelectControl();
                            }
                        }
                        else
                        {
                            aCs[p + dir].SelectControl();
                        }
                    }
                    else if (dir < 0)
                    {
                        if (p + dir < 0)
                        {
                            if (loop)
                            {
                                int newSel = p;

                                for(int i = 0; i <= Mathf.Abs(dir); i++)
                                {
                                    newSel--;

                                    if(newSel < 0)
                                    {
                                        newSel = aCs.Count - 1;
                                    }
                                }

                                aCs[newSel].SelectControl();
                            }
                        } 
                        else
                        {
                            aCs[p + dir].SelectControl();
                        }
                    }
                }
            }
        }
    }

    public void SelectControl()
    {
        next_Select = Time.time + .5f;
        EventSystem eS = EventSystem.current;
        eS.SetSelectedGameObject(gameObject, null);

        switch(control_Type)
        {
            case ControlType.InputField:
                inputField_Script.selectionFocusPosition = inputField_Script.text.Length;
                break;
        }
    }

    //Will get all active Form Manager Items
    List<Control> GetAllControls()
    {
        List<Control> result = new List<Control>();

        Control[] aControls = GameObject.FindObjectsOfType<Control>();

        if (aControls.Length > 0)
        {
            foreach (Control cn in aControls)
            {
                result.Add(cn);
            }
        }

        return result;
    }

    public void FindConnectedControl()
    {
        Window_Script = gameObject.GetComponent<Window>();

        if (Window_Script == null)
        {
            dropdown_Script = gameObject.GetComponent<Dropdown>();

            if (dropdown_Script == null)
            {
                inputField_Script = gameObject.GetComponent<InputField>();

                if(inputField_Script == null)
                {
                    toggle_Script = gameObject.GetComponent<Toggle>();

                    if (toggle_Script == null)
                    {
                        button_Script = gameObject.GetComponent<Button>();

                        if (button_Script == null)
                        {
                            rawImage_Script = gameObject.GetComponent<RawImage>();

                            if (rawImage_Script == null)
                            {
                                image_Script = gameObject.GetComponent<Image>();

                                if (image_Script == null)
                                {
                                    text_Script = gameObject.GetComponent<Text>();

                                    if (text_Script == null)
                                    {
                                        control_Type = ControlType.None;
                                    }
                                    else
                                    {
                                        control_Type = ControlType.Text;
                                    }
                                }
                                else
                                {
                                    control_Type = ControlType.Image;
                                }
                            }
                            else
                            {
                                control_Type = ControlType.RawImage;
                            }
                        }
                        else
                        {
                            control_Type = ControlType.Button;
                        }
                    }
                    else
                    {
                        control_Type = ControlType.Toggle;
                    }
                }
                else
                {
                    control_Type = ControlType.InputField;
                }
            }
            else
            {
                control_Type = ControlType.Dropdown;
            }
        }
        else
        {
            control_Type = ControlType.Window;
        }
    }

    void ControlledByWindow(Window window)
    {
        if(control_Type != ControlType.Window || (control_Type == ControlType.Window && window != Window_Script))
        {
            controlled_by_window = window;
        }
    }

    //Create Unique Name
    void CreateUniqueName()
    {
        string result = "";

        List<string> unsl = new List<string> { "A", "B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
        "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z","0","1","2","3","4","5","6","7","8","9"};

        //Will Create a 50 char unique Id
        for (int i = 0; i < 50; i++)
        {
            result += unsl[Random.Range(0, unsl.Count)];
        }

        uniqueName = result;
    }
}