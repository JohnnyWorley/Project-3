using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeartFlash : MonoBehaviour
{

    private SpriteRenderer sr;
    Color hex1;
    Color hex2;
    // Start is called before the first frame update
    void Start()
    {
       sr = GetComponent<SpriteRenderer>();
       hex1 = sr.color;
       hex2 = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        var pingPong = Mathf.PingPong(Time.time, 1);
        sr.color = Color.Lerp(hex1, hex2, pingPong);
    }
}
