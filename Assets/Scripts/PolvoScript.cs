using UnityEngine;

public class PolvoScript : MonoBehaviour
{
    public GameObject Bala;
    public int Contador;
    
    public void Atirar()
    {
        GameObject X = Instantiate(Bala, new Vector3(transform.position.x + transform.localScale.x * 0.77f, transform.position.y - 0.05f, transform.position.z), transform.rotation);
    }

    public void Verificar()
    {
        if (Contador > 4)
        {
            gameObject.SetActive(false);
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Contador++;
            GetComponent<Animator>().SetTrigger("Atingido");
            GetComponent<Animator>().SetInteger("Contador", Contador);
        }
    }
}
