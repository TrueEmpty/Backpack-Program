using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManagers : MonoBehaviour
{
    public static NetworkManagers instance;
    public bool serverOn = false;
    public bool clientOn = false;
    public string inviteKey = "";
    int port = 5461;

    public List<string> chatLog = new List<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void StartClient(string var1, int var2 = 0)
    {
        if(var2 == 0)
        {
            var2 = port;
        }

        Client.instance.ConnectToServer(var1, var2);
    }

    public void StartServer()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        inviteKey = GetLocalIPv4() + "|" + port;

        Server.Start(10, port);
        serverOn = true;
    }

    void StopServer()
    {
        Server.Stop();
        serverOn = false;
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
        if(serverOn)
        {
            Server.Stop();
            serverOn = false;
        }
    }
}