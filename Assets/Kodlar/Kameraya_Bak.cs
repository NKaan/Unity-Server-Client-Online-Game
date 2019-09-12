using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kameraya_Bak : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject cm;
    void Start()
    {
        cm = Global.global.myCam;
    }

    // Update is called once per frame
    void Update()
    {

        transform.rotation = cm.transform.rotation;
        transform.GetComponent<TextMesh>().fontSize = Mathf.Clamp(
            Mathf.RoundToInt(
                Vector3.Distance(transform.position,cm.transform.position)),12,60) + 3;
    }
}
