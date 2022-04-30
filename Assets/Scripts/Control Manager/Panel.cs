using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Panel : MonoBehaviour
{
    public Texture background;
    public bool selOnEntry = true;

    public enum AutoOrder
    {
        None,
        DownToRight,
        DownToLeft,
        UpToRight,
        UpToLeft,
        RightToDown,
        LeftToDown,
        RightToUp,
        LeftToUp
    }

    public AutoOrder auto_order = AutoOrder.None;
    public bool auto_group = false;

    bool lastSelState = false;
    public bool selected = false;
    public string group = null;

    // Start is called before the first frame update
    void Start()
    {
        //If auto_group
        if(auto_group)
        {
            if(transform.childCount > 0)
            {
                for(int i = 0; i < transform.childCount; i++)
                {
                    Control c = transform.GetChild(i).GetComponent<Control>();

                    if(c != null)
                    {
                        c.group = group;
                    }
                }
            }
        }

        if(auto_order != AutoOrder.None)
        {
            List<GameObject> gol = GetAllControlChildrens();

            if(gol.Count > 0)
            {
                //Do proper sort

            }
        }

        if(selOnEntry)
        {
            selected = true;
            SelectFirstControl();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lastSelState == false && selected == true)
        {
            SelectFirstControl();
        }

        lastSelState = selected;
    }

    void SelectFirstControl()
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

            //Checks if there is one
            if (aCs.Count > 0)
            {
                //Sort by Order number
                aCs.Sort((p1, p2) => p1.order.CompareTo(p2.order));

                //Select first control
                aCs[0].SelectControl();
            }
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

    List<GameObject> GetAllControlChildrens()
    {
        List<GameObject> result = new List<GameObject>();

        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Control c = transform.GetChild(i).GetComponent<Control>();

                if (c != null)
                {
                    result.Add(transform.GetChild(i).gameObject);
                }
            }
        }

        return result;
    }
}
