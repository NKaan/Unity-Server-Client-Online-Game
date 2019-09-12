using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kontrol : MonoBehaviour
{

    public Camera cam;
    public Animator anim;
    RaycastHit rayHit;
    Vector3 targerPosition;
    Vector3 lookAtTarget;
    Quaternion playerRot;
    public float hiz = 1f;
    public float bakma_hizi = 1f;
    bool moving = false;

    void Update()
    {
       
        if (Input.GetMouseButton(0))
        {
           
            SetTargetPosition();
        }

        if (moving)
        {
            Move();
        }

    }


    void SetTargetPosition()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out rayHit, 100))
        {
            targerPosition = rayHit.point;


            if (Vector3.Distance(transform.position, targerPosition) < 1.2f)
            {
                return;
            }

            lookAtTarget = new Vector3(targerPosition.x - transform.position.x,
                transform.position.y - transform.position.y,
                targerPosition.z - transform.position.z);
            playerRot = Quaternion.LookRotation(lookAtTarget);
            moving = true;
        }
    }

    void Move()
    {
        AnimasyonKontrol.Anim_Yurut("Kos", null, 1);
        transform.rotation = Quaternion.Slerp(transform.rotation, playerRot, bakma_hizi - Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, targerPosition, hiz * Time.deltaTime);

        DataSender.SendHaraketPosGonder(
            (float)Math.Round(transform.position.x, 2),
            (float)Math.Round(transform.position.y, 2),
            (float)Math.Round(transform.position.z, 2),
            (float)Math.Round(transform.rotation.x, 2),
            (float)Math.Round(transform.rotation.y, 2),
            (float)Math.Round(transform.rotation.z, 2),
            (float)Math.Round(transform.rotation.w, 2));

        if(Vector3.Distance(transform.position,targerPosition) < 1f)
        {
            AnimasyonKontrol.Anim_Yurut("Kos", null, 0);
            moving = false;
        }
    }

}
