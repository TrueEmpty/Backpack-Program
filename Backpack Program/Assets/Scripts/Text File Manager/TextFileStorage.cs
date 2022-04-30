using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class TextFileStorage
{
    public string Path = null;
    public string Filename = null;
    public string Extention = "txt";
    public string Data = null;
    public System.DateTime lastUpdated = new System.DateTime(2000, 1, 1, 1, 1, 1, 1);

    //The threads to save and load
    Thread saveThread;

    public TextFileStorage()
    {
        saveThread = new Thread(SaveThread);
    }

    public TextFileStorage(string fileName,string path = "Assets/TextSaves",string extention = "txt")
    {
        Filename = fileName;
        Path = path;
        Extention = extention;
        
        saveThread = new Thread(SaveThread);

        LoadFile();
    }

    public TextFileStorage(TextFileStorage textFileStorage)
    {
        Filename = textFileStorage.Filename;
        Path = textFileStorage.Path;
        Data = textFileStorage.Data;
        Extention = textFileStorage.Extention;
        lastUpdated = textFileStorage.lastUpdated;

        saveThread = new Thread(SaveThread);
    }

    public void SetTextFileStorage(TextFileStorage textFileStorage)
    {
        Filename = textFileStorage.Filename;
        Path = textFileStorage.Path;
        Data = textFileStorage.Data;
        Extention = textFileStorage.Extention;
        lastUpdated = textFileStorage.lastUpdated;
    }

    public bool SaveFile()
    {
        bool result = false;
        bool cont = true;

        if (saveThread.IsAlive)
        {
            cont = false;
        }

        if (Filename == "" || Filename == null)
        {
            cont = false;
        }

        if (Path == "" || Path == null)
        {
            cont = false;
        }

        if (cont)
        {
            //Check if path exist if not create it
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }

            result = true;

            saveThread.Start();
        }

        return result;
    }

    public bool LoadFile()
    {
        bool result = false;
        bool cont = true;

        if (Filename == "" || Filename == null)
        {
            cont = false;
        }

        if (Path == "" || Path == null)
        {
            cont = false;
        }

        if (Extention == "" || Extention == null)
        {
            cont = false;
        }

        //Check if path exist if not create it
        if (!Directory.Exists(Path))
        {
            cont = false;
        }

        if (cont)
        {
            result = true;

            LoadThread();
        }

        return result;
    }

    void SaveThread()
    {
        string fullPath = Path + "/" + Filename + "." + Extention;

        //Write some text to the file
        StreamWriter writer = new StreamWriter(fullPath, false);

        //Split data
        writer.WriteLine(Data);

        writer.Close();
    }

    void LoadThread()
    {
        string fullPath = Path + "/" + Filename + "." + Extention;

        if(File.Exists(fullPath))
        {
            //Read the text from directly from the file
            string data = File.ReadAllText(fullPath);
            TextFileStorage tfs = new TextFileStorage();

            tfs.Filename = Filename;
            tfs.Path = Path;
            tfs.Extention = Extention;

            string[] bsp = data.Split(';');
            System.DateTime lud = System.DateTime.Parse(bsp[0]);

            tfs.Data = data;
            tfs.lastUpdated = lud;

            SetTextFileStorage(tfs);
        }
    }
}
