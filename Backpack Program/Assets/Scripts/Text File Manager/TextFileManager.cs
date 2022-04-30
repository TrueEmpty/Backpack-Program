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

    public List<TextFileStorage> filesAdded = new List<TextFileStorage>();

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

        LoadFiles();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<TextFileStorage> GetFilesInPath(string location,string extention = "txt")
    {
        List<TextFileStorage> result = new List<TextFileStorage>();

        DirectoryInfo dir = new DirectoryInfo(GetPath(location));
        FileInfo[] info = dir.GetFiles("*." + extention);

        foreach (FileInfo f in info)
        {
            string[] na = f.Name.Split('.');
            result.Add(new TextFileStorage(na[0], GetPath(location), extention));
        }

        return result;
    }

    public List<TextFileStorage> GetFilesInAllPaths(List<string> locations, string extention = "txt")
    {
        List<TextFileStorage> result = new List<TextFileStorage>();

        if (locations.Count > 0)
        {
            foreach(string s in locations)
            {
                result.AddRange(GetFilesInPath(s,extention));
            }
        }

        return result;
    }

    public List<TextFileStorage> GetFilesInAllPaths(List<string> paths)
    {
        List<TextFileStorage> result = new List<TextFileStorage>();

        if (paths.Count > 0)
        {
            foreach (string s in paths)
            {
                result.AddRange(GetFilesInPath(s));
            }
        }

        return result;
    }

    public void LoadFiles()
    {
        //Get all files in the paths
        List<TextFileStorage> fps = GetFilesInAllPaths(paths);

        if(fps.Count > 0)
        {
            Database db = Database.instance;

            foreach(TextFileStorage tfs in fps)
            {
                Parents pa = db.parents.Find(x=> x.UniqueId == tfs.Filename);
                Schools sc = db.schools.Find(x => x.UniqueId == tfs.Filename);
                PickUpTimes pu = db.pickuptimes.Find(x => x.UniqueId == tfs.Filename);
                Children ch = db.children.Find(x => x.UniqueId == tfs.Filename);

                if(pa != null)
                {
                    if(tfs.lastUpdated > pa.lastUpdated)
                    {
                        pa.Decode(tfs.Data);
                    }
                }
                else if (sc != null)
                {
                    if (tfs.lastUpdated > sc.lastUpdated)
                    {
                        sc.Decode(tfs.Data);
                    }
                }
                else if (pu != null)
                {
                    if (tfs.lastUpdated > pu.lastUpdated)
                    {
                        pu.Decode(tfs.Data);
                    }
                }
                else if (ch != null)
                {
                    if (tfs.lastUpdated > ch.lastUpdated)
                    {
                        ch.Decode(tfs.Data);
                    }
                }
                else //Does not exist add to Database
                {
                    switch(tfs.Filename.Substring(0,3).Trim())
                    {
                        case "Pa_":
                            db.parents.Add(new Parents(tfs.Data));
                            break;
                        case "Sc_":
                            db.schools.Add(new Schools(tfs.Data));
                            break;
                        case "Pu_":
                            db.pickuptimes.Add(new PickUpTimes(tfs.Data));
                            break;
                        case "Ch_":
                            db.children.Add(new Children(tfs.Data));
                            break;
                        default:
                            ConsoleManager.instance.Write("Location for [" + tfs.Filename + "] can not be found");
                            break;
                    }
                }
            }
        }
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

    public void SaveFiles(string path, List<string> data)
    {
        //Check if there are any records to upload
        if (data.Count > 0)
        {
            for (int i = 0; i < data.Count; i++)
            {
                string filename = GetFileName(data[i]);
                System.DateTime lastUpdated = GetFileLastUploadDateTime(data[i]);

                //Cant upload a file if there is not a file name
                if (filename != null)
                {
                    if (filesAdded.Count > 0)
                    {
                        //Check if File add has that file in that path
                        TextFileStorage fA = filesAdded.Find(x => x.Path == GetPath(path) && x.Filename == filename);

                        //Check if file was found
                        if (fA != null)
                        {
                            //Check if the incoming file has been updated later
                            if (fA.lastUpdated < lastUpdated)
                            {
                                //Upload incoming file
                                SaveFile(GetPath(path), filename, data[i]);

                                //Delete Previous
                                filesAdded.Remove(fA);
                            }
                        }
                        else
                        {
                            //No files found upload the incoming file
                            SaveFile(GetPath(path), filename, data[i]);
                        }
                    }
                    else
                    {
                        //No files upload the incoming file
                        SaveFile(GetPath(path), filename, data[i]);
                    }

                }
            }
        }
    }

    public void SaveFile(string path,string filename,string data)
    {
        System.DateTime lud = GetFileLastUploadDateTime(data);

        //Check if it exist
        if (filesAdded.Exists(x => x.Path == path && x.Filename == filename))
        {
            TextFileStorage ffs = filesAdded.Find(x => x.Path == path && x.Filename == filename);

            if (ffs.lastUpdated < lud)
            {
                string wData = data;

                if(ffs.Data != "")
                {
                    wData += "\n" + ffs.Data;
                }

                ffs.Data = wData;
                ffs.lastUpdated = lud;

                ffs.SaveFile();
            }
        }
        else
        {
            TextFileStorage nfs = new TextFileStorage(filename,path);

            nfs.Data = data;
            nfs.lastUpdated = lud;

            nfs.SaveFile();

            filesAdded.Add(new TextFileStorage(nfs));
        }
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

    //Will pull the file name out of the content which will be the first value after the last updated
    string GetFileName(string content)
    {
        string result = null;

        //Grab last updated field
        string[] bsp = GetMostRecentData(content).Split(';');
        //Get Data
        string[] sp = bsp[1].Split('|');

        result = sp[0];

        return result;
    }

    public List<TextFileStorage> GetFileList(string pathPerameter = null, string filenamePerameter = null)
    {
        List<TextFileStorage> result = new List<TextFileStorage>();

        if (filesAdded.Count > 0)
        {
            if (pathPerameter != null && filenamePerameter != null)
            {
                result = filesAdded.FindAll(x => x.Path.Trim().ToLower() == GetPath(pathPerameter).Trim().ToLower() && x.Filename == filenamePerameter);
            }
            else if (pathPerameter != null)
            {
                result = filesAdded.FindAll(x => x.Path.Trim().ToLower() == GetPath(pathPerameter).Trim().ToLower());
            }
            else if (filenamePerameter != null)
            {
                result = filesAdded.FindAll(x => x.Filename == filenamePerameter);
            }
            else
            {
                result = filesAdded;
            }
        }

        return result;
    }

    public string GetFileUID(TextFileStorage file)
    {
        //Grab last updated field
        string[] bsp = GetMostRecentData(file).Split(';');
        //Get Data
        string[] sp = bsp[1].Split('|');

        string result = sp[0];

        return result;
    }

    public DateTime GetFileLastUploadDateTime(string content)
    {
        //Grab last updated field
        string[] bsp = GetMostRecentData(content).Split(';');
        System.DateTime result = System.DateTime.Parse(bsp[0]);

        return result;
    }

    public string GetMostRecentData(TextFileStorage file)
    {
        string result = "";

        //Grab last updated field
        string[] mrd = file.Data.Split('\n');

        if(mrd.Length > 0)
        {
            result = mrd[0];
        }

        return result;
    }

    public string GetMostRecentData(string content)
    {
        string result = "";

        //Grab last updated field
        string[] mrd = content.Split('\n');

        if (mrd.Length > 0)
        {
            result = mrd[0];
        }

        return result;
    }
}
