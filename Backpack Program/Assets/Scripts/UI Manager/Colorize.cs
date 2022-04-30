using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Colorize : MonoBehaviour
{
    public List<Transform> followers = new List<Transform>();

    public Color Normal = new Color();
    public Color Highlighted = new Color();
    public Color Pressed = new Color();
    public Color Disabled = new Color();

    public bool disabled = false;

    public bool isTrigger = false;

    public bool highlighted, pressed, clicked;
    public bool hasLeader = false;

    Image im = null;
    Text te = null;
    
    //Used in editor
    public string showWhatColor = "";
    public bool showColors = true;

    public Camera camUsed;

    // Start is called before the first frame update
    void Start()
    {
        im = GetComponent<Image>();
        te = GetComponent<Text>();

        if(followers.Count > 0)
        {
            foreach(Transform ftr in followers)
            {
                ftr.gameObject.GetComponent<Colorize>().hasLeader = true;
            }
        }

        if(camUsed == null)
        {
            camUsed = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //State Update
        ResetState();

        if(!hasLeader)
        { 
            //Check for state
            UpdateState();

            if(followers.Count > 0)
            {
                for(int i = 0; i < followers.Count; i++)
                {
                    Colorize co = followers[i].GetComponent<Colorize>();

                    co.highlighted = highlighted;
                    co.pressed = pressed;
                    co.clicked = clicked;
                    co.disabled = disabled;
                    co.isTrigger = isTrigger;
                }
            }
        }

        //Visual Update
        if (im == null)
        {
            if (te == null)
            {

            }
            else
            {
                VisualUpdateText();
            }
        }
        else
        {
            VisualUpdateImage();
        }
    }

    void VisualUpdateImage()
    {
        if(!disabled)
        {
            if(pressed || clicked)
            {
                im.color = Pressed;
            }
            else if(highlighted)
            {
                im.color = Highlighted;
            }
            else
            {
                im.color = Normal;
            }
        }
        else
        {
            im.color = Disabled;
        }
    }

    void VisualUpdateText()
    {
        if (!disabled)
        {
            if (pressed || clicked)
            {
                te.color = Pressed;
            }
            else if (highlighted)
            {
                te.color = Highlighted;
            }
            else
            {
                te.color = Normal;
            }
        }
        else
        {
            te.color = Disabled;
        }
    }

    void UpdateState()
    {
        if(!disabled)
        {
            RaycastHit hit;
            Ray ray = camUsed.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)//Highlighted or pressed
                {
                    if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                    {
                        pressed = true;
                    }
                    else if (pressed && (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))) //Clicked
                    {
                        if(isTrigger)
                        {
                            clicked = !clicked;
                        }
                        else
                        {
                            clicked = true;
                        }
                    }
                    else if (pressed && (Input.GetMouseButton(0) || Input.GetMouseButton(1))) //Clicked
                    {
                        pressed = true;
                    }
                    else //highlighted
                    {
                        highlighted = true;
                        pressed = false;
                    }
                }
            }
            else
            {
                pressed = false;
            }
        }
        else
        {
            pressed = false;
        }
    }

    void ResetState()
    {
        highlighted = false;

        if (!isTrigger)
        {
            clicked = false;
        }
    }
}
