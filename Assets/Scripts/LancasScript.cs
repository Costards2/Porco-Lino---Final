using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancasScript : MonoBehaviour
{
    private PlayerScript PlayerScript;

    // Start is called before the first frame update
    void Start()
    {
        PlayerScript = FindAnyObjectByType<PlayerScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerScript.HitPlayer();
        }
    }
}
