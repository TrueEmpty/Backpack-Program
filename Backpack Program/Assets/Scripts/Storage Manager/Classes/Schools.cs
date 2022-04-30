using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Schools
{
    //Identification
    public string UniqueId = ""; //Random series of NumbersAndLetters 10 char long
    public string SchoolName = "";

    public List<string> Grades = new List<string>();

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

    public Schools()
    {
        Database db = Database.instance;

        if (db != null)
        {
            List<Schools> sc = db.schools;
            List<string> uuids = null;

            if (sc != null)
            {
                if (sc.Count > 0)
                {
                    uuids = new List<string>();

                    for (int i = 0; i < sc.Count; i++)
                    {
                        uuids.Add(sc[i].UniqueId);
                    }
                }
            }

            UniqueId = "Sc_" + GenerateUniqueID(uuids);

            lastUpdated = System.DateTime.Now;
        }
    }

    public Schools(Schools school)
    {
        UniqueId = school.UniqueId;
        SchoolName = school.SchoolName;

        Grades.Clear();

        if (school.Grades.Count > 0)
        {
            for (int i = 0; i < school.Grades.Count; i++)
            {
                Grades.Add(school.Grades[i]);
            }
        }

        lastUpdated = school.lastUpdated;
        Computer = school.Computer;
        Sent = school.Sent;
        Remove = school.Remove;
    }

    public Schools(List<string> dataList)
    {
        if (dataList.Count > 0)
        {
            Decode(dataList[0]);
        }

        Data = dataList;
    }

    public Schools(string dataString)
    {
        Decode(dataString);
    }

    public void Decode(List<string> dataList)
    {
        string dataString = "";

        if (dataList.Count > 0)
        {
            for(int i = 0; i < dataList.Count; i++)
            {
                if(i > 0)
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
        SchoolName = sp[1];

        string[] ssp = sp[2].Split(',');

        Grades.Clear();
        if (ssp.Length > 0)
        {
            for (int i = 0; i < ssp.Length; i++)
            {
                Grades.Add(ssp[i]);
            }
        }

        Computer = sp[3];

        if (sp.Length > 4)
        {
            Sent = bool.Parse(sp[4]);
        }

        if (sp.Length > 5)
        {
            Remove = bool.Parse(sp[5]);
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
        result += SchoolName.Replace(",", "");
        result += "|";
        if (Grades.Count > 0)
        {
            for (int i = 0; i < Grades.Count; i++)
            {
                result += Grades[i];

                if (i < Grades.Count - 1)
                {
                    result += ",";
                }
            }
        }
        result += "|";
        result += Computer;
        result += "|";
        result += Sent.ToString();
        result += "|";
        result += Remove.ToString();

        return result;
    }

    public void WriteData()
    {
        Data.Reverse();
        Data.Add(ToString());
        Data.Reverse();
    }

    public void OverwriteFields(Schools school, bool includeOverwriteAllData = false, bool voidRemove = false)
    {
        UniqueId = school.UniqueId;
        SchoolName = school.SchoolName;

        Grades.Clear();

        if (school.Grades.Count > 0)
        {
            for (int i = 0; i < school.Grades.Count; i++)
            {
                Grades.Add(school.Grades[i]);
            }
        }

        lastUpdated = school.lastUpdated;
        Computer = school.Computer;
        Sent = school.Sent;

        if (includeOverwriteAllData)
        {
            Data = school.Data;
        }

        Remove = school.Remove;

        if (school.Remove && !voidRemove)
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
        //Set remove
        Remove = true;
    }
}
