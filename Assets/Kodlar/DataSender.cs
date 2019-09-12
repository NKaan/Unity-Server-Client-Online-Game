using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ClientPackes
{
    CMerhabaServer = 1,
    CKordinatBilgilerimiGonder = 2,
    CLogin_Giris = 3,
    CAnim_Gonder = 4,
}


public class DataSender : MonoBehaviour
{

    public static void SendMerhabaServer()
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.Int_Yaz((int)ClientPackes.CMerhabaServer);
        buffer.String_Yaz("Huuuu ben geldimmmm");
        ClientTCP.SendData(buffer.ToArray());
        buffer.Dispose();
    }

    public static void SendHaraketPosGonder(float xCor,float yCor,float zCor,float xRot,float yRot,float zRot,float wRot)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.Int_Yaz((int)ClientPackes.CKordinatBilgilerimiGonder);
        buffer.Float_Yaz(xCor);
        buffer.Float_Yaz(yCor);
        buffer.Float_Yaz(zCor);

        buffer.Float_Yaz(xRot);
        buffer.Float_Yaz(yRot);
        buffer.Float_Yaz(zRot);
        buffer.Float_Yaz(wRot);

        ClientTCP.SendData(buffer.ToArray());
        buffer.Dispose();

    }

    public static void SendLoginGiris(string kullanici_adi,string sifre)
    {

        ByteBuffer buffer = new ByteBuffer();
        buffer.Int_Yaz((int)ClientPackes.CLogin_Giris);
        buffer.String_Yaz(kullanici_adi);
        buffer.String_Yaz(sifre);
        ClientTCP.SendData(buffer.ToArray());
        buffer.Dispose();
    }

    public static void SendAnimGonder(string anim_adi , int aktif = 0, string animasyon_kimde_oynayacak = "0")
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.Int_Yaz((int)ClientPackes.CAnim_Gonder);
        buffer.String_Yaz(animasyon_kimde_oynayacak);
        buffer.String_Yaz(anim_adi);
        buffer.Int_Yaz(aktif);
        ClientTCP.SendData(buffer.ToArray());
        buffer.Dispose();
    }

   
}
