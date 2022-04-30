using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleManager : MonoBehaviour
{
    public static ConsoleManager instance;
    public GameObject content;
    public GameObject consoleInput;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void Write(string input)
    {
        GameObject go = Instantiate(consoleInput, content.transform) as GameObject;

        Text tx = go.GetComponent<Text>();

        if(tx != null)
        {
            tx.text = System.DateTime.Now + ": " + input;
        }
    }
}
