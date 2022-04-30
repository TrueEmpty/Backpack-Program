using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Parents
{
    //Identification
    public string UniqueId = ""; //Random series of NumbersAndLetters 10 char long
    public string FirstName = "";
    public string LastName = "";

    public string Address = "";
    public string City = "";
    public string Contact = "";
    public string PickupTime = "";

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

    public Parents()
    {
        Database db = Database.instance;

        if (db != null)
        {
            List<Parents> pl = db.parents;
            List<string> uuids = null;

            if (pl != null)
            {
                if (pl.Count > 0)
                {
                    uuids = new List<string>();

                    for (int i = 0; i < pl.Count; i++)
                    {
                        uuids.Add(pl[i].UniqueId);
                    }
                }
            }

            UniqueId = "Pa_" + GenerateUniqueID(uuids);

            lastUpdated = System.DateTime.Now;
        }
    }

    public Parents(Parents parent)
    {
        UniqueId = parent.UniqueId;
        FirstName = parent.FirstName;
        LastName = parent.LastName;

        Address = parent.Address;
        City = parent.City;
        Contact = parent.Contact;

        PickupTime = parent.PickupTime;

        lastUpdated = parent.lastUpdated;
        Computer = parent.Computer;
        Sent = parent.Sent;
        Remove = parent.Remove;
    }

    public Parents(List<string> dataList)
    {
        if(dataList.Count > 0)
        {
            Decode(dataList[0]);
        }

        Data = dataList;
    }

    public Parents(string dataString)
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
        FirstName = sp[1];
        LastName = sp[2];
        Address = sp[3];
        City = sp[4];
        Contact = sp[5];
        PickupTime = sp[6];
        Computer = sp[7];

        if (sp.Length > 8)
        {
            Sent = bool.Parse(sp[8]);
        }

        if (sp.Length > 9)
        {
            Remove = bool.Parse(sp[9]);
        }

        Data.Clear();

        for(int i = 0; i < lsp.Length; i++)
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
        result += FirstName.Replace(",", "");
        result += "|";
        result += LastName.Replace(",", "");
        result += "|";
        result += Address.Replace(",", "");
        result += "|";
        result += City.Replace(",", "");
        result += "|";
        result += Contact;
        result += "|";
        result += PickupTime;
        result += "|";
        result += Computer;
        result += "|";
        result += Sent.ToString();
        result += "|";
        result += Remove.ToString();

        return result;
    }

    public string GetFullName()
    {
        string result = FirstName + " " + LastName;
        return result;
    }

    //Look for file and record if not found creates a new one if found check if it is up to date and save new if not
    public IEnumerator Save(string location = "Parents", bool SendNetData = true, string removeLoc = "")
    {
        Database db = Database.instance;
        //Set update info
        lastUpdated = System.DateTime.Now;
        Sent = false;
        Computer = db.computer.text;

        Parents p = db.parents.Find(x => x.UniqueId == this.UniqueId);

        //Check if this exist in the database
        if (p != null)
        {
            //Save old data
            p.WriteData();

            //Overwrite Current Data
            p.OverwriteFields(this,false,true);

            //Grab full data and store
            Data = p.Data;
        }
        else
        {
            WriteData();
            db.parents.Add(this);
        }

        //Check if there is a file made for him
        TextFileManager.instance.SaveRecord("Database/" + location, UniqueId,lastUpdated,DataString(), "Database/" + removeLoc);

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

    public void OverwriteFields(Parents parent,bool includeOverwriteAllData = false, bool voidRemove = false)
    {
        FirstName = parent.FirstName;
        LastName = parent.LastName;

        Address = parent.Address;
        City = parent.City;
        Contact = parent.Contact;

        PickupTime = parent.PickupTime;

        lastUpdated = parent.lastUpdated;
        Computer = parent.Computer;
        Sent = parent.Sent;

        if(includeOverwriteAllData)
        {
            Data = parent.Data;
        }

        Remove = parent.Remove;

        if (parent.Remove && !voidRemove)
        {
            //Remove from normal database
            RemoveRecord();
        }
    }

    public string DataString()
    {
        string result = null;

        if(Data.Count > 0)
        {
            result = "";

            for(int i = 0; i < Data.Count; i++)
            {
                if(i > 0)
                {
                    result += "\n";
                }

                result += Data[i];
            }
        }

        return result;
    }

    public void UpdateRecord(Parents parent)
    {
        if(UniqueId == parent.UniqueId)
        {
            Database db = Database.instance;

            Data.Add(DataString());

            FirstName = parent.FirstName;
            LastName = parent.LastName;

            Address = parent.Address;
            City = parent.City;
            Contact = parent.Contact;

            PickupTime = parent.PickupTime;

            lastUpdated = System.DateTime.Now;
            Computer = db.computer.text;
            Sent = false;

            Remove = false;
        }
    }

    public void RemoveRecord()
    {
        Database db = Database.instance;
        //Set remove
        Remove = true;

        //Save to removed
        Save("Removed", true, "Parents");

        //Remove from normal database
        db.parents.RemoveAll(x => x.UniqueId == UniqueId);
    }
}
