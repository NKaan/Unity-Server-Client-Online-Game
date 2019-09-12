using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public enum ClientPackets
    {
        CMerhabaServer = 1,
        CKordinatBilgilerimiGonder = 2,
        CLogin_Giris = 3,
        CAnim_Gonder = 4,
    }

    static class DataReceiver
    {     
        public static void HandleMerhabaServer(int connectionID,byte[] data)
        {
            Kaan_ByteBuffer buffer = new Kaan_ByteBuffer();
            buffer.Bytes_Yaz(data);
            int packetID = buffer.Int_Oku();
            string msg = buffer.String_Oku();
            buffer.Dispose();
            Yazi.Gelen_Mesaj(connectionID + " " + msg);
            DataSender.SendHodsgeldinMesaji(connectionID);

        }

        public static void Handle_Anim_Al(int connectionID,byte[] data)
        {
            Kaan_ByteBuffer buffer = new Kaan_ByteBuffer();
            buffer.Bytes_Yaz(data);
            int packetID = buffer.Int_Oku();
            string animasyon_kimde_oynayacak = buffer.String_Oku();
            string anim_Adi = buffer.String_Oku();
            int akiflik = buffer.Int_Oku();

            DataSender.SendAnimGonder(connectionID, animasyon_kimde_oynayacak, anim_Adi, akiflik);

            buffer.Dispose();

        }

        public static void HandleCordinatBilgileriniGonder(int connectionID ,byte[] data)
        {

            Kaan_ByteBuffer buffer = new Kaan_ByteBuffer();
            buffer.Bytes_Yaz(data);
            int packetID = buffer.Int_Oku();

            //Oyuncu Pozisyonları
            float posx = buffer.Float_Oku();
            float posy = buffer.Float_Oku();
            float posz = buffer.Float_Oku();

            //Oyuncu Rotasyonlarımız
            float rotx = buffer.Float_Oku();
            float roty = buffer.Float_Oku();
            float rotz = buffer.Float_Oku();
            float rotw = buffer.Float_Oku();

            Sabitler.bagli_client[connectionID].xCord = posx;
            Sabitler.bagli_client[connectionID].yCord = posy;
            Sabitler.bagli_client[connectionID].zCord = posz;
            DataSender.SendKordinatBilgileriniGonder(connectionID,posx,posy,posz,rotx,roty,rotz,rotw);
            buffer.Dispose();

        }

        public static void HandleLoginGiris(int connectionID, byte[] data)
        {
            Kaan_ByteBuffer buffer = new Kaan_ByteBuffer();
            buffer.Bytes_Yaz(data);
            int packetID = buffer.Int_Oku();
            string kullanici_adi = buffer.String_Oku();
            string sifre = buffer.String_Oku();

            ArrayList Ara = new ArrayList();
            Ara.Add("kullanici_adi='" + kullanici_adi + "'");
            Ara.Add("sifre='" + sifre + "'");

            MySqlCommand cmd = Sabitler.Mysql_Data.MySql_Veri_Cek(Sabitler.bagli_client[connectionID].baglanti, null, "Select * From tum_kullanicilar", Ara);

            if(cmd != null)
            {
                MySqlDataReader oku = cmd.ExecuteReader();

                int id = 0;
                while (oku.Read())
                {
                    //////
                    id = (int)oku["id"];
                    Yazi.Log_yaz("Bağlanan Kullanıcın Kullancı ID si : " + id.ToString());
                }
                oku.Close();

                ///
                DataSender.SendLoginGirisCevap(connectionID, 1);

            }
            else
            {
                DataSender.SendLoginGirisCevap(connectionID, 0);
            }
            buffer.Dispose();

        }

    }
}
