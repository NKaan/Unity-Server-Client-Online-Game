using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public enum ServerPackets
    {
        SHosgeldinMesaji = 1,
        SOyundakiler = 2,
        SKendiBilgilerimiOyuncularaGonder = 3,
        SKordinatBilgileriniGonder = 4,
        SOyuncuOyundanCikti = 5,
        SLogin_Giris_Cevap = 6,
        SAnim_Gonder = 7,
    }

    static class DataSender
    {

        public static void SendHodsgeldinMesaji(int connectionID)
        {
            Kaan_ByteBuffer buffer = new Kaan_ByteBuffer();
            buffer.Int_Yaz((int)ServerPackets.SHosgeldinMesaji); //Paket Numarası
            buffer.Int_Yaz(connectionID); // ConnectionID Numarası Yani Port Numarası,,,
            buffer.String_Yaz("Merhaba , Sunucuya Hoş Geldin..");
            buffer.Float_Yaz(Sabitler.bagli_client[connectionID].xCord);
            buffer.Float_Yaz(Sabitler.bagli_client[connectionID].yCord);
            buffer.Float_Yaz(Sabitler.bagli_client[connectionID].zCord);
            ClientManager.SendDataTo(connectionID, buffer.ToArray());
            SendOyundakiler(connectionID);
            SendKendiBilgilerimiOyuncularaGonder(connectionID);
            buffer.Dispose();
            Sabitler.bagli_client[connectionID].oyunda_mi = true;
        } 

        public static void SendOyundakiler(int connectionID)
        {

            foreach (Client oyuncu in Sabitler.bagli_client.Values)
            {
                if(oyuncu != null && oyuncu.oyunda_mi == true && oyuncu.connectionID != connectionID)
                {
                    Kaan_ByteBuffer buffer = new Kaan_ByteBuffer();
                    buffer.Int_Yaz((int)ServerPackets.SOyundakiler); //Paket Numarası
                    buffer.Int_Yaz(oyuncu.connectionID); // ConnectionID Numarası Yani Port Numarası,,,
                    buffer.Float_Yaz(oyuncu.xCord);
                    buffer.Float_Yaz(oyuncu.yCord);
                    buffer.Float_Yaz(oyuncu.zCord);
                    ClientManager.SendDataTo(connectionID, buffer.ToArray());
                    buffer.Dispose();
                }
            }

        }

        public static void SendKendiBilgilerimiOyuncularaGonder(int connectionID)
        {
            Kaan_ByteBuffer buffer = new Kaan_ByteBuffer();
            buffer.Int_Yaz((int)ServerPackets.SKendiBilgilerimiOyuncularaGonder); //Paket Numarası
            buffer.Int_Yaz(connectionID); // ConnectionID Numarası Yani Port Numarası,,,
            buffer.Float_Yaz(Sabitler.bagli_client[connectionID].xCord);
            buffer.Float_Yaz(Sabitler.bagli_client[connectionID].yCord);
            buffer.Float_Yaz(Sabitler.bagli_client[connectionID].zCord);
            ClientManager.SendDataToInGameAll(connectionID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendKordinatBilgileriniGonder(int connectionID,float posX,float posY,float posZ, float rotX,float rotY,float rotZ,float rotW)
        {
            Kaan_ByteBuffer buffer = new Kaan_ByteBuffer();
            buffer.Int_Yaz((int)ServerPackets.SKordinatBilgileriniGonder); //Paket Numarası
            buffer.Int_Yaz(connectionID); // ConnectionID Numarası Yani Port Numarası,,,

            buffer.Float_Yaz(posX);
            buffer.Float_Yaz(posY);
            buffer.Float_Yaz(posZ);

            buffer.Float_Yaz(rotX);
            buffer.Float_Yaz(rotY);
            buffer.Float_Yaz(rotZ);
            buffer.Float_Yaz(rotW);

            ClientManager.SendDataToInGameAll(connectionID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendOyuncuOyundanCikti(int connectionID)
        {
            Kaan_ByteBuffer buffer = new Kaan_ByteBuffer();
            buffer.Int_Yaz((int)ServerPackets.SOyuncuOyundanCikti); //Paket Numarası
            buffer.Int_Yaz(connectionID);
            ClientManager.SendDataToInGameAll(connectionID,buffer.ToArray());
            buffer.Dispose();

        }

        public static void SendLoginGirisCevap(int connectionID,int cevap)
        {
            Kaan_ByteBuffer buffer = new Kaan_ByteBuffer();
            buffer.Int_Yaz((int)ServerPackets.SLogin_Giris_Cevap); //Paket Numarası
            buffer.Int_Yaz(cevap);
            ClientManager.SendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendAnimGonder(int connectionID , string animasyon_kimde_oynayacak , string anim_adi,int akitflik)
        {
            Kaan_ByteBuffer buffer = new Kaan_ByteBuffer();
            buffer.Int_Yaz((int)ServerPackets.SAnim_Gonder); //Paket Numarası
            buffer.Int_Yaz(connectionID);
            buffer.String_Yaz(animasyon_kimde_oynayacak);
            buffer.String_Yaz(anim_adi);
            buffer.Int_Yaz(akitflik);
            ClientManager.SendDataToInGameAll(connectionID, buffer.ToArray());
            buffer.Dispose();
        }

    }
}
