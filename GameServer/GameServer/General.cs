using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class General
    {

        public static void Sunucuyu_Baslat()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ServerTCP.InitializeNetwork();
            Yazi.Log_yaz("Sunucu Başlatıldı");

            Sabitler.Sunucu_MySql_Baglanti = Sabitler.Mysql_Data.MySqlBaslat();
            Yazi.Log_yaz("MySQL Sunucu Başlatıldı");

            sw.Stop();
            Yazi.Log_yaz("Sunucu Başlama Süresi : " + sw.ElapsedMilliseconds.ToString() + "ms");
        }

    }
}
