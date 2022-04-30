using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        // Now that we have the client's id, connect UDP
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);

        NetworkManagers.instance.clientOn = true;
    }

    public static void DataTransfer(Packet _packet)
    {
        string _msg = _packet.ReadString();

        Debug.Log($"Message recieved from server: {_msg}");

        //Get the Data
        Database.instance.IncomingRecord(_msg);
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();
    }
}
