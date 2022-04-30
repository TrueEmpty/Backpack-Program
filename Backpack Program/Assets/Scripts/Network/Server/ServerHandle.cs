using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _Computer = _packet.ReadString();

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now Computer {_Computer}.");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Computer \"{_Computer}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }       
    }

    public static void DataTransfer(int _fromClient, Packet _packet)
    {
        string _msg = _packet.ReadString();

        //Get the Data
        Database.instance.IncomingRecord(_msg);
    }
}