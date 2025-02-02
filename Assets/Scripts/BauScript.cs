using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BauScript : MonoBehaviour
{
    public GameObject Moeda;
    public bool BauAberto;
    private Animator BauAnimator;
    public AudioSource SomMoeda;
    public AudioSource SomBau;
    private PlayerScript PlayerScript;

    // Start is called before the first frame update
    void Start()
    {
        BauAnimator = GetComponent<Animator>();
        PlayerScript = FindAnyObjectByType<PlayerScript>();
    }

    public void Interagir()
    {
        SomBau.volume = PlayerScript.Volume;
        SomBau.Play();
        BauAnimator.SetTrigger("Ativar");
        BauAberto = true;
    }

    IEnumerator GerarMoedas()
    {
        SomMoeda.volume = PlayerScript.Volume;
        int TotalMoedas = Random.Range(5, 20);
        for (int i = 0; i < TotalMoedas; i++)
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

    public void RetirarColisor()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<Collider2D>().enabled = false;
    }

    public void ReativarColisor()
    {
        GetComponent<Collider2D>().enabled = true;
    }

}
