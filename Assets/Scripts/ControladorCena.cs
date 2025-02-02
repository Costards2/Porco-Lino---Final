using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorCena : MonoBehaviour
{
    private ControladorPrincipalScript ControladorPrincipalScript;
    private Color CorPlayer;

    // Start is called before the first frame update
    void Start()
    {
        ControladorPrincipalScript = FindAnyObjectByType<ControladorPrincipalScript>();        
        CorPlayer = ControladorPrincipalScript.Player.GetComponent<SpriteRenderer>().color;
        CorPlayer.a = 0;
        ControladorPrincipalScript.Player.GetComponent<Transform>().position = Vector3.zero;
        ControladorPrincipalScript.Player.GetComponent<Transform>().localScale = Vector3.one;
        ControladorPrincipalScript.Painel.SetActive(true);
        StartCoroutine("AtivarObjetos");
    }

    IEnumerator AtivarObjetos()
    {
        yield return new WaitForSeconds(0.01f); 
        CorPlayer.a = 1;
        ControladorPrincipalScript.Player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        ControladorPrincipalScript.Player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        ControladorPrincipalScript.Player.GetComponent<SpriteRenderer>().color = CorPlayer;
        ControladorPrincipalScript.Player.GetComponent<PlayerScript>().Musica.Play();
        ControladorPrincipalScript.HUD.SetActive(true);
        ControladorPrincipalScript.Player.GetComponent<PlayerScript>().Start();
        ControladorPrincipalScript.Player.GetComponent<PlayerScript>().CarregandoCena = false;
    }
}
