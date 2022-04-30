using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    public bool RunSavesOnClose = true;

    public bool active = false;
    public bool CloseOnNonWindowClick = false;

    public bool CloseOnDeactivate = true;

    public GameObject Active_Deactive_Shading;

    public bool auto_group = false;
    public string innerGroup = "";

    public bool selOnEntry = true;

    public int currentChildCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        active = true;

        if (selOnEntry)
        {
            Control c = GetComponent<Control>();

            if (c != null)
            {
                c.MoveControlItem(1);
            }
        }
    }

    public void Deactivate(bool deactivate)
    {
        if(deactivate)
        {
            active = false;
        }
        else
        {
            active = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetupBoxCollider();
        ShowActive();
    }

    void SetupBoxCollider()
    {
        RectTransform rt = GetComponent<RectTransform>();
        BoxCollider bc = GetComponent<BoxCollider>();

        if(bc == null)
        {
            gameObject.AddComponent(typeof(BoxCollider));
            bc = GetComponent<BoxCollider>();
        }

        if (rt != null && bc != null)
        {
            bc.size = new Vector3(rt.rect.width, rt.rect.height, 1);
        }
    }

    public bool MouseClickInsideWindow()
    {
        bool result = false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit))
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                result = true;
            }
        }

        return result;
    }

    public void CloseWindow()
    {
        if(RunSavesOnClose)
        {
            //Send Message to itself and all of its children
            gameObject.BroadcastMessage("OnClose", SendMessageOptions.DontRequireReceiver);
        }

        Invoke("CloseIt", .5f);
    }

    void ShowActive()
    {
        if (Active_Deactive_Shading != null)
        {
            Active_Deactive_Shading.SetActive(!active);
        }

        if (active)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                if (!MouseClickInsideWindow() && CloseOnNonWindowClick)
                {
                    active = false;

                    if (CloseOnDeactivate)
                    {
                        CloseWindow();
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                if (MouseClickInsideWindow())
                {
                    active = true;

                    if(selOnEntry)
                    {
                        Control c = GetComponent<Control>();

                        if (c != null)
                        {
                            c.MoveControlItem(1);
                        }
                    }
                }
            }
        }
    }

    void CloseIt()
    {
        Destroy(gameObject);
    }
}
