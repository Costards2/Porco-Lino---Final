using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextoMaisUmScript : MonoBehaviour
{
    private PlayerScript PlayerScript;
    public AudioSource SomMoeda;

    // Start is called before the first frame update
    void Start()
    {
        PlayerScript = FindAnyObjectByType<PlayerScript>();

        if (PlayerScript.SentidoPlayer == Vector2.right)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(100,200), Random.Range(300,400)));
        }
        else
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, -200), Random.Range(300, 400)));
        }
        SomMoeda.volume = PlayerScript.Volume;
        SomMoeda.Play();
        PlayerScript.Moedas++;
        PlayerScript.DisplayMoedas.text = PlayerScript.Moedas.ToString();
        Destroy(gameObject, 0.8f);
    }
}
