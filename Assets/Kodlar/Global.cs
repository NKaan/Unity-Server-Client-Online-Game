using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{

    public static Global global;

    private void Awake()
    {
        global = this;

    }

    [Header("Panaller")]
    public GameObject Login_Paneli;

    [Header("Ortak Bilgiler")]
    public int myConnectionID;
    public GameObject myKarakter;
    public GameObject myCam;

    public Dictionary<string, int> Gonderilen_Anim_Kontrol = new Dictionary<string, int>();
    public Dictionary<int, Player> Tum_Oyuncular = new Dictionary<int, Player>();

    [Header("Karakter Sınıfları")]
    public GameObject Sura;

}
