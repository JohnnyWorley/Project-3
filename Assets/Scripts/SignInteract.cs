using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignInteract : MonoBehaviour
{
    public TextMeshProUGUI text;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        text.transform.parent.transform.gameObject.SetActive(true);
        text.fontSize = 22;
        if (gameObject.name == "GrappleHintSign")
        {
            text.text = "Space to Grapple";
        }
        else if (gameObject.name == "AttackHintSign")
        {
            text.text = "Click to Attack";
        }
        else
        {
            text.text = "Escape to Pause";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        text.transform.parent.transform.gameObject.SetActive(false);
        text.fontSize = 34;
    }
}
