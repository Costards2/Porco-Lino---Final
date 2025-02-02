using UnityEngine;

public class BalaScript : MonoBehaviour
{
    public float Velocidade;
    private PlayerScript PlayerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerScript = FindAnyObjectByType<PlayerScript>();
        GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.localScale.x * Velocidade, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Desativar()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<Animator>().SetTrigger("Impacto");
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        if (collision.CompareTag("Player"))
        {
            PlayerScript.HitPlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<Animator>().SetTrigger("Impacto");
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }
}
