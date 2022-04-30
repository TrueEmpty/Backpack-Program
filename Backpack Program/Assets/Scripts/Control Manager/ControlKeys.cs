using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlKeys
{
    public List<List<KeyCode>> keysList = new List<List<KeyCode>>();

    public ControlKeys()
    {

    }

    public ControlKeys(params KeyCode[] defaultKeys)
    {
        if(defaultKeys.Length > 0)
        {
            List<KeyCode> insert_keys = new List<KeyCode>();

            for(int i = 0; i < defaultKeys.Length; i++)
            {
                insert_keys.Add(defaultKeys[i]);
            }

            keysList.Add(insert_keys);
        }
    }

    public int KeysPressed()
    {
        int result = -1;

        if (keysList.Count > 0)
        {
            for (int i = 0; i < keysList.Count; i++)
            {
                if (KeysUsed(keysList[i]))
                {
                    if(result < keysList[i].Count)
                    {
                        result = keysList[i].Count;
                    }
                }
            }
        }

        return result;
    }

    bool KeysUsed(List<KeyCode> keys)
    {
        bool result = false;

        if (keys.Count > 0)
        {
            bool check_all = true;

            for (int i = 0; i < keys.Count; i++)
            {
                if (!Input.GetKey(keys[i]))
                {
                    check_all = false;
                    break;
                }
            }

            result = check_all;
        }

        return result;
    }
}
