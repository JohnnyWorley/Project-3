using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossActivation : MonoBehaviour
{
    public GameObject minotaur; 
    public GameObject blocker;
    public Canvas canvas;
    private void Start()
    {
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        blocker.transform.position -= new Vector3(0, 10, 0);
        minotaur.GetComponent<Enemy>().enabled = true;
        minotaur.GetComponent<MinotaurBoss>().enabled = true;
        minotaur.GetComponent <MinotaurBoss>().begun = true;
        canvas.gameObject.SetActive(true);
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
