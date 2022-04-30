using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Children
{
    //Identification
    public string UniqueId = ""; //Random series of NumbersAndLetters 10 char long

    public string Id = "";

    public string FirstName = "";
    public string LastName = "";

    public bool Male = false;
    public System.DateTime Birthday = new System.DateTime(2000, 1, 1, 1, 1, 1, 1);

    public string SchoolUID = "";
    public string Grade = "";

    public string ParentUID = "";

    public string Computer = "";

    public System.DateTime lastUpdated = new System.DateTime(2000, 1, 1, 1, 1, 1, 1);

    public List<string> Data = new List<string>();

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

    public Children()
    {
        Database db = Database.instance;

        if (db != null)
        {
            List<Children> ch = db.children;
            List<string> uuids = null;

            if (ch != null)
            {
                if (ch.Count > 0)
                {
                    uuids = new List<string>();

                    for (int i = 0; i < ch.Count; i++)
                    {
                        uuids.Add(ch[i].UniqueId);
                    }
                }
            }

            UniqueId = "Ch_" + GenerateUniqueID(uuids);

            lastUpdated = System.DateTime.Now;
            Computer = db.computer.text;
        }
    }

    public Children(Children children)
    {
        Id = children.Id;
        UniqueId = children.UniqueId;
        FirstName = children.FirstName;
        LastName = children.LastName;

        Male = children.Male;
        Birthday = children.Birthday;

        SchoolUID = children.SchoolUID;

        Grade = children.Grade;

        ParentUID = children.ParentUID;

        lastUpdated = children.lastUpdated;

        Computer = children.Computer;

        Sent = children.Sent;

        Remove = children.Remove;
    }

    public Children(List<string> dataList)
    {
        if (dataList.Count > 0)
        {
            Decode(dataList[0]);
        }

        Data = dataList;
    }

    public Children(string dataString)
    {
        Decode(dataString);
    }

    public string GetPrint(Parents parent, Schools school)
    {
        string result = "";

        result += "#" + Id + "  Pickup:" + parent.PickupTime;
        result += ",";
        result += "Name:" + FirstName + " " + LastName;
        result += ",";
        result += "Gen:" + GetGender() + "  Grade:" + Grade + "  Age:" + Age.ToString();
        result += ",";
        result += "School:" + school.SchoolName;
        result += ",";
        result += "Contact:" + parent.Contact;

        return result;
    }

    public string GetAllDataCSV(Parents parent, Schools school)
    {
        string result = "";

        result += lastUpdated.ToString();
        result += ",";

        result += UniqueId;
        result += ",";
        result += Id;
        result += ",";
        result += FirstName.Replace(",", "");
        result += ",";
        result += LastName.Replace(",", "");
        result += ",";
        result += GetGender();
        result += ",";
        result += Age.ToString();
        result += ",";
        result += SchoolUID;
        result += ",";
        result += school.SchoolName.Replace(",", "");
        result += ",";
        result += Grade;
        result += ",";
        result += ParentUID;
        result += ",";
        result += parent.FirstName.Replace(",", "");
        result += ",";
        result += parent.LastName.Replace(",", "");
        result += ",";
        result += parent.Address.Replace(",", "");
        result += ",";
        result += parent.City.Replace(",", "");
        result += ",";
        result += parent.Contact;
        result += ",";
        result += parent.PickupTime;
        result += ",";
        result += Computer;

        return result;
    }

    public string GetCSVHeader()
    {
        string result = "";

        result += "Last Updated";
        result += ",";

        result += "Child Unique Id";
        result += ",";
        result += "Printed Id";
        result += ",";
        result += "First Name";
        result += ",";
        result += "Last Name";
        result += ",";
        result += "Gender";
        result += ",";
        result += "Age";
        result += ",";
        result += "School Unique Id";
        result += ",";
        result += "School Name";
        result += ",";
        result += "Grade";
        result += ",";
        result += "Parent Unique Id";
        result += ",";
        result += "Parent First Name";
        result += ",";
        result += "Parent Last Name";
        result += ",";
        result += "Parent Address";
        result += ",";
        result += "Parent City";
        result += ",";
        result += "Parent Contact";
        result += ",";
        result += "Parent Pickup Time";
        result += ",";
        result += "Child Input Computer Number";

        return result;
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
        Id = sp[1];
        FirstName = sp[2];
        LastName = sp[3];
        Male = bool.Parse(sp[4]);
        Birthday = System.DateTime.Parse(sp[5]);
        SchoolUID = sp[6];
        Grade = sp[7];
        ParentUID = sp[8];
        Computer = sp[9];

        if (sp.Length > 10)
        {
            Sent = bool.Parse(sp[10]);
        }

        if (sp.Length > 11)
        {
            Remove = bool.Parse(sp[11]);
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
        result += Id;
        result += "|";
        result += FirstName.Replace(",", "");
        result += "|";
        result += LastName.Replace(",", "");
        result += "|";
        result += Male.ToString();
        result += "|";
        result += Birthday.ToString();
        result += "|";
        result += SchoolUID;
        result += "|";
        result += Grade;
        result += "|";
        result += ParentUID;
        result += "|";
        result += Computer;
        result += "|";
        result += Sent.ToString();
        result += "|";
        result += Remove.ToString();

        return result;
    }


    #region Grab info

    public string GetName(bool initals = false)
    {
        string result = "";

        if (initals)
        {
            result = GetInitals();
        }
        else
        {
            result = FirstName + " " + LastName;
        }

        return result;
    }

    public int Age
    {
        get
        {
            int result = 0;

            System.DateTime today = System.DateTime.Now;

            //Get normal age
            result = today.Year - Birthday.Year;

            if (Birthday.Month > today.Month)
            {
                //If this is true minus 1 year because they havent made it past their birthday yet
                result -= 1;

            }
            else if (Birthday.Month == today.Month)
            {
                //check the day
                if (Birthday.Day > today.Day)
                {
                    result -= 1;
                }
            }

            return result;
        }

        set
        {
            System.DateTime today = System.DateTime.Now;

            Birthday = new System.DateTime(today.Year - value,1,1);
        }
    }

    public string GetInitals()
    {
        string result = "";

        string fN = "";
        string lN = "";

        if (FirstName.Length >= 1)
        {
            fN = FirstName.Substring(0, 1).ToUpper();
        }

        if (LastName.Length >= 1)
        {
            lN = LastName.Substring(0, 1).ToUpper();
        }

        result = fN + lN;

        return result;
    }

    public string GetGender()
    {
        if(Male)
        {
            return "Male";
        }
        else
        {
            return "Female";
        }
    }

    public void ToGender(string gender)
    {
        if(gender.ToUpper().Trim() == "MALE")
        {
            Male = true;
        }
        else if(gender.ToUpper().Trim() == "FEMALE")
        {
            Male = false;
        }
        else
        {
            Debug.Log("Unknown Gender Assigned as Male (" + gender + ")");
            Male = true;
        }
    }

    #endregion


    //Look for file and record if not found creates a new one if found check if it is up to date and save new if not
    public void Save(string location = "Children", bool SendNetData = true, string removeLoc = "")
    {
        Database db = Database.instance;
        //Set update info
        lastUpdated = System.DateTime.Now;
        Sent = false;
        Computer = db.computer.text;

        Children c = db.children.Find(x => x.UniqueId == UniqueId);

        //Check if this exist in the database
        if (c != null)
        {
            //Save old data
            c.WriteData();

            //Overwrite Current Fields
            c.OverwriteFields(this, false, true);

            //Grab full data and store
            Data = c.Data;
        }
        else
        {
            WriteData();
            db.children.Add(this);
        }

        //Check if there is a file made for him
        TextFileManager.instance.SaveRecord("Database/" + location, UniqueId, lastUpdated, DataString(), "Database/" + removeLoc);

        //Send on Network
        if (SendNetData)
        {
            db.SendData(DataString());
        }
    }

    public void WriteData()
    {
        Data.Reverse();
        Data.Add(ToString());
        Data.Reverse();
    }

    public void OverwriteFields(Children children, bool includeOverwriteAllData = false, bool voidRemove = false)
    {
        Id = children.Id;
        UniqueId = children.UniqueId;
        FirstName = children.FirstName;
        LastName = children.LastName;

        Male = children.Male;
        Birthday = children.Birthday;

        SchoolUID = children.SchoolUID;

        Grade = children.Grade;

        ParentUID = children.ParentUID;

        lastUpdated = children.lastUpdated;

        Computer = children.Computer;

        Sent = children.Sent;

        if (includeOverwriteAllData)
        {
            Data = children.Data;
        }

        Remove = children.Remove;

        if (children.Remove && !voidRemove)
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

    public void UpdateRecord(Children record)
    {
        if (UniqueId == record.UniqueId)
        {
            Database db = Database.instance;

            Data.Add(DataString());

            Id = record.Id;
            UniqueId = record.UniqueId;
            FirstName = record.FirstName;
            LastName = record.LastName;

            Male = record.Male;
            Birthday = record.Birthday;

            SchoolUID = record.SchoolUID;

            Grade = record.Grade;

            ParentUID = record.ParentUID;

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
        Save("Removed", true, "Children");

        //Remove from normal database
        db.children.RemoveAll(x => x.UniqueId == UniqueId);
    }
}
