using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class Client
    {

        public Oda oda;

        public TcpClient socket;
        public NetworkStream stream;
        private byte[] recBuffer;
        public Kaan_ByteBuffer buffer;


        // Oyuncu Bilgileri;
        public int connectionID;
        public bool oyunda_mi = false;
        public float xCord = 5;
        public float yCord = 0;
        public float zCord = 8;


        private MySqlConnection baglanti_;
        public MySqlConnection baglanti
        {
            get
            {
                baglanti_control(baglanti_);
                return baglanti_;
            }
            set
            {
                baglanti_ = value;
            }
        }

        public void baglanti_control(MySqlConnection baglanti)
        {
            if(baglanti.State != ConnectionState.Open)
            {
                baglanti.Open();
                Yazi.Log_yaz(connectionID.ToString() + " Kullanıcısının Bağlantısı Tekrar Açıldı");
            }
        }


        public void Start()
        {
            socket.SendBufferSize = 4096;
            socket.ReceiveBufferSize = 4096;
            stream = socket.GetStream();
            recBuffer = new byte[4096];
            stream.BeginRead(recBuffer, 0, socket.ReceiveBufferSize, OnReceiveData, null);
            Sabitler.oyuncu_baglandi(connectionID.ToString());

        }

        private void OnReceiveData(IAsyncResult result)
        {
            try
            {

                int length = stream.EndRead(result);
                if(length <= 0)
                {
                    CloseConnection();
                    return;
                }

                byte[] newBytes = new byte[length];
                Array.Copy(recBuffer, newBytes, length);
                ServerHandleData.HandleData(connectionID, newBytes);
                stream.BeginRead(recBuffer, 0, socket.ReceiveBufferSize, OnReceiveData, null);


            }
            catch (Exception)
            {
                CloseConnection();
                return;
            }
        }

        private void CloseConnection()
        {
            Sabitler.bagli_client.Remove(connectionID);
            Sabitler.oyuncu_cikti(connectionID.ToString());
            DataSender.SendOyuncuOyundanCikti(connectionID);
            socket.Close();
        }

    }
}
