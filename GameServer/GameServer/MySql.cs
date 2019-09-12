using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class MySql
    {
        public MySqlConnection MySqlBaslat()
        {
            try
            {
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                builder.UserID = "root";
                builder.Password = "";
                builder.Database = "youtubeegitim";
                builder.Server = "localhost";
                builder.Pooling = true;
                builder.ConnectionLifeTime = 0;
                builder.ConnectionTimeout = 30;

                string connString = builder.ToString();

                MySqlConnection baglanti = new MySqlConnection(connString);

                if (baglanti.State != ConnectionState.Open)
                {
                    baglanti.Open();
                    Yazi.Log_yaz("Mysql Connection Acildi");
                    return baglanti;
                }

                return null;

            }
            catch (Exception e)
            {
                Yazi.Hata_Yaz("MySql Bağlantısı Hatası : " + e.Message);
                return null;

            }

        }


        public DataTable MySql_Tablo_Cek(MySqlConnection baglanti, String SqlCommand)
        {
            MySqlDataAdapter listele = new MySqlDataAdapter(SqlCommand, baglanti);
            DataTable oku = new DataTable();
            listele.Fill(oku);

            return oku;
        }

        public int MySqlVeri_Sil(MySqlConnection baglanti, String SqlCommand, String Select = null, ArrayList WhereList = null)
        {
            String sol, sag;
            try
            {
                if (SqlCommand == null)
                {
                    SqlCommand = Select;

                    SqlCommand = SqlCommand + " WHERE ";

                    for (int i = 0; i < WhereList.Count; i++)
                    {
                        sol = WhereList[i].ToString().Split('=')[0];
                        sag = WhereList[i].ToString().Split('=')[1];

                        if (WhereList.Count == 1)
                        {
                            SqlCommand = SqlCommand + sol + "=" + "'" + sag + "'";

                        }
                        else if (i < WhereList.Count - 1)
                        {
                            SqlCommand = SqlCommand + sol + "=" + "'" + sag + "'" + " AND ";
                        }
                        else if (i == WhereList.Count - 1)
                        {
                            SqlCommand = SqlCommand + sol + "=" + "'" + sag + "'" + "";
                        }

                    }

                }
                else
                {

                }

                Console.WriteLine(SqlCommand);

                MySqlCommand guncelle = null;
                guncelle = new MySqlCommand(SqlCommand, baglanti);

                if (guncelle != null)
                {

                    int id = Convert.ToInt32(guncelle.ExecuteNonQuery());
                    return id;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception e)
            {
                Yazi.Hata_Yaz("MySql Hatası : " + e.Message);
                baglanti.Close();
                throw;
            }
        }

        public int MySqlVeri_Guncelle(MySqlConnection baglanti, String SqlCommand = null, String Select = null, ArrayList GuncellenecekList = null, ArrayList WhereList = null)
        {
            String sol, sag;
            try
            {
                if (SqlCommand == null)
                {
                    SqlCommand = Select + " SET ";


                    for (int i = 0; i < GuncellenecekList.Count; i++)
                    {

                        sol = GuncellenecekList[i].ToString().Split('=')[0];
                        sag = GuncellenecekList[i].ToString().Split('=')[1];

                        if (GuncellenecekList.Count == 1)
                        {
                            SqlCommand = SqlCommand + sol + "=" + "'" + sag + "'";

                        }
                        else if (i < GuncellenecekList.Count - 1)
                        {
                            SqlCommand = SqlCommand + sol + "=" + "'" + sag + "'" + " , ";
                        }
                        else if (i == GuncellenecekList.Count - 1)
                        {
                            SqlCommand = SqlCommand + sol + "=" + "'" + sag + "'" + "";
                        }

                    }

                    SqlCommand = SqlCommand + " WHERE ";

                    for (int i = 0; i < WhereList.Count; i++)
                    {
                        sol = WhereList[i].ToString().Split('=')[0];
                        sag = WhereList[i].ToString().Split('=')[1];

                        if (WhereList.Count == 1)
                        {
                            SqlCommand = SqlCommand + sol + "=" + "'" + sag + "'";

                        }
                        else if (i < WhereList.Count - 1)
                        {
                            SqlCommand = SqlCommand + sol + "=" + "'" + sag + "'" + " AND ";
                        }
                        else if (i == WhereList.Count - 1)
                        {
                            SqlCommand = SqlCommand + sol + "=" + "'" + sag + "'" + "";
                        }

                    }

                }
                else
                {

                }

                Console.WriteLine(SqlCommand);

                MySqlCommand guncelle = null;
                guncelle = new MySqlCommand(SqlCommand, baglanti);

                if (guncelle != null)
                {

                    int id = Convert.ToInt32(guncelle.ExecuteNonQuery());

                    return id;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception e)
            {
                Yazi.Hata_Yaz("MySql Hatası : " + e.Message);
                baglanti.Close();
                throw;
            }

        }

        public MySqlCommand MySql_Veri_Cek(MySqlConnection baglanti, String SqlCommand = null, String Select = null, ArrayList WhereList = null)
        {
            try
            {
                ArrayList tablo_Adi = new ArrayList();
                ArrayList eklenen_veri = new ArrayList();

                if (SqlCommand == null)
                {
                    SqlCommand = Select;

                    if (WhereList != null)
                    {
                        SqlCommand = SqlCommand + " WHERE (";

                        for (int i = 0; i < WhereList.Count; i++)
                        {
                            if (WhereList.Count == 1)
                            {
                                SqlCommand = SqlCommand + WhereList[i].ToString() + ");";

                            }
                            else if (i < WhereList.Count - 1)
                            {
                                SqlCommand = SqlCommand + WhereList[i].ToString() + " AND ";
                            }
                            else if (i == WhereList.Count - 1)
                            {
                                SqlCommand = SqlCommand + WhereList[i].ToString() + ");";
                            }

                        }
                    }

                }

                Console.WriteLine(SqlCommand);
                MySqlCommand cmd = new MySqlCommand(SqlCommand, baglanti);
                int Count = Convert.ToInt32(cmd.ExecuteScalar());

                Console.WriteLine("Karakter Sayısı : " + Count);

                if (Count != 0)
                {

                    return cmd;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                Yazi.Hata_Yaz("MySql Hatası : " + e.Message);
                baglanti.Close();
                throw;
            }

        }

        public int MySql_Veri_Kaydet(MySqlConnection baglanti, String tablo_adi, ArrayList Eklenecek_Veri)
        {
            string SqlCommand = "Insert Into " + tablo_adi + " (";

            try
            {
                ArrayList tablo_Adi = new ArrayList();
                ArrayList eklenen_veri = new ArrayList();

                for (int i = 0; i < Eklenecek_Veri.Count; i++)
                {
                    tablo_Adi.Add(Eklenecek_Veri[i].ToString().Split(',')[0]);
                    eklenen_veri.Add(Eklenecek_Veri[i].ToString().Split(',')[1]);
                }

                for (int i = 0; i < Eklenecek_Veri.Count; i++)
                {
                    if (i < Eklenecek_Veri.Count - 1)
                    {
                        SqlCommand = SqlCommand + tablo_Adi[i].ToString() + ", ";
                    }
                    else if (i == Eklenecek_Veri.Count - 1)
                    {
                        SqlCommand = SqlCommand + tablo_Adi[i].ToString() + ") VALUES ('";
                    }

                }

                for (int i = 0; i < Eklenecek_Veri.Count; i++)
                {
                    if (Eklenecek_Veri.Count == 1)
                    {
                        SqlCommand = SqlCommand + eklenen_veri[i].ToString() + "'); " +
                            "SELECT LAST_INSERT_ID();";
                    }
                    else if (i < Eklenecek_Veri.Count - 1)
                    {
                        SqlCommand = SqlCommand + eklenen_veri[i].ToString() + "', '";
                    }
                    else if (i == Eklenecek_Veri.Count - 1)
                    {
                        SqlCommand = SqlCommand + eklenen_veri[i].ToString() + "'); " +
                            "SELECT LAST_INSERT_ID();";
                    }

                }

                MySqlCommand ekle = null;
                ekle = new MySqlCommand(SqlCommand, baglanti);

                if (ekle != null)
                {

                    int id = Convert.ToInt32(ekle.ExecuteScalar());
                    return id;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception e)
            {
                Yazi.Hata_Yaz("MySql Hatası : " + e.Message);
                baglanti.Close();
                throw;
            }

        }


        public int MySql_Veri_Varmi_Kontrol(MySqlConnection baglanti, String SqlCommand = null, String Select = null, ArrayList WhereList = null)
        {
            try
            {
                ArrayList tablo_Adi = new ArrayList();
                ArrayList eklenen_veri = new ArrayList();

                if (SqlCommand == null)
                {
                    SqlCommand = Select;

                    if (WhereList != null)
                    {
                        SqlCommand = SqlCommand + " WHERE (";

                        for (int i = 0; i < WhereList.Count; i++)
                        {
                            if (WhereList.Count == 1)
                            {
                                SqlCommand = SqlCommand + WhereList[i].ToString() + ");";

                            }
                            else if (i < WhereList.Count - 1)
                            {
                                SqlCommand = SqlCommand + WhereList[i].ToString() + " AND ";
                            }
                            else if (i == WhereList.Count - 1)
                            {
                                SqlCommand = SqlCommand + WhereList[i].ToString() + ");";
                            }

                        }
                    }

                }

                Console.WriteLine(SqlCommand);
                MySqlCommand cmd = new MySqlCommand(SqlCommand, baglanti);
                int Count = Convert.ToInt32(cmd.ExecuteScalar());

                if (Count == 1)
                {
                    MySqlDataReader oku = cmd.ExecuteReader();
                    return Convert.ToInt32(oku["id"]);

                }
                else
                {
                    return Count;
                }

            }
            catch (Exception e)
            {
                Yazi.Hata_Yaz("MySql Hatası : " + e.Message);
                baglanti.Close();
                throw;
            }

        }

    }

   
}
