using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BarrilScript : MonoBehaviour
{
    public GameObject Moeda;
    public GameObject Barril;
    public int Contador;
    private PlayerScript PlayerScript;
    public AudioSource SomMoeda;
    public AudioSource SomBarril;

    // Start is called before the first frame update
    void Start()
    {
        PlayerScript = FindAnyObjectByType<PlayerScript>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (PlayerScript.PlayerRigidbody.linearVelocity.y > 0)
            {
                PlayerScript.PlayerRigidbody.AddForce(new Vector2(0, -300));
            }
            else
            {
                PlayerScript.PlayerRigidbody.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            }

            Contador++;
            if (Contador > 5)
            {
                return;
            }

            GetComponent<Animator>().SetTrigger("Ativar");
            GetComponent<Animator>().SetInteger("Contador", Contador);
            StartCoroutine("GerarMoedas");
        }
    }

    IEnumerator GerarMoedas()
    {
        SomMoeda.volume = PlayerScript.Volume;
        for (int i = 0; i < 2; i++)
        {
            GameObject MoedaTemp = Instantiate(Moeda, transform.position, transform.rotation);
            if (Random.Range(1, 10) < 6)
            {
                MoedaTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-50, -20), Random.Range(200, 300)));
            }
            else
            {
                MoedaTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(20, 50), Random.Range(200, 300)));
            }
            SomMoeda.Play();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void DesativarBarril()
    {
        Barril.SetActive(false);
    }

    void Interagir()
    {
        StartCoroutine("DestruirBarril");
    }

    IEnumerator DestruirBarril()
    {
        yield return new WaitForSeconds(0.3f);
        GetComponent<Animator>().SetTrigger("Ativar");
        GetComponent<Animator>().SetInteger("Contador", 5);
    }

    public void AtivarSom()
    {
        SomBarril.volume = PlayerScript.Volume;
        SomBarril.Play();
    }
}
