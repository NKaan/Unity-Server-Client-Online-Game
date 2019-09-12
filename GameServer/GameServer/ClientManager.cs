using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    static class ClientManager
    {

        public static void CreateNewConnection(TcpClient tempClient)
        {
            Client newClient = new Client();
            newClient.socket = tempClient;
            newClient.connectionID = ((IPEndPoint)tempClient.Client.RemoteEndPoint).Port;
            newClient.Start();
            Sabitler.bagli_client.Add(newClient.connectionID, newClient);

            //if(Sabitler.Odalar)

            Sabitler.bagli_client[newClient.connectionID].baglanti = Sabitler.Mysql_Data.MySqlBaslat();

           


        }


        public static void SendDataTo(int connectionID,byte[] data)
        {
            Kaan_ByteBuffer buffer = new Kaan_ByteBuffer();
            buffer.Int_Yaz((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
            buffer.Bytes_Yaz(data);
            Sabitler.bagli_client[connectionID].stream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
            buffer.Dispose();
        }

        public static async void SendDataToInGameAll(int connectionID,byte[] data)
        {

            Kaan_ByteBuffer buffer = new Kaan_ByteBuffer();
            buffer.Int_Yaz((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
            buffer.Bytes_Yaz(data);

            foreach (Client oyuncu in Sabitler.bagli_client.Values)
            {
                if(oyuncu != null && oyuncu.connectionID != connectionID && oyuncu.oyunda_mi)
                {
                    Sabitler.bagli_client[oyuncu.connectionID].stream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
                }
            }
            await Task.Delay(20);
            buffer.Dispose();
        }

        public static void SendDataToAll(int connectionID, byte[] data)
        {
            Kaan_ByteBuffer buffer = new Kaan_ByteBuffer();
            buffer.Int_Yaz((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
            buffer.Bytes_Yaz(data);

            foreach (Client oyuncu in Sabitler.bagli_client.Values)
            {
                if (oyuncu != null && oyuncu.connectionID != connectionID)
                {
                    Sabitler.bagli_client[oyuncu.connectionID].stream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
                }
            }

            buffer.Dispose();
        }

    }
}
