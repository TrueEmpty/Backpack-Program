using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PickUpTimes
{
    //Identification
    public string UniqueId = ""; //Random series of NumbersAndLetters 10 char long
    public string Times = "";

    public System.DateTime lastUpdated = new System.DateTime(2000, 1, 1, 1, 1, 1, 1);

    public List<string> Data = new List<string>();

    public string Computer = "";

    public bool Sent = false;
    public bool Remove = false;

    //Will Generate A UniqueId and check it amoung the database to see if it is unique
    public string GenerateUniqueID(List<string> usedUIDs = null)
    {
        string avalableChars = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //add the characters you want

        rerun:
        string result = "";

        //Create UID
        for (int i = 0; i < 10; i++)
        {
            result += avalableChars.Substring(Random.Range(0, avalableChars.Length), 1);
        }

        //Check if already in use
        if (usedUIDs != null)
        {
            if (usedUIDs.Count > 0)
            {
                if (usedUIDs.Contains(result))
                {
                    goto rerun;
                }
            }
        }

        return result;
    }

    public PickUpTimes()
    {
        Database db = Database.instance;

        if (db != null)
        {
            List<PickUpTimes> pu = db.pickuptimes;
            List<string> uuids = null;

            if (pu != null)
            {
                if (pu.Count > 0)
                {
                    uuids = new List<string>();

                    for (int i = 0; i < pu.Count; i++)
                    {
                        uuids.Add(pu[i].UniqueId);
                    }
                }
            }

            UniqueId = "Pu_" + GenerateUniqueID(uuids);

            lastUpdated = System.DateTime.Now;
        }
    }

    public PickUpTimes(PickUpTimes pickupTimes)
    {
        UniqueId = pickupTimes.UniqueId;
        Times = pickupTimes.Times;

        lastUpdated = pickupTimes.lastUpdated;
        Computer = pickupTimes.Computer;
        Sent = pickupTimes.Sent;
        Remove = pickupTimes.Remove;
    }

    public PickUpTimes(List<string> dataList)
    {
        if (dataList.Count > 0)
        {
            Decode(dataList[0]);
        }

        Data = dataList;
    }

    public PickUpTimes(string dataString)
    {
        Decode(dataString);
    }

    public void Decode(List<string> dataList)
    {
        string dataString = "";

        if (dataList.Count > 0)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                if (i > 0)
                {
                    dataString += "\n";
                }

                dataString += dataList[0];
            }

            Decode(dataString);
        }
    }

    public void Decode(string dataString)
    {
        string[] lsp = dataString.Split('\n');

        string[] bsp = lsp[0].Split(';');
        //Get last updated
        lastUpdated = System.DateTime.Parse(bsp[0]);

        string[] sp = bsp[1].Split('|');

        UniqueId = sp[0];
        Times = sp[1];
        Computer = sp[2];

        if (sp.Length > 3)
        {
            Sent = bool.Parse(sp[3]);
        }

        if (sp.Length > 4)
        {
            Remove = bool.Parse(sp[4]);
        }

        Data.Clear();

        for (int i = 0; i < lsp.Length; i++)
        {
            Data.Add(lsp[i]);
        }
    }

    public override string ToString()
    {
        string result = "";

        result += lastUpdated.ToString();
        result += ";";

        result += UniqueId;
        result += "|";
        result += Times;
        result += "|";
        result += Computer;
        result += "|";
        result += Sent.ToString();
        result += "|";
        result += Remove.ToString();

        return result;
    }

    //Look for file and record if not found creates a new one if found check if it is up to date and save new if not
    public IEnumerator Save(string location = "PickUpTimes", bool SendNetData = true, string removeLoc = "")
    {
        Database db = Database.instance;
        //Set update info
        lastUpdated = System.DateTime.Now;
        Sent = false;
        Computer = db.computer.text;

        PickUpTimes p = db.pickuptimes.Find(x => x.UniqueId == this.UniqueId);

        //Check if this exist in the database
        if (p != null)
        {
            //Save old data
            p.WriteData();

            //Overwrite Current Data
            p.OverwriteFields(this, false, true);

            //Grab full data and store
            Data = p.Data;
        }
        else
        {
            WriteData();
            db.pickuptimes.Add(this);
        }

        //Check if there is a file made for him
        TextFileManager.instance.SaveRecord("Database/" + location, UniqueId, lastUpdated, DataString(), "Database/" + removeLoc);

        //Send on Network
        if (SendNetData)
        {
            db.SendData(DataString());
        }

        yield return new WaitForSecondsRealtime(.01f);
    }

    public void WriteData()
    {
        Data.Reverse();
        Data.Add(ToString());
        Data.Reverse();
    }

    public void OverwriteFields(PickUpTimes pickupTimes, bool includeOverwriteAllData = false,bool voidRemove = false)
    {
        UniqueId = pickupTimes.UniqueId;
        Times = pickupTimes.Times;

        lastUpdated = pickupTimes.lastUpdated;
        Computer = pickupTimes.Computer;
        Sent = pickupTimes.Sent;

        if (includeOverwriteAllData)
        {
            Data = pickupTimes.Data;
        }

        Remove = pickupTimes.Remove;

        if (pickupTimes.Remove && !voidRemove)
        {
            //Remove from normal database
            RemoveRecord();
        }
    }

    public string DataString()
    {
        string result = null;

        if (Data.Count > 0)
        {
            result = "";

            for (int i = 0; i < Data.Count; i++)
            {
                if (i > 0)
                {
                    result += "\n";
                }

                result += Data[i];
            }
        }

        return result;
    }

    public void RemoveRecord()
    {
        Database db = Database.instance;
        //Set remove
        Remove = true;

        //Save to removed
        Save("Removed",true, "PickUpTimes");

        //Remove from normal database
        db.pickuptimes.RemoveAll(x => x.UniqueId == UniqueId);
    }
}
