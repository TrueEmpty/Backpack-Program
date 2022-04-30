using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Diagnostics;

[System.Serializable]
public class FileAdd
{
    public string Filename = null;
    public string Root = null;
    public string Path = null;
    public List<string> Data = new List<string>();
    public System.DateTime lastUpdated;
    public string Extention = "txt";

    //The threads to save
    Thread saveThread;

    public FileAdd(string path, string filename, List<string> data)
    {
        if(path.Substring(0,1) == "/" || path.Substring(0, 1) == "\\")
        {
            path = path.Substring(1, path.Length - 1);
        }

        Path = path;
        Filename = filename;
        Data = data;

        if(data.Count > 0)
        {
            string[] bsp = data[0].Split(';');
            lastUpdated = System.DateTime.Parse(bsp[0]);
        }
    }

    public FileAdd(string path, string filename, string data)
    {
        if (path.Substring(0, 1) == "/" || path.Substring(0, 1) == "\\")
        {
            path = path.Substring(1, path.Length - 1);
        }

        Path = path;
        Filename = filename;
        Data.Add(data);

        string[] bsp = data.Split(';');
        lastUpdated = System.DateTime.Parse(bsp[0]);
    }

    public void Update(string path, string filename, string data)
    {
        if (path.Substring(0, 1) == "/" || path.Substring(0, 1) == "\\")
        {
            path = path.Substring(1, path.Length - 1);
        }

        Path = path;
        Filename = filename;
        Data.Insert(0,data);

        string[] bsp = data.Split(';');
        lastUpdated = System.DateTime.Parse(bsp[0]);
    }

    public string DataToString()
    {
        string result = "";

        if(Data.Count > 0)
        {
            for(int i = 0; i < Data.Count; i++)
            {
                if(i > 0)
                {
                    result += '\n';
                }

                result += Data[i];
            }
        }

        return result;
    }

    public void LocalSave(string localRoot = "",string newData = null,string extention = "txt")
    {
        Root = localRoot;

        if (saveThread == null)
        {
            saveThread = new Thread(SaveThread);
        }

        if(newData != null)
        {
            Data.Insert(0, newData);
        }

        Extention = extention.Replace(".","");

        if (!saveThread.IsAlive && saveThread.ThreadState != System.Threading.ThreadState.Running)
        {
            //Check if path exist if not create it
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }

            saveThread.Start();
        }
    }

    void SaveThread()
    {
        if (Data.Count > 0)
        {
            string fullPath = "";

            if (Root == "" || Root == null)
            {
                fullPath = Path + "/" + Filename + "." + Extention;
            }
            else
            {
                fullPath = Root + "/" + Path + "/" + Filename + "." + Extention;
            }

            //Write some text to the file
            StreamWriter writer = new StreamWriter(fullPath, false);

            //Split data
            string dataSplit = "";

            for (int i = 0; i < Data.Count; i++)
            {
                if(i > 0)
                {
                    dataSplit += "\n";
                }

                dataSplit += Data[i];
            }

            //Write Data
            writer.Write(dataSplit);

            writer.Close();
        }
    }
}
