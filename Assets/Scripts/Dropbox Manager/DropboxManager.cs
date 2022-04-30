using DBXSync;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;
using System.Linq;
using DBXSync.Model;
using System.IO;
using System.Net;
using UnityEngine.UI;
using System.Collections;

public class DropboxManager : DebuggerLog
{
    public string localRootpath = "Database";

    string myIp = "";
    public string MasterIp = "";

    public Text connectionDisplay;
    public Color masterCD = Color.red;
    public Color clientCD = Color.green;
    public Color notconnectedCD = Color.gray;

    ConsoleManager cm;

    // Use this for initialization
    void Start()
    {
        cm = ConsoleManager.instance;

        cm.Write("Running Connect");
        myIp = GetLocalIPv4();
        StartCoroutine(GetConnectionFile());
    }

    void Update()
    {

    }

    public void OverrideMaster()
    {
        StartCoroutine(OverrideMaster("/Info/Info.txt"));
    }

    IEnumerator OverrideMaster(string path)
    {

        DropboxSync.Main.GetFile<string>(path, (res) => {
            if (res.error != null)
            {
                Log("Failed to download " + path + " to cache", res.error.ErrorDescription + '\n' + "Script:DropBoxManager ", LogType.Error);
                ConsoleManager.instance.Write("Failed to check for Master" + '\n' + res.error.ErrorDescription + '\n' + "Script:DropBoxManager OverrideMaster 55");
            }
            else
            {
                //Place file in fileAddList
                string data = res.data;

                string[] sa = data.Split('\n');

                MasterIp = myIp;
                NetworkManagers.instance.StartServer();
                data = "Info/base" + '\n' + myIp;
                StartCoroutine(UploadDBFile(data));

                ConsoleManager.instance.Write("Overrided Master Computer! Warning if original Master is still running please restart it. New connections will connect here!");

                if (MasterIp == myIp)
                {
                    connectionDisplay.text = "Master";
                    connectionDisplay.color = masterCD;
                }
                else if (MasterIp == "")
                {
                    connectionDisplay.text = "Not Connected";
                    connectionDisplay.color = notconnectedCD;
                }
                else
                {
                    connectionDisplay.text = "Client";
                    connectionDisplay.color = clientCD;
                }
            }
        }, (progress) => {
            Log("Downloading " + path, (progress * 100).ToString() + "%" + '\n' + "Script:DropBoxManager RenderFolderItems", LogType.Info);
        });

        yield return new WaitForSeconds(1);
    }

    IEnumerator GetConnectionFile()
    {
        string path = "/Info/Info.txt";


        DropboxSync.Main.GetFile<string>(path, (res) => {
            if (res.error != null)
            {
                Log("Failed to download " + path + " to cache", res.error.ErrorDescription + '\n' + "Script:DropBoxManager RenderFolderItems", LogType.Error);
                ConsoleManager.instance.Write("Failed to check for Master" + '\n' + res.error.ErrorDescription + '\n' + "Script:DropBoxManager GetConnectionFile 103");
            }
            else
            {
                //Place file in fileAddList
                string data = res.data;

                string[] sa = data.Split('\n');

                string connectionIp = "";

                if (sa.Length > 1)
                {
                    connectionIp = sa[1];
                }

                Debug.Log(connectionIp);

                if(connectionIp.Trim() == myIp || connectionIp == "")
                { //Set this computer to master
                    MasterIp = myIp;
                    NetworkManagers.instance.StartServer();
                    data = "Info/base" + '\n' + myIp;
                    StartCoroutine(UploadDBFile(data));
                    cm.Write("Connect Complete: This is set as the Master");
                }
                else
                {
                    MasterIp = connectionIp.Trim();
                    NetworkManagers.instance.StartClient(MasterIp);
                    cm.Write("Connect Complete: This is set as a Client");
                }

                if (MasterIp == myIp)
                {
                    connectionDisplay.text = "Master";
                    connectionDisplay.color = masterCD;
                }
                else if (MasterIp == "")
                {
                    connectionDisplay.text = "Not Connected";
                    connectionDisplay.color = notconnectedCD;
                }
                else
                {
                    connectionDisplay.text = "Client";
                    connectionDisplay.color = clientCD;
                }
            }
        }, (progress) => {
            Log("Downloading " + path, (progress * 100).ToString() + "%" + '\n' + "Script:DropBoxManager RenderFolderItems", LogType.Info);
        });

        yield return new WaitForSeconds(1);
    }

    #region Upload Methods

    IEnumerator UploadDBFile(string DataToUpload)
    {

        string path = "/Info/Info.txt";

        var textToUpload = DataToUpload;
        var bytesToUpload = Encoding.UTF8.GetBytes(textToUpload);

        DropboxSync.Main.UploadFile(path, bytesToUpload, (res) => {

            if (res.error != null)
            {
                cm.Write(res.error.ErrorDescription);
            }
        });

        yield return new WaitForSeconds(1);
    }

    #endregion

    //0 = false, 1 = true, -1 == null
    public int IsMaster()
    {
        int result = 0;

        if(MasterIp == myIp)
        {
            result = 1;
        }
        
        if(MasterIp == null)
        {
            result = -1;
        }

        return result;
    }

    public string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName())
            .AddressList.First(
                f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            .ToString();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Quitting");

        if(myIp == MasterIp)
        {
            StartCoroutine(UploadDBFile("Info/base"));
        }

        Debug.Log("Quit");
    }
}