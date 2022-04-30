using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextFileManager : MonoBehaviour
{
    public static TextFileManager instance;
    public string root = "Assets/TextSaves";
    public List<string> paths = new List<string>();
    public float rate = 0;
    public float startTime = -1; //If -1 then it will never start on its own

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
        if(paths.Count > 0)
        {
            foreach(string p in paths)
            {
                CreatePath(GetPath(p));
            }
        }

        ConsoleManager.instance.Write("Loading in Records...");
        StartCoroutine(LoadFiles());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator LoadFiles( string extention = "txt")
    {
        if(paths.Count > 0)
        {
            for(int i = 0; i < paths.Count; i++)
            {
                Database db = Database.instance;
                DirectoryInfo dir = new DirectoryInfo(GetPath(paths[i]));
                FileInfo[] info = dir.GetFiles("*." + extention);

                foreach (FileInfo f in info)
                {
                    string nam = f.Name.Split('.')[0];
                    string path = GetPath(paths[i]);
                    string fullPath = path + "/" + nam + "." + extention;

                    string data = File.ReadAllText(fullPath);
                    string fLine = data.Split('\n')[0];
                    string ludd = fLine.Split(';')[0];
                    System.DateTime lud = System.DateTime.Parse(ludd);

                    Parents pa = db.parents.Find(x => x.UniqueId == nam);
                    Schools sc = db.schools.Find(x => x.UniqueId == nam);
                    PickUpTimes pu = db.pickuptimes.Find(x => x.UniqueId == nam);
                    Children ch = db.children.Find(x => x.UniqueId == nam);

                    if (pa != null)
                    {
                        if (lud > pa.lastUpdated)
                        {
                            pa.Decode(data);
                        }
                    }
                    else if (sc != null)
                    {
                        if (lud > sc.lastUpdated)
                        {
                            sc.Decode(data);
                        }
                    }
                    else if (pu != null)
                    {
                        if (lud > pu.lastUpdated)
                        {
                            pu.Decode(data);
                        }
                    }
                    else if (ch != null)
                    {
                        if (lud > ch.lastUpdated)
                        {
                            ch.Decode(data);
                        }
                    }
                    else //Does not exist add to Database
                    {
                        switch (nam.Substring(0, 3).Trim())
                        {
                            case "Pa_":
                                Parents pr = new Parents(data);
                                db.AddNew(pr);
                                ConsoleManager.instance.Write("New Parent [" + pr.GetFullName() + "] loaded");
                                break;
                            case "Sc_":
                                Schools sr = new Schools(data);
                                db.AddNew(sr);
                                ConsoleManager.instance.Write("New School [" + sr.SchoolName + "] loaded");
                                break;
                            case "Pu_":
                                PickUpTimes tr = new PickUpTimes(data);
                                db.AddNew(tr);
                                ConsoleManager.instance.Write("New Pickup Time [" + tr.Times + "] loaded");
                                break;
                            case "Ch_":
                                Children cr = new Children(data);
                                db.AddNew(cr);
                                ConsoleManager.instance.Write("New Child [" + cr.GetName() + "] loaded");
                                break;
                            default:
                                ConsoleManager.instance.Write("Location for [" + nam + "] can not be found");
                                break;
                        }
                    }

                    yield return new WaitForSeconds(1);
                }

                yield return new WaitForSeconds(1);
            }
        }

        yield return new WaitForSeconds(1);
    }

    public void SaveRecord(string path, string filename, System.DateTime lastUpdate, string dStr, string removepath = "")
    {
        StartCoroutine(RecordSave(path, filename, lastUpdate,  dStr, removepath));
    }

    IEnumerator RecordSave(string path, string filename, System.DateTime lastUpdate, string dStr, string removepath = "")
    {
        string fullPath = path + "/" + filename + ".txt";
        string secPath = removepath + "/" + filename + ".txt";

        bool dontOverwrite = false;

        //Check if path exist if not create it
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            yield return new WaitForEndOfFrame();
        }

        //Check if File Exist if not create it
        if (File.Exists(fullPath))
        {
            //Read the text from directly from the file
            string data = File.ReadAllText(fullPath);

            string[] bsp = data.Split(';');
            System.DateTime lud = System.DateTime.Parse(bsp[0]);

            //Check if Files last Date is after this last date
            if (lud > lastUpdate)
            {
                dontOverwrite = true;
            }

            yield return new WaitForEndOfFrame();
        }

        if (!dontOverwrite && dStr != null)
        {
            //Save File
            StreamWriter writer = new StreamWriter(fullPath, false);

            //Split data
            writer.Write(dStr);

            writer.Close();
        }

        yield return new WaitForEndOfFrame();

        if(removepath != "" && removepath != null)
        {
            //Check if path exist if not stop
            if (Directory.Exists(path))
            {
                //Check if File Exist if not stop
                if (File.Exists(secPath))
                {
                    File.Delete(secPath);
                }
            }
        }
        yield return new WaitForEndOfFrame();

    }

    public void CreatePath(string path)
    {
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public string GetPath(string path = "")
    {
        string result = "";

        if(root != "")
        {
            result = root;
        }

        if(result != "" && (path != "" || path != null))
        {
            result += "/";
        }

        result += path;

        return result;
    }
 
}
