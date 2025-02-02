using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ControladorPrincipalScript : MonoBehaviour
{
    public GameObject ObjetosTodasAsTelas, Player, HUD, Painel;
    public VideoPlayer Abertura;
    private Color CorPlayer;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(ObjetosTodasAsTelas);
        print(Application.persistentDataPath);
        CorPlayer = Player.GetComponent<SpriteRenderer>().color;
        CorPlayer.a = 0;
        Player.GetComponent<SpriteRenderer>().color = CorPlayer;
        Player.SetActive(true);
        StartCoroutine("CarregarCena");
    }

    IEnumerator CarregarCena()
    {
        yield return new WaitForSeconds(5);
        Abertura.enabled = false;
        SceneManager.LoadScene("PorcoLino");
    }
}
