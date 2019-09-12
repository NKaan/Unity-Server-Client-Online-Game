using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraKontrol : MonoBehaviour
{
    public Transform hedef;
    public Vector3 mesafe;
    private Vector3 newPos;
    private float mevcutZoom = 10f;
    private float RotasyonHizi = 5.0f;
    int max_k_bilgi_size = 30, min_k_bilgi_size = 12, def_k_bilgi_size = 20;


    private void LateUpdate()
    {
        Zoom();
        CameraTakip();
    }

    private void Start()
    {
        
    }

    void CameraTakip()
    {
        if (Input.GetMouseButton(1))
        {
            Quaternion camTurnAngle =
                Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotasyonHizi, Vector3.up);

            mesafe = camTurnAngle * mesafe;
        }

        newPos = hedef.position - (mesafe * mevcutZoom);
        transform.position = Vector3.Slerp(transform.position, newPos, 1);
        transform.LookAt(hedef.position + Vector3.up);
    }

    void Zoom()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            def_k_bilgi_size -= 1;
            mevcutZoom -= 0.5f;
        }else if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            mevcutZoom += 0.5f;
            def_k_bilgi_size += 1;
        }

        mevcutZoom = Mathf.Clamp(mevcutZoom, 2f, 18f);
    }

}
