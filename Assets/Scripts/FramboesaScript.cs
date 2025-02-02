using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramboesaScript : MonoBehaviour
{
    private PlayerScript PlayerScript;
    public AudioSource SomFramboesa;

    // Start is called before the first frame update
    void Start()
    {
        PlayerScript = FindAnyObjectByType<PlayerScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerScript.Energia = PlayerScript.Energia + 100;
            PlayerScript.DisplayEnergia.text = PlayerScript.Energia.ToString();
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine("AtivarAudio");            
        }        
    }
    IEnumerator AtivarAudio()
    {
        SomFramboesa.volume = PlayerScript.Volume;
        SomFramboesa.Play();
        while (SomFramboesa.isPlaying) yield return null;
        gameObject.SetActive(false);
    }
}
