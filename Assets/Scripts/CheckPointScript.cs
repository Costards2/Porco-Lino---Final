using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    public bool CheckPointAtivado = false;
    public AudioSource SomSino;
    private PlayerScript PlayerScript;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !CheckPointAtivado)
        {

            PlayerScript = FindAnyObjectByType<PlayerScript>();            
            SomSino.volume = PlayerScript.Volume;
            GetComponent<Animator>().enabled = true; 
            SomSino.Play();
            CheckPointAtivado = true;
            PlayerPrefs.SetFloat("CoordenadaX", transform.position.x);
            PlayerPrefs.SetFloat("CoordenadaY", transform.position.y);
            PlayerPrefs.SetFloat("EscalaX", transform.localScale.x);
        }
    }

    public void DesativarAnimacao()
    {
        GetComponent<Animator>().enabled = false;
    }
}
