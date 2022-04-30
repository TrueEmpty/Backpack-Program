using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Database : MonoBehaviour
{
    public static Database instance;
    TextFileManager tfm;
    public DropboxManager dm;

    [SerializeField]
    public List<Parents> parents = new List<Parents>();

    [SerializeField]
    public List<Children> children = new List<Children>();

    [SerializeField]
    public List<Schools> schools = new List<Schools>();

    [SerializeField]
    public List<PickUpTimes> pickuptimes = new List<PickUpTimes>();

    public InputField computer;
    public int printAmount = 2;

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
        computer.text = PlayerPrefs.GetString("Comp");
        printAmount = int.Parse(PlayerPrefs.GetString("PrintAmount"));
        tfm = TextFileManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
    }

    #region Add New
    public void AddNew(Parents parent)
    {
        Parents r = parents.Find(x => x.UniqueId == parent.UniqueId);

        if (r == null)
        {
            parents.Add(parent);
        }
        else
        {
            if (r.lastUpdated < parent.lastUpdated)
            {
                r = parent;
            }
        }
    }

    public void AddNew(Schools school)
    {
        Schools r = schools.Find(x => x.UniqueId == school.UniqueId);
        
        if (r == null)
        {
            schools.Add(school);
        }
        else
        {
            if(r.lastUpdated < school.lastUpdated)
            {
                r = school;
            }
        }    
    }

    public void AddNew(PickUpTimes pickupTime)
    {
        PickUpTimes r = pickuptimes.Find(x => x.UniqueId == pickupTime.UniqueId);

        if (r == null)
        {
            pickuptimes.Add(pickupTime);
        }
        else
        {
            if (r.lastUpdated < pickupTime.lastUpdated)
            {
                r = pickupTime;
            }
        }
    }

    public void AddNew(Children child)
    {
        Children r = children.Find(x => x.UniqueId == child.UniqueId);

        if (r == null)
        {
            children.Add(child);
        }
        else
        {
            if (r.lastUpdated < child.lastUpdated)
            {
                r = child;
            }
        }
    }
    #endregion

    #region GetInfo

    public List<Children> GetParentsChildren(string parentUID)
    {
        List<Children> result = new List<Children>();

        result = children.FindAll(x => x.ParentUID == parentUID);

        return result;
    }

    public List<Parents> GetAllParents(string searchbarContent)
    {
        List<Parents> result = new List<Parents>();

        if (parents.Count > 0)
        {
            if (searchbarContent.Trim() == "")
            {
                result = parents;
            }
            else
            {
                result = parents.FindAll(x => x.ToString().ToLower().Contains(searchbarContent.Trim().ToLower()));
            }

            result.Sort((p1, p2) => p1.GetFullName().CompareTo(p2.GetFullName()));
        }

        return result;
    }

    #endregion

    #region Network

    public void SendData(string msg)
    {
        if(dm.IsMaster() == 1)
        {
            ServerSend.DataTransfer(msg);
        }
        else if(dm.IsMaster() == 0)
        {
            ClientSend.DataTransfer(msg);
        }
    }

    public void SendAllDirty()
    {
        List<string> sends = new List<string>();

        //Parents
        List<Parents> pSend = parents.FindAll(x => x.Sent = false);

        if(pSend.Count > 0)
        {
            for(int i = 0; i < pSend.Count; i++)
            {
                //If Master set to sent
                if (dm.IsMaster() == 1)
                {
                    pSend[i].Sent = true;
                }

                sends.Add(pSend[i].ToString());
            }
        }

        //Schools
        List<Schools> sSend = schools.FindAll(x => x.Sent = false);

        if (sSend.Count > 0)
        {
            for (int i = 0; i < sSend.Count; i++)
            {
                //If Master set to sent
                if (dm.IsMaster() == 1)
                {
                    sSend[i].Sent = true;
                }

                sends.Add(sSend[i].ToString());
            }
        }

        //Children
        List<Children> cSend = children.FindAll(x => x.Sent = false);

        if (cSend.Count > 0)
        {
            for (int i = 0; i < cSend.Count; i++)
            {
                //If Master set to sent
                if (dm.IsMaster() == 1)
                {
                    cSend[i].Sent = true;
                }

                sends.Add(cSend[i].ToString());
            }
        }

        //Pickup Times
        List<PickUpTimes> tSend = pickuptimes.FindAll(x => x.Sent = false);

        if (tSend.Count > 0)
        {
            for (int i = 0; i < tSend.Count; i++)
            {
                //If Master set to sent
                if (dm.IsMaster() == 1)
                {
                    tSend[i].Sent = true;
                }

                sends.Add(tSend[i].ToString());
            }
        }

        //Send it out
        if (sends.Count > 0)
        {
            for(int i = 0; i < sends.Count; i++)
            {
                SendData(sends[i]);
            }
        }
    }

    public void IncomingRecord(string msg)
    {
        ConsoleManager cm = ConsoleManager.instance;

        //Get UID
        string uid = GetUID(msg);
        
        //Check UID for type
        switch(uid.Substring(0,3))
        {
            case "Pa_":
                Parents nRp = new Parents(msg);
                Parents fRp = parents.Find(x => x.UniqueId == nRp.UniqueId);

                if(fRp != null)
                {
                    if(fRp.lastUpdated < nRp.lastUpdated)
                    {
                        fRp.OverwriteFields(nRp, true);
                    }
                }
                else
                {
                    //nRp.Save(SendNetData: false);
                }                
                break;            
            case "Sc_":
                Schools nRs = new Schools(msg);
                Schools fRs = schools.Find(x => x.UniqueId == nRs.UniqueId);

                if (fRs != null)
                {
                    if (fRs.lastUpdated < nRs.lastUpdated)
                    {
                        fRs.OverwriteFields(nRs, true);
                    }
                }
                else
                {
                    //nRs.Save(SendNetData: false);
                }
                break;          
            case "Pu_":
                PickUpTimes nRt = new PickUpTimes(msg);
                PickUpTimes fRt = pickuptimes.Find(x => x.UniqueId == nRt.UniqueId);

                if (fRt != null)
                {
                    if (fRt.lastUpdated < nRt.lastUpdated)
                    {
                        fRt.OverwriteFields(nRt, true);
                    }
                }
                else
                {
                    //nRt.Save(SendNetData: false);
                }
                break;          
            case "Ch_":
                Children nRc = new Children(msg);
                Children fRc = children.Find(x => x.UniqueId == nRc.UniqueId);

                if (fRc != null)
                {
                    if (fRc.lastUpdated < nRc.lastUpdated)
                    {
                        fRc.OverwriteFields(nRc, true);
                    }
                }
                else
                {
                    //nRc.Save(SendNetData: false);
                }
                break;
            default:
                cm.Write("Unknown Record came in [" + uid + "] could not add to db");
                break;
        }

        if (dm.IsMaster() == 1)
        {
            //Send it out to all clients
            //ServerSend.DataTransfer(msg);
        }
    }
    #endregion

    string GetUID(string msg)
    {
        string result = null;

        string[] dra = msg.Split('\n');

        if(dra.Length > 0)
        {
            string[] bsp = dra[0].Split(';');
            string[] sp = bsp[1].Split('|');

            result = sp[0];
        }

        return result;
    }


    public void CloseApp()
    {
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("Comp", computer.text);
        PlayerPrefs.SetString("PrintAmount", printAmount.ToString());

        //Send out Close
    }
}
