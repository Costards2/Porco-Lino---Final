using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ControladorGameOver : MonoBehaviour
{
    public VideoPlayer GameOver;
    private ControladorPrincipalScript ControladorPrincipalScript;
    private PlayerScript PlayerScript;
    private Color CorPlayer;

    // Start is called before the first frame update
    void Start()
    {
        ControladorPrincipalScript = FindAnyObjectByType<ControladorPrincipalScript>();
        PlayerScript = FindAnyObjectByType<PlayerScript>();
        PlayerScript.Musica.Stop();
        CorPlayer = PlayerScript.gameObject.GetComponent<SpriteRenderer>().color;
        CorPlayer.a = 0;
        PlayerScript.gameObject.GetComponent<SpriteRenderer>().color = CorPlayer;
        ControladorPrincipalScript.HUD.SetActive(false);
        StartCoroutine("CarregarCena");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator CarregarCena()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("PorcoLino");        
    }
}
