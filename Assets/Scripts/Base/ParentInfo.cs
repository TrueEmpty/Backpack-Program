using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ParentInfo : MonoBehaviour
{
    public static ParentInfo instance;
    Database db;

    #region Parent Info
    public Parents parent;

    public Text title;

    public InputField pFN;
    public InputField pLN;
    public InputField pAd;
    public InputField pCi;
    public InputField pCo;
    public Dropdown pPu;

    public Color orgColor = Color.yellow;
    public Color newColor = Color.white;

    public WindowLoad wl;
    bool running = false;
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        db = Database.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(!running)
        {
            running = true;
            UpdateParentInfo();
        }

        if(title != null)
        {
            if (parent.UniqueId == "" || parent.UniqueId == null)
            {
                title.text = "New Parent/Guardian";
                SetInputColors(newColor);
            }
            else
            {
                title.text = "Parent/Guardian";
                SetInputColors(orgColor);
            }
        }
    }

    void UpdateParentInfo(bool ignoreChange = false, bool setToParent = false)
    {
        if (parent != null)
        {
            bool change = false;

            if (pFN != null)
            {
                if(parent.FirstName != pFN.text)
                {
                    change = true;
                }

                if (EventSystem.current.currentSelectedGameObject == pFN.gameObject && !setToParent)
                {
                    parent.FirstName = pFN.text;
                }
                else
                {
                    pFN.text = parent.FirstName;
                }
            }

            if (pLN != null)
            {
                if (parent.LastName != pLN.text)
                {
                    change = true;
                }

                if (EventSystem.current.currentSelectedGameObject == pLN.gameObject && !setToParent)
                {
                    parent.LastName = pLN.text;
                }
                else
                {
                    pLN.text = parent.LastName;
                }
            }

            if (pAd != null)
            {
                if (parent.LastName != pLN.text)
                {
                    change = true;
                }

                if (EventSystem.current.currentSelectedGameObject == pAd.gameObject && !setToParent)
                {
                    parent.Address = pAd.text;
                }
                else
                {
                    pAd.text = parent.Address;
                }
            }

            if (pCi != null)
            {
                if (parent.City != pCi.text)
                {
                    change = true;
                }

                if (EventSystem.current.currentSelectedGameObject == pCi.gameObject && !setToParent)
                {
                    parent.City = pCi.text;
                }
                else
                {
                    pCi.text = parent.City;
                }
            }

            if (pCo != null)
            {
                if (parent.Contact != pCo.text)
                {
                    change = true;
                }

                if (EventSystem.current.currentSelectedGameObject == pCo.gameObject && !setToParent)
                {
                    parent.Contact = pCo.text;
                }
                else
                {
                    pCo.text = parent.Contact;
                }
            }

            if (pPu != null)
            {
                //Set Pickup Time Field
                if (db.pickuptimes.Count > 0)
                {
                    if(db.pickuptimes.Count != pPu.options.Count)
                    {
                        //Add New PickupTimes
                        if (db.pickuptimes.Count > 0)
                        {
                            for (int i = 0; i < db.pickuptimes.Count; i++)
                            {
                                if (!pPu.options.Exists(x => x.text == db.pickuptimes[i].Times))
                                {
                                    pPu.options.Add(new Dropdown.OptionData(db.pickuptimes[i].Times));
                                }
                            }
                        }

                        //Remove Old PickupTimes
                        if (pPu.options.Count > 0)
                        {
                            for (int i = pPu.options.Count - 1; i >= 0; i--)
                            {
                                if (!db.pickuptimes.Exists(x => x.Times == pPu.options[i].text))
                                {
                                    pPu.options.RemoveAt(i);
                                }
                            }
                        }
                    }

                    if (pPu.options.Count > 0)
                    {
                        if (parent.UniqueId != null && parent.UniqueId != "")
                        {
                            Parents fP = db.parents.Find(x => x.UniqueId == parent.UniqueId);

                            if (fP != null)
                            {
                                int fppu = pPu.options.FindIndex(x => x.text.ToLower() == parent.PickupTime.ToLower());

                                if (fppu >= 0)
                                {
                                    pPu.value = fppu;

                                    if (pPu.options.Count > 0)
                                    {
                                        Text lc = pPu.gameObject.transform.Find("Label").GetComponent<Text>();

                                        if (lc != null)
                                        {
                                            lc.text = pPu.options[pPu.value].text;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if(pPu.options.Count > 0)
                {
                    if (parent.PickupTime != pPu.options[pPu.value].text)
                    {
                        change = true;
                    }

                    if (EventSystem.current.currentSelectedGameObject == pPu.gameObject && !setToParent)
                    {
                        parent.PickupTime = pPu.options[pPu.value].text;
                    }
                    else
                    {
                        int fppu = pPu.options.FindIndex(x => x.text.ToLower() == parent.PickupTime.ToLower());

                        if (fppu >= 0)
                        {
                            pPu.value = fppu;

                            if (pPu.options.Count > 0)
                            {
                                Text lc = pPu.gameObject.transform.Find("Label").GetComponent<Text>();

                                if (lc != null)
                                {
                                    lc.text = pPu.options[pPu.value].text;
                                }
                            }
                        }
                        else
                        {
                            pPu.value = 0;

                            if (pPu.options.Count > 0)
                            {
                                Text lc = pPu.gameObject.transform.Find("Label").GetComponent<Text>();

                                if (lc != null)
                                {
                                    lc.text = pPu.options[pPu.value].text;
                                }
                            }
                        }
                    }
                }
            }

            //If any of the data is changed Send out the parent info
            if(change && !ignoreChange)
            {
                SendParentInfo();
            }
        }

        running = false;
    }

    void SendParentInfo()
    {
        if (CheckValidity())
        {
            if (parent.UniqueId == "" || parent.UniqueId == null)
            {
                Parents newParent = new Parents();

                parent.UniqueId = newParent.UniqueId;
                newParent.OverwriteFields(parent);

                StartCoroutine(newParent.Save());
            }
            else
            {
                //Find Parent on the Database
                Parents fP = db.parents.Find(x => x.UniqueId == parent.UniqueId);

                if (fP != null)
                {
                    //Set Change to the parent
                    fP.UpdateRecord(parent);
                    fP.Save();
                }
            }
        }
    }

    void SetInputColors(Color c)
    {
        Image gI = null;

        if (pFN != null)
        {
            gI = pFN.GetComponent<Image>();

            if(gI != null)
            {
                gI.color = c;
            }

            gI = null;
        }

        if (pLN != null)
        {
            gI = pLN.GetComponent<Image>();

            if (gI != null)
            {
                gI.color = c;
            }

            gI = null;
        }

        if (pAd != null)
        {
            gI = pAd.GetComponent<Image>();

            if (gI != null)
            {
                gI.color = c;
            }

            gI = null;
        }

        if (pCi != null)
        {
            gI = pCi.GetComponent<Image>();

            if (gI != null)
            {
                gI.color = c;
            }

            gI = null;
        }

        if (pCo != null)
        {
            gI = pCo.GetComponent<Image>();

            if (gI != null)
            {
                gI.color = c;
            }

            gI = null;
        }

        if (pPu != null)
        {
            gI = pPu.GetComponent<Image>();

            if (gI != null)
            {
                gI.color = c;
            }

            gI = null;
        }
    }

    bool OnParentControl()
    {
        bool result = false;

        if (EventSystem.current.currentSelectedGameObject == pFN.gameObject)
        {
            result = true;
        }
        else if (EventSystem.current.currentSelectedGameObject == pLN.gameObject)
        {
            result = true;
        }
        else if (EventSystem.current.currentSelectedGameObject == pAd.gameObject)
        {
            result = true;
        }
        else if (EventSystem.current.currentSelectedGameObject == pCi.gameObject)
        {
            result = true;
        }
        else if (EventSystem.current.currentSelectedGameObject == pCo.gameObject)
        {
            result = true;
        }
        else if (EventSystem.current.currentSelectedGameObject == pPu.gameObject)
        {
            result = true;
        }

        return result;
    }

    bool CheckValidity()
    {
        bool result = true;

        if (parent.FirstName.Trim() == "" && parent.LastName.Trim() == "")
        {
            result = false;
        }

        if (parent.Address.Trim() == "")
        {
            result = false;
        }

        if (parent.City.Trim() == "")
        {
            result = false;
        }

        if (parent.Contact.Trim() == "")
        {
            result = false;
        }

        if (parent.PickupTime.Trim() == "")
        {
            result = false;
        }

        return result;
    }

    public void Remove()
    {
        if (parent.UniqueId != "" && parent.UniqueId != null)
        {
            parent.RemoveRecord();

            AddNew();
        }
    }

    public void SelectParent(Parents pa)
    {
        parent = pa;

        if (!running)
        {
            running = true;
            UpdateParentInfo(true, true);
        }
    }
    
    public void AddNew()
    {
        parent = new Parents();
        parent.UniqueId = "";

        if (!running)
        {
            running = true;
            UpdateParentInfo(true, true);
        }
    }
}
