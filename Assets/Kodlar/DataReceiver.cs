using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


public class DataReceiver : MonoBehaviour
{

    public static void HandleHosGeldinMesaji(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.Bytes_Yaz(data);
        int packetID = buffer.Int_Oku();
        int connectionID = buffer.Int_Oku();
        string msg = buffer.String_Oku();
        float xCord = buffer.Float_Oku();
        float yCord = buffer.Float_Oku();
        float zCord = buffer.Float_Oku();
       
        Global.global.myConnectionID = connectionID;
        GameObject _PlayerPref;
        _PlayerPref = Instantiate(Global.global.Sura, new Vector3(xCord, yCord, zCord), Quaternion.identity);
        _PlayerPref.name = "Player : " + connectionID.ToString();

        Player newPlayer = _PlayerPref.GetComponent<Player>();
        Kontrol karaktere_kontrol_ekle = _PlayerPref.AddComponent<Kontrol>() as Kontrol;
        karaktere_kontrol_ekle.anim = _PlayerPref.GetComponent<Animator>();
        karaktere_kontrol_ekle.cam = _PlayerPref.GetComponentInChildren<Camera>();
        karaktere_kontrol_ekle.bakma_hizi = 1;
        karaktere_kontrol_ekle.hiz = 1;
        _PlayerPref.GetComponent<Kontrol>().cam.name = "Camera : " + Global.global.myConnectionID;
        _PlayerPref.GetComponent<Kontrol>().cam.enabled = true;
        Global.global.myCam = (GameObject)_PlayerPref.transform.Find("Camera : " + Global.global.myConnectionID).gameObject;      
        Global.global.myKarakter = _PlayerPref;
        newPlayer.Karakter = _PlayerPref;

        buffer.Dispose();
        Debug.Log("Port Numaran : " + connectionID.ToString() + " Mesajin : " + msg);
        

    }

    public static void HandleAnimAl(byte[] data)
    {

        ByteBuffer buffer = new ByteBuffer();
        buffer.Bytes_Yaz(data);
        int packetID = buffer.Int_Oku();
        int connectionID = buffer.Int_Oku();
        string animasyon_kimde_oynayacak = buffer.String_Oku();
        string anim_Adi = buffer.String_Oku();
        int aktiflik = buffer.Int_Oku();
        buffer.Dispose();
        Animator animator;
        Debug.Log(animasyon_kimde_oynayacak);

        if(animasyon_kimde_oynayacak == "0")
        {
            animator = Global.global.Tum_Oyuncular[connectionID].Karakter.GetComponent<Animator>();
        }
        else
        {
            animator = GameObject.Find(animasyon_kimde_oynayacak).GetComponent<Animator>();
        }

        AnimasyonKontrol.Anim_Yurut(anim_Adi, animator, aktiflik, animasyon_kimde_oynayacak, true);

    }

    public static void HandleOyundakiler(byte[] data)
    {
        GameObject _PlayerPref;

        ByteBuffer buffer = new ByteBuffer();
        buffer.Bytes_Yaz(data);
        int packetID = buffer.Int_Oku();
        int connectionID = buffer.Int_Oku();
        float xCord = buffer.Float_Oku();
        float yCord = buffer.Float_Oku();
        float zCord = buffer.Float_Oku();

        _PlayerPref = Instantiate(Global.global.Sura, new Vector3(xCord, yCord, zCord), Quaternion.identity);
        _PlayerPref.name = "Player : " + connectionID.ToString();
        _PlayerPref.GetComponent<Player>().connectionID = connectionID;
        _PlayerPref.GetComponent<Player>().Karakter = _PlayerPref;
        _PlayerPref.GetComponentInChildren<Camera>().enabled = false;
        Global.global.Tum_Oyuncular.Add(connectionID, _PlayerPref.GetComponent<Player>());
        buffer.Dispose();
    }

    public static void HandleOyuncuGirisYapti(byte[] data)
    {
        GameObject _PlayerPref;

        ByteBuffer buffer = new ByteBuffer();
        buffer.Bytes_Yaz(data);
        int packetID = buffer.Int_Oku();
        int connectionID = buffer.Int_Oku();
        float xCord = buffer.Float_Oku();
        float yCord = buffer.Float_Oku();
        float zCord = buffer.Float_Oku();
        _PlayerPref = Instantiate(Global.global.Sura, new Vector3(xCord, yCord, zCord), Quaternion.identity);
        _PlayerPref.name = "Player : " + connectionID.ToString();
        _PlayerPref.GetComponent<Player>().connectionID = connectionID;
        _PlayerPref.GetComponentInChildren<Camera>().enabled = false;
        _PlayerPref.GetComponent<Player>().Karakter = _PlayerPref;
        Global.global.Tum_Oyuncular.Add(connectionID, _PlayerPref.GetComponent<Player>());


        buffer.Dispose();
    }

    public static void HandleOyuncuKordinatBilgileri(byte[] data)
    {
      
        ByteBuffer buffer = new ByteBuffer();
        buffer.Bytes_Yaz(data);
        int packetID = buffer.Int_Oku();
        int connectionID = buffer.Int_Oku();
        float xCord = buffer.Float_Oku();
        float yCord = buffer.Float_Oku();
        float zCord = buffer.Float_Oku();

        float xRot = buffer.Float_Oku();
        float yRot = buffer.Float_Oku();
        float zRot = buffer.Float_Oku();
        float wRot = buffer.Float_Oku();

        GameObject _Diger_Oyuncu;
        if(_Diger_Oyuncu = GameObject.Find("Player : " + connectionID.ToString()))
        {

            _Diger_Oyuncu.GetComponent<Animator>().SetBool("Kos", true);

            _Diger_Oyuncu.transform.rotation =
                Quaternion.Slerp(_Diger_Oyuncu.transform.rotation,
                new Quaternion(xRot, yRot, zRot, wRot), 10f * Time.deltaTime);

            _Diger_Oyuncu.transform.position =
                Vector3.MoveTowards(_Diger_Oyuncu.transform.position, new Vector3(xCord, yCord, zCord),
                1f * Time.deltaTime);
        }

       
        buffer.Dispose();
    }

    public static void HandleOyuncuOyundanCikti(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.Bytes_Yaz(data);
        int packetID = buffer.Int_Oku();
        int connectionID = buffer.Int_Oku();
        buffer.Dispose();
        Destroy(GameObject.Find("Player : " + connectionID.ToString()));
    }

    public static void HandleLoginGirisCevap(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.Bytes_Yaz(data);
        int packetID = buffer.Int_Oku();
        int cevap = buffer.Int_Oku();
        if(cevap == 1)
        {
            //DataSender.SendMerhabaServer();
            Loading_Panel.loading_Panel.LoadLevel(1);
            Global.global.Login_Paneli.SetActive(false);
            Debug.Log("Giriş Başarılı");
           
        }
        else if(cevap == 0)
        {
            Debug.Log("Giriş Başarısız");
        }

        Global.global.Login_Paneli.GetComponent<Login_Panel>().giris_butonu.enabled = true;
    }

}
