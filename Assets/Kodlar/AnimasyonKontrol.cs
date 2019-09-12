using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimasyonKontrol : MonoBehaviour
{
   
    public static void Anim_Yurut(string Anim_Adi,Animator animator = null,int aktiflik = 0,string animasyon_kimde_oynayacak = "0",bool disardan_gelen = false)
    {

        if (!Global.global.Gonderilen_Anim_Kontrol.ContainsKey(Anim_Adi))
        {
            Global.global.Gonderilen_Anim_Kontrol.Add(Anim_Adi, aktiflik);
        }
        else
        {
            if (Global.global.Gonderilen_Anim_Kontrol[Anim_Adi] == aktiflik)
                return;
            Global.global.Gonderilen_Anim_Kontrol[Anim_Adi] = aktiflik;
        }

        if (animator == null)
            animator = Global.global.myKarakter.GetComponent<Animator>();

        if(aktiflik == 0)
        {
            animator.SetBool(Anim_Adi, false);
        }
        else
        {
            animator.SetBool(Anim_Adi, true);
        }

        if (!disardan_gelen)
            DataSender.SendAnimGonder(Anim_Adi, aktiflik, animasyon_kimde_oynayacak);

    }


}
