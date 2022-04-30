using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ChildInfo : MonoBehaviour
{
    public Database db = null;
    ParentInfo pI;
    ConsoleManager cm;
    public Children child = null;

    public InputField ncID;
    public InputField ncFN;
    public InputField ncLN;
    public InputField ncAG;
    public Dropdown ncGe;
    public Dropdown ncSc;
    public Dropdown ncGr;

    public RectTransform rt;
    bool change = false;
    bool ignoreChange = false;
    bool setToChild = false;
    float firstSend = 10;
    bool sendit = false;
    bool sending = false;
    string lastSchool = "";

    // Start is called before the first frame update
    void Start()
    {
        db = Database.instance;
        pI = ParentInfo.instance;
        cm = ConsoleManager.instance;
        ChangeControl();
        firstSend = Time.time + 5;
    }

    void ChangeControl()
    {
        Control c = ncID.gameObject.GetComponent<Control>();

        if (c != null)
        {
            c.group = "Child: " + child.GetName();
        }

        c = ncFN.gameObject.GetComponent<Control>();

        if (c != null)
        {
            c.group = "Child: " + child.GetName();
        }

        c = ncLN.gameObject.GetComponent<Control>();

        if (c != null)
        {
            c.group = "Child: " + child.GetName();
        }

        c = ncAG.gameObject.GetComponent<Control>();

        if (c != null)
        {
            c.group = "Child: " + child.GetName();
        }

        c = ncGe.gameObject.GetComponent<Control>();

        if (c != null)
        {
            c.group = "Child: " + child.GetName();
        }

        c = ncSc.gameObject.GetComponent<Control>();

        if (c != null)
        {
            c.group = "Child: " + child.GetName();
        }

        c = ncGr.gameObject.GetComponent<Control>();

        if (c != null)
        {
            c.group = "Child: " + child.GetName();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckForParent();

        SetSchoolOptions();

        if (lastSchool != child.SchoolUID && child.SchoolUID != "" && child.SchoolUID != null)
        {
            SetGradeOptions();
            lastSchool = child.SchoolUID;
        }
    }

    public void SetChildInfo(Children ch)
    {
        child = ch;
        db = Database.instance;

        if (ncID != null)
        {
            ncID.text = child.Id;
        }

        if (ncFN != null)
        {
            ncFN.text = child.FirstName;
        }

        if (ncLN != null)
        {
            ncLN.text = child.LastName;
        }

        if (ncAG != null)
        {
            ncAG.text = child.Age.ToString();
        }

        if (ncGe != null && ncGe.options.Count > 0)
        {
            int genOp = ncGe.options.FindIndex(x => x.text == child.GetGender());

            if (genOp >= 0 && genOp < ncGe.options.Count)
            {
                ncGe.value = genOp;
            }
        }

    }

    public void UpdateID()
    {
        if (ncID != null)
        {
            child.Id = ncID.text;
        }
    }    
    
    public void UpdateFirstName()
    {
        if (ncFN != null)
        {
            child.FirstName = ncFN.text;
        }
    }

    public void UpdateLastName()
    {
        if (ncLN != null)
        {
            child.LastName = ncLN.text;
        }
    }

    public void UpdateAge()
    {
        if (ncAG != null)
        {
            child.Age = int.Parse(ncAG.text);
        }
    }

    public void UpdateGender()
    {
        if (ncGe != null && ncGe.options.Count > 0)
        {
            string genVal = ncGe.options[ncGe.value].text;
            child.ToGender(genVal);
        }
    }

    public void UpdateSchool()
    {
        if(ncSc != null && ncSc.options.Count > 0)
        {

        }
    }

    public void UpdateGrade()
    {

    }

    void SetSchoolOptions()
    {
        if(db.schools.Count > 0)
        {
            List<Dropdown.OptionData> sData = new List<Dropdown.OptionData>();

            for(int i = 0; i < db.schools.Count; i++)
            {
                sData.Add(new Dropdown.OptionData(db.schools[i].SchoolName));
            }

            if(ncSc != null)
            {
                string sVal = null;
                
                if(ncSc.options.Count > 0)
                {
                    sVal = ncSc.options[ncSc.value].text;
                }

                ncSc.options.Clear();

                if(sData.Count > 0)
                {
                    ncSc.options = sData;

                    int sInd = ncSc.options.FindIndex(x => x.text == sVal);

                    if(sInd > -1 && sInd < ncSc.options.Count)
                    {
                        ncSc.value = sInd;
                    }
                    else
                    {
                        ncSc.value = 0;
                    }
                }
            }
        }
    }

    void SetGradeOptions()
    {
        
    }

    void SendChildInfo()
    {
        if (CheckValidity())
        {
            if (child.UniqueId != "" && child.UniqueId != null && child.ParentUID != "" && child.ParentUID != null)
            {
                //Find Child on the Database
                Children fC = db.children.Find(x => x.UniqueId == child.UniqueId);

                if (fC != null)
                {
                    //Set Change to the parent
                    fC.UpdateRecord(child);
                    fC.Save();
                    sendit = false;
                    sending = false;
                }
            }
        }
    }

    bool OnChildControl()
    {
        bool result = false;

        if (EventSystem.current.currentSelectedGameObject == ncID.gameObject)
        {
            result = true;
        }
        else if (EventSystem.current.currentSelectedGameObject == ncFN.gameObject)
        {
            result = true;
        }
        else if (EventSystem.current.currentSelectedGameObject == ncLN.gameObject)
        {
            result = true;
        }
        else if (EventSystem.current.currentSelectedGameObject == ncAG.gameObject)
        {
            result = true;
        }
        else if (EventSystem.current.currentSelectedGameObject == ncGe.gameObject)
        {
            result = true;
        }
        else if (EventSystem.current.currentSelectedGameObject == ncSc.gameObject)
        {
            result = true;
        }
        else if (EventSystem.current.currentSelectedGameObject == ncGr.gameObject)
        {
            result = true;
        }

        return result;
    }

    public void RemoveChild()
    {
        cm.Write("Removed Child : " + child.GetName());
        child.RemoveRecord();

        if (!IsInvoking("DestroyGO"))
        {
            Invoke("DestroyGO", .1f);
        }
    }

    public void PrintChild()
    {
        cm.Write("Printing Child : " + child.GetName());

        PrintInfo.instance.PrintCurrentChildren(child);
    }

    void CheckForParent()
    {
        if (pI.parent.UniqueId == null || pI.parent.UniqueId == "" || child == null)
        {
            //If any of the data is changed Send out the parent info
            if(!IsInvoking("DestroyGO"))
            {
                Invoke("DestroyGO", .1f);
            }
        }
        else
        {

            if (child.ParentUID.Trim() != pI.parent.UniqueId.Trim() || child.Remove)
            {
                if (!IsInvoking("DestroyGO"))
                {
                    Invoke("DestroyGO", .1f);
                }
            }
        }
    }

    void DestroyGO()
    {
        Destroy(gameObject);
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
