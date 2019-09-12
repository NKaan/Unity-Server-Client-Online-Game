using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login_Panel : MonoBehaviour
{

    public Text kullanici_adi;
    public Text sifre;
    public Button giris_butonu;

    public void Oyuna_Giris()
    {
        giris_butonu.enabled = false;


        DataSender.SendLoginGiris(kullanici_adi.text, sifre.text);

        
    }


}
