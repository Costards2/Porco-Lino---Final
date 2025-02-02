using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoedaScript : MonoBehaviour
{
    public GameObject TextoMaisUm;

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Rigidbody2D>().linearVelocity.y < 0)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
    }

    private void OnTriggerEnter2D(Collider2D ObjetoQueColidiu)
    {
        if (ObjetoQueColidiu.CompareTag("Player"))
        {            
            GameObject TextoMaisUmTemp = Instantiate(TextoMaisUm, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
    }
}
