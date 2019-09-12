using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    static class ServerHandleData
    {
        public delegate void Packet(int connectionID, byte[] data);
        public static Dictionary<int, Packet> packets = new Dictionary<int, Packet>();

        public static void InitializePackets()
        {
            packets.Add((int)ClientPackets.CMerhabaServer, DataReceiver.HandleMerhabaServer);
            packets.Add((int)ClientPackets.CKordinatBilgilerimiGonder, DataReceiver.HandleCordinatBilgileriniGonder);
            packets.Add((int)ClientPackets.CLogin_Giris, DataReceiver.HandleLoginGiris);
            packets.Add((int)ClientPackets.CAnim_Gonder, DataReceiver.Handle_Anim_Al);
        }

        public static void HandleData(int connectionID,byte[] data)
        {
            byte[] buffer = (byte[])data.Clone();
            int pLength = 0;

            if (Sabitler.bagli_client[connectionID].buffer == null)
                Sabitler.bagli_client[connectionID].buffer = new Kaan_ByteBuffer();

            Sabitler.bagli_client[connectionID].buffer.Bytes_Yaz(buffer);
            if(Sabitler.bagli_client[connectionID].buffer.Count() == 0)
            {
                Sabitler.bagli_client[connectionID].buffer.Clear();
                return;
            }

            if(Sabitler.bagli_client[connectionID].buffer.Length() >= 4){

                pLength = Sabitler.bagli_client[connectionID].buffer.Int_Oku(false);
                if(pLength <= 0)
                {
                    Sabitler.bagli_client[connectionID].buffer.Clear();
                    return;
                }
            }

            while (pLength > 0 & pLength <= Sabitler.bagli_client[connectionID].buffer.Length() - 4)
            {
                if(pLength <= Sabitler.bagli_client[connectionID].buffer.Length() - 4)
                {
                    Sabitler.bagli_client[connectionID].buffer.Int_Oku();
                    data = Sabitler.bagli_client[connectionID].buffer.Bytes_Oku(pLength);
                    HandleDataPackets(connectionID, data);
                }
                pLength = 0;

                if(Sabitler.bagli_client[connectionID].buffer.Length() >= 4)
                {
                    pLength = Sabitler.bagli_client[connectionID].buffer.Int_Oku(false);
                    if(pLength <= 0)
                    {
                        Sabitler.bagli_client[connectionID].buffer.Clear();
                        return;
                    }
                }

                if (pLength <= 1)
                    Sabitler.bagli_client[connectionID].buffer.Clear();

            }


        }

        private static void HandleDataPackets(int connetiocID,byte[] data)
        {
            Kaan_ByteBuffer buffer = new Kaan_ByteBuffer();
            buffer.Bytes_Yaz(data);
            int packetID = buffer.Int_Oku();
            buffer.Dispose();
            if(packets.TryGetValue(packetID,out Packet packet))
            {
                packet.Invoke(connetiocID, data);
            }
        }

    }
}
