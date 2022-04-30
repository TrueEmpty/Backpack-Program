using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewChildInfo : MonoBehaviour
{
    public Database db = null;
    ParentInfo pI;
    ConsoleManager cm;
    public AddNewChild anc;

    public Children child = null;

    public InputField ncID;
    public InputField ncFN;
    public InputField ncLN;
    public InputField ncAG;
    public Dropdown ncGe;
    public Dropdown ncSc;
    public Dropdown ncGr;

    public RectTransform rt;
    bool running = false;

    // Start is called before the first frame update
    void Start()
    {
        db = Database.instance;
        pI = ParentInfo.instance;
        cm = ConsoleManager.instance;

        ChangeControl();
    }

    void ChangeControl()
    {
        Control c = ncID.gameObject.GetComponent<Control>();

        if (c != null)
        {
              c.group = "New Child";
        }

        c = ncFN.gameObject.GetComponent<Control>();

        if (c != null)
        {
              c.group = "New Child";
        }

        c = ncLN.gameObject.GetComponent<Control>();

        if (c != null)
        {
              c.group = "New Child";
        }

        c = ncAG.gameObject.GetComponent<Control>();

        if (c != null)
        {
              c.group = "New Child";
        }

        c = ncGe.gameObject.GetComponent<Control>();

        if (c != null)
        {
              c.group = "New Child";
        }

        c = ncSc.gameObject.GetComponent<Control>();

        if (c != null)
        {
              c.group = "New Child";
        }

        c = ncGr.gameObject.GetComponent<Control>();

        if (c != null)
        {
            c.group = "New Child";
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckForParent();
    }

    public void CallUpdate()
    {
        if(CheckValidity() && Manager.instance.autoAddNewChild)
        {
            AddChild();
        }
    }

    void CheckForParent()
    {
        if (pI.parent.UniqueId == null || pI.parent.UniqueId == "" || child == null)
        {
            Destroy(gameObject);
        }
        else
        {

            if (child.ParentUID.Trim() != pI.parent.UniqueId.Trim())
            {
                Destroy(gameObject);
            }
        }
    }


    public void AddChild()
    {
        if(CheckValidity() && anc != null)
        {
            db.AddNew(child);
            anc.ResetChildList();
        }
    }

    bool CheckValidity()
    {
        bool result = true;

        if (child.Id.Trim() == "")
        {
            result = false;
        }

        if (child.FirstName.Trim() == "" && child.LastName.Trim() == "")
        {
            result = false;
        }

        if (child.ParentUID.Trim() == "")
        {
            result = false;
        }

        if (child.GetGender().Trim() == "")
        {
            result = false;
        }

        if (child.Age < 2)
        {
            result = false;
        }

        if (child.SchoolUID.Trim() == "")
        {
            result = false;
        }

        if (child.Grade.Trim() == "")
        {
            result = false;
        }

        return result;
    }
}
