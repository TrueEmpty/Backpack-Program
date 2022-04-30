using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAToColorize : MonoBehaviour
{
    [Tooltip("UIAnimate script. If none set will look for one within its gameObject")]
    public UIAnimate uia;
    [Tooltip("Colorize script. If none set will look for one within its gameObject")]
    public Colorize colorize;

    // Start is called before the first frame update
    void Start()
    {
        if(uia == null)
        {
            uia = GetComponent<UIAnimate>();
        }

        if(colorize == null)
        {
            colorize = GetComponent<Colorize>();
        }

        //Throw a warning if still null
        if(uia == null || colorize == null)
        {
            Debug.LogWarning("GameObject " + name + " could not connect to UIAnimate and/or Colorize script.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(uia != null  && colorize != null)
        {
            if (uia.baseKey != uia.inputKey && uia.onExtra)
            {
                colorize.disabled = true;
            }
            else
            {
                colorize.disabled = false;
            }
        }
    }
}
