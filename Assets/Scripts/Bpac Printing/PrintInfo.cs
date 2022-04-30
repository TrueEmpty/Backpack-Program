using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class PrintInfo : MonoBehaviour
{
    public static PrintInfo instance;
    public Database db;
    ParentInfo pI;
    public AddNewChild anc;

    string basepath = "";
    string infoFileName = "BackPackInfo.csv";
    string templateName = "BackPackLabel.lbx";

    public List<string> printItems = new List<string>();
    public List<string> addToprintItems = new List<string>();

    public Vector2 printDelay = new Vector2(2,4); //Start Print,Reset Print

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
        basepath = Application.dataPath + "\\Print Template\\";
        pI = ParentInfo.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsInvoking("StartPrint") && !IsInvoking("ResetPrint"))
        {
            if (printItems.Count > 0)
            {
                Invoke("StartPrint", printDelay.x);
            }
        }
    }

    public void PrintCurrentChildren(Children child)
    {
        //Get Print Amount
        int pAmount = db.printAmount;

        if(db.printAmount <= 0)
        {
            pAmount = 1;
            db.printAmount = 1;
        }

        Schools school = db.schools.Find(x => x.UniqueId == child.SchoolUID);
        Parents parent = db.parents.Find(x => x.UniqueId == child.ParentUID);

        if (school == null)
        {
            school = new Schools();
        }

        if (parent == null)
        {
            parent = new Parents();
        }

        for (int a = 0; a < pAmount; a++)
        {
            printItems.Add(child.GetPrint(parent, school));
        }
    }

    public void PrintCurrentChildren()
    {
        //Get Parent's Children
        List<Children> parentsChildren = db.children.FindAll(x => x.ParentUID == pI.parent.UniqueId);

        if (parentsChildren.Count > 0)
        {
            //Get Print Amount
            int pAmount = db.printAmount;

            if (db.printAmount <= 0)
            {
                pAmount = 1;
                db.printAmount = 1;
            }

            for (int i = 0; i < parentsChildren.Count; i++)
            {
                Children child = parentsChildren[i];
                Schools school = db.schools.Find(x => x.UniqueId == child.SchoolUID);
                Parents parent = db.parents.Find(x => x.UniqueId == child.ParentUID);

                if (school == null)
                {
                    school = new Schools();
                }

                if (parent == null)
                {
                    parent = new Parents();
                }

                for (int a = 0; a < pAmount; a++)
                {
                    printItems.Add(child.GetPrint(parent, school));
                }
            }
        }
    }

    void StartPrint()
    {
        //Update csv File based on printItems
        string csvPath = basepath + infoFileName;
        //File.WriteAllText(csvPath, printItems[0]);
        StreamWriter writer = new StreamWriter(csvPath, false);
        writer.WriteLine(printItems[0]);
        writer.Close();

        //Print File
        string runPath = basepath + templateName;

        ProcessStartInfo info = new ProcessStartInfo(runPath);

        info.Verb = "Print";

        info.CreateNoWindow = true;

        info.WindowStyle = ProcessWindowStyle.Hidden;

        Process.Start(info);

        if (!IsInvoking("ResetPrint"))
        {
            Invoke("ResetPrint", printDelay.y);
        }
    }

    void ResetPrint()
    {
        printItems.RemoveAt(0);
    }
}