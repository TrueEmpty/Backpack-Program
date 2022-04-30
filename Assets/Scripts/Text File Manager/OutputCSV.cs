using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class OutputCSV : MonoBehaviour
{
    public string startDate = "6/1/2021";
    public string endDate = "9/1/2021";

    Database db;

    string Path = null;
    string Filename = null;
    string Extention = "csv";

    Thread saveThread;
    string Data = "";

    // Start is called before the first frame update
    void Start()
    {
        db = gameObject.GetComponent<Database>();


            Filename = "Data Report";
            Path = "Database/Outputs";
            Extention = "csv";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OutputData()
    {
        System.DateTime sd;
        bool sdr = false;
        System.DateTime ed;
        bool edr = false;

        sdr = System.DateTime.TryParse(startDate,out sd);
        edr = System.DateTime.TryParse(startDate, out ed);

        if(!sdr)
        {
            System.DateTime.TryParse("1/1/1800",out sd);
        }

        if (!edr)
        {
            System.DateTime.TryParse("1/1/2300", out ed);
        }

        List<string> outputs = new List<string>();

        if(db.children.Count > 0)
        {
            List<Children> oC = new List<Children>();

            oC = db.children.FindAll(x=> x.lastUpdated.CompareTo(sd) >= 0 && x.lastUpdated.CompareTo(ed) <= 0);

            if (oC.Count > 0)
            {
                for(int i = 0; i < oC.Count; i++)
                {
                    string outLine = "";

                    Schools school = db.schools.Find(x => x.UniqueId == oC[i].SchoolUID);
                    Parents parent = db.parents.Find(x => x.UniqueId == oC[i].ParentUID);

                    if (school == null)
                    {
                        school = new Schools();
                    }

                    if (parent == null)
                    {
                        parent = new Parents();
                    }

                    outLine = oC[i].GetAllDataCSV(parent, school);

                    outputs.Add(outLine);
                }
            }
        }

        if(outputs.Count > 0)
        {
            System.DateTime sdt = System.DateTime.Now;

            string hour = sdt.Hour.ToString();
            string min = sdt.Minute.ToString();
            string sec = sdt.Second.ToString();

            Filename = "Data Report " + hour + min + sec;

            //Check if path exist if not create it
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            else
            {
                Debug.Log("Path (" + Path + ") Found");
            }
           

            Data = GetCSVHeader();

            for (int i = 0; i < outputs.Count; i++)
            {
                //Split data
                Data += "\n" + outputs[i];
            }


            saveThread = new Thread(OutputCSVThread);
            saveThread.Start();

            Debug.Log("Output Count (" + outputs.Count + ")");
        }
        else
        {
            Debug.Log("Output Count (" + outputs.Count + ")");
        }
    }

    void OutputCSVThread()
    {
        if(Data != "")
        {
            string fullPath = Path + "/" + Filename + "." + Extention;

            //Write some text to the file
            StreamWriter writer = new StreamWriter(fullPath, false);

            //Split data
            writer.WriteLine(Data);

            writer.Close();

            Data = "";
        }
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
}
