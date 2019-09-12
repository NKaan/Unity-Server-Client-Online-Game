using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class ClientTCP
{
    private static TcpClient clientSocket;
    private static NetworkStream myStream;
    private static byte[] recBuffer;

    public static void InitializingNetworking()
    {
        clientSocket = new TcpClient();
        clientSocket.ReceiveBufferSize = 4096;
        clientSocket.SendBufferSize = 4096;
        recBuffer = new byte[4096 * 2];
        clientSocket.BeginConnect("Sunucu IP Adresinizi Giriniz", 6060, new AsyncCallback(ClientConnectCallback), clientSocket);
    }

    private static void ClientConnectCallback(IAsyncResult result)
    {
       
        clientSocket.EndConnect(result);
        if (clientSocket.Connected == false)
        {
            return;
        }
        else
        {
           
            clientSocket.NoDelay = true;
           
            myStream = clientSocket.GetStream();
           
            myStream.BeginRead(recBuffer, 0, 4096 * 2, ReceiveCallback, null);
            
        }

    }

    private static void ReceiveCallback(IAsyncResult result)
    {

        try
        {
            int lenght = myStream.EndRead(result);
            if (lenght <= 0)
            {              
                return;
            }

            byte[] newBytes = new byte[lenght];
            Array.Copy(recBuffer, newBytes, lenght);
            UnityThread.executeInFixedUpdate(() =>
            {
                ClientHandleData.HandleData(newBytes);
            });
            myStream.BeginRead(recBuffer, 0, 4096 * 2, ReceiveCallback, null);
        }
        catch (Exception)
        {
            Disconnect();
            return;
        }

    }


    public static void SendData(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.Int_Yaz((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
        buffer.Bytes_Yaz(data);
        myStream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
        buffer.Dispose();
    }

    public static void Disconnect()
    {
        clientSocket.Close();
        Debug.Log("Çıkış Yapıldı");
    }


}
