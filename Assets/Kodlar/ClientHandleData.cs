using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandleData : MonoBehaviour
{
    public delegate void Packet(byte[] data);
    public static Dictionary<int, Packet> packets = new Dictionary<int, Packet>();
    public static ByteBuffer playerBuffer;

    public static void InitializePackets()
    {
        packets.Add((int)ServerPackets.SHosgeldinMesaji, DataReceiver.HandleHosGeldinMesaji);
        packets.Add((int)ServerPackets.SOyundakiler, DataReceiver.HandleOyundakiler);
        packets.Add((int)ServerPackets.SKendiBilgilerimiOyuncularaGonder, DataReceiver.HandleOyuncuGirisYapti);
        packets.Add((int)ServerPackets.SKordinatBilgileriniGonder, DataReceiver.HandleOyuncuKordinatBilgileri);
        packets.Add((int)ServerPackets.SOyuncuOyundanCikti, DataReceiver.HandleOyuncuOyundanCikti);
        packets.Add((int)ServerPackets.SLogin_Giris_Cevap, DataReceiver.HandleLoginGirisCevap);
        packets.Add((int)ServerPackets.SAnim_Gonder, DataReceiver.HandleAnimAl);
    }

    public static void HandleData(byte[] data)
    {
        byte[] buffer = (byte[])data.Clone();
        int pLength = 0;

        if (playerBuffer == null)
            playerBuffer = new ByteBuffer();

        playerBuffer.Bytes_Yaz(buffer);
        if (playerBuffer.Count() == 0)
        {
            playerBuffer.Clear();
            return;
        }

        if (playerBuffer.Length() >= 4)
        {

            pLength = playerBuffer.Int_Oku(false);
            if (pLength <= 0)
            {
                playerBuffer.Clear();
                return;
            }
        }

        while (pLength > 0 & pLength <= playerBuffer.Length() - 4)
        {
            if (pLength <= playerBuffer.Length() - 4)
            {
                playerBuffer.Int_Oku();
                data = playerBuffer.Bytes_Oku(pLength);
                HandleDataPackets(data);
            }
            pLength = 0;

            if (playerBuffer.Length() >= 4)
            {
                pLength = playerBuffer.Int_Oku(false);
                if (pLength <= 0)
                {
                    playerBuffer.Clear();
                    return;
                }
            }

            if (pLength <= 1)
                playerBuffer.Clear();

        }


    }

    private static void HandleDataPackets(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.Bytes_Yaz(data);
        int packetID = buffer.Int_Oku();
        buffer.Dispose();
        if (packets.TryGetValue(packetID, out Packet packet))
        {
            packet.Invoke(data);
        }
    }
}
