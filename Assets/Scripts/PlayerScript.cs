using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class PlayerScript : MonoBehaviour
{
    [Header("Sons")]
    public AudioSource Musica;
    public AudioSource SomDano;
    public AudioSource SomPulo;
    public float Volume;
    
    [Header("Controles Animacao")]
    public Animator PlayerAnimator;
    public Rigidbody2D PlayerRigidbody;
    public Transform SensorSuperficie;
    public bool SobreSuperficie;
    private bool PermiteSegundoPulo;
    public LayerMask Plataformas;
    public int IDAnimacao;
    public bool Jump;
    public bool Attack1;
    public bool Attack2;
    public bool Hit;
    public float ForcaPulo;
    public float Velocidade;
    private float h, v;
    private bool EliminandoPlayer;

    [Header("Controle Zoom")]
    public float ZoomMaximo;
    public float ZoomMinimo;
    public float ZoomPadrao;
    public float PassoZoom;
    private float ZoomAtual;
    public float TempoZoom;
    public CinemachineCamera CinemachineCamera;

    [Header("Head-Up Display")]
    public Sprite[] DPADSprites;
    public SpriteRenderer DPADRenderer;
    public Transform Delimitador;
    public float DPAD_x, DPAD_y, Delimitador_x;
    private Touch Toque;
    // public Transform Cursor;
    public float RaioMomentaneo, RaioMinimo, RaioMaximo, x, y;
    private int idFinger;
    private bool BotaoJump, BotaoAttack1, BotaoAttack2;
    private ControladorPrincipalScript ControladorPrincipalScript;

    public int StartMoedas;
    public int StartEnergia;
    public int StartVidas;
    public int Moedas;
    public int Energia;
    public int Vidas;
    public TextMeshProUGUI DisplayMoedas;
    public TextMeshProUGUI DisplayEnergia;
    public TextMeshProUGUI DisplayVidas;

    [Header("Interacao")]
    public Vector2 SentidoPlayer;
    public LayerMask Interacao;
    public Transform SensorMao;

    [Header("Menu")]
    private bool MenuTeclado, BotaoMenu;
    public GameObject Menu;
    public GameObject Pagina1;
    public GameObject Pagina2;
    public GameObject Pagina3;
    public bool CarregandoCena;

    private bool Win;

    // Start is called before the first frame update
    public void Start()
    {
        Win = false;

        ControladorPrincipalScript = FindAnyObjectByType<ControladorPrincipalScript>();    
        
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PermiteSegundoPulo = true;

        DPAD_x = Camera.main.WorldToScreenPoint(DPADRenderer.transform.position).x;
        DPAD_y = Camera.main.WorldToScreenPoint(DPADRenderer.transform.position).y;
        Delimitador_x = Camera.main.WorldToScreenPoint(Delimitador.position).x;
        RaioMaximo = Delimitador_x - DPAD_x;
        RaioMinimo = 0.15f * RaioMaximo;

        Moedas = StartMoedas;
        Energia = StartEnergia;
        Vidas = StartVidas;

        DisplayMoedas.text = Moedas.ToString();
        DisplayEnergia.text = Energia.ToString();
        DisplayVidas.text = Vidas.ToString();

        PlayerPrefs.SetFloat("CoordenadaX", 0);
        PlayerPrefs.SetFloat("CoordenadaY", 0);
        PlayerPrefs.SetFloat("EscalaX", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (EliminandoPlayer || Win)
        {
            return;
        }
        
        DetectarTeclado();

        DetectarTouch();

        if (BotaoMenu || MenuTeclado)
        {
            BotaoMenu = false;
            if (Menu.activeSelf)
            {
                Menu.GetComponent<Animator>().SetTrigger("Fechar");
            }
            else
            {
                Menu.GetComponent<MenuScript>().Barris = GameObject.Find("Barris").GetComponent<Transform>();
                Menu.GetComponent<MenuScript>().Baus = GameObject.Find("Baus").GetComponent<Transform>();
                Menu.GetComponent<MenuScript>().CheckPoints = GameObject.Find("CheckPoints").GetComponent<Transform>();
                Menu.GetComponent<MenuScript>().Coletaveis = GameObject.Find("Coletaveis").GetComponent<Transform>();

                Menu.SetActive(true);
            }
        }

        if (Menu.activeSelf)
        {
            return;
        }

        SobreSuperficie = Physics2D.OverlapCircle(new Vector2(SensorSuperficie.position.x, SensorSuperficie.position.y), 0.1f, Plataformas);
        
        if (Jump || BotaoJump)
        {
            BotaoJump = false;
            SomPulo.volume = Volume;
            if (SobreSuperficie)
            {
                SomPulo.Play();
                PlayerAnimator.SetTrigger("Jump");
                PlayerRigidbody.AddForce(new Vector2(h, ForcaPulo));
                PermiteSegundoPulo = true;
            }
            else if (PermiteSegundoPulo)
            {
                SomPulo.Play();
                PlayerAnimator.SetTrigger("Jump");
                PlayerRigidbody.AddForce(new Vector2(h, ForcaPulo));
                PermiteSegundoPulo = false;
            }
        }

        if (Attack1 || BotaoAttack1)
        {
            BotaoAttack1 = false;
            RaycastHit2D RaioInteracao = Physics2D.Raycast(new Vector2(SensorMao.position.x, SensorMao.position.y), SentidoPlayer, 0.6f, Interacao);
            Debug.DrawRay(new Vector2(SensorMao.position.x, SensorMao.position.y), SentidoPlayer * 0.6f, Color.red);
            RaycastHit2D RaioInteracao2 = Physics2D.Raycast(transform.position, SentidoPlayer, 0.6f, Interacao);
            Debug.DrawRay(transform.position, SentidoPlayer * 0.6f, Color.red); 

            PlayerAnimator.SetTrigger("Attack1");
            if (RaioInteracao)
            {
                RaioInteracao.collider.SendMessage("Interagir", SendMessageOptions.DontRequireReceiver);
            }
            
            if (RaioInteracao2)
            {
                RaioInteracao2.collider.SendMessage("Interagir", SendMessageOptions.DontRequireReceiver);
            }
        }
        
        if (Attack2 || BotaoAttack2)
        {
            BotaoAttack2 = false;
            RaycastHit2D RaioInteracao = Physics2D.Raycast(new Vector2(SensorMao.position.x, SensorMao.position.y), SentidoPlayer, 0.6f, Interacao);
            Debug.DrawRay(new Vector2(SensorMao.position.x, SensorMao.position.y), SentidoPlayer * 0.6f, Color.red);
            
            PlayerAnimator.SetTrigger("Attack2");
            if (RaioInteracao)
            {
                RaioInteracao.collider.SendMessage("Interagir", SendMessageOptions.DontRequireReceiver);
            }
        }
        
        if (h != 0)
        {
            if (h > 0)
            {
                DPADRenderer.sprite = DPADSprites[2];
                SentidoPlayer = Vector2.right;
            }
            else
            {
                DPADRenderer.sprite = DPADSprites[4];
                SentidoPlayer = Vector2.left;
            }
            IDAnimacao = 2;
            transform.localScale = new Vector3(h, 1, 1);
        }
        else
        {
            if (Energia > 200)
            {
                IDAnimacao = 0;
            }
            else
            {
                IDAnimacao = 1;
            }
            if (v == 0)
            {
                DPADRenderer.sprite = DPADSprites[0];
            }
        }

        if (v == 1)
        {
            ZoomIn();
            DPADRenderer.sprite = DPADSprites[1];
        }
        else if (v == -1)
        {
            ZoomOut();
            DPADRenderer.sprite = DPADSprites[3];
        }

        if (PlayerRigidbody.linearVelocityY < 0)
        {
            IDAnimacao = 4;
        }

        PlayerAnimator.SetInteger("IDAnimacao", IDAnimacao);
        PlayerRigidbody.linearVelocity = new Vector2(h * Velocidade, PlayerRigidbody.linearVelocity.y);
    }

    private void DetectarTeclado()
    {
        Jump = Input.GetButtonDown("Jump");
        Attack1 = Input.GetButtonDown("Fire2");
        Attack2 = Input.GetButtonDown("Fire1");
        MenuTeclado = Input.GetButtonDown("Cancel");
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
    }

    public void ZoomIn()
    {
        PassoZoom = (ZoomMinimo - ZoomMaximo) * Time.deltaTime / TempoZoom; 
        ZoomAtual = CinemachineCamera.Lens.OrthographicSize - PassoZoom;
        if (ZoomAtual < ZoomMaximo)
        {
            ZoomAtual = ZoomMaximo;
        }
        CinemachineCamera.Lens.OrthographicSize = ZoomAtual;
    }

    public void ZoomOut()
    {
        PassoZoom = (ZoomMinimo - ZoomMaximo) * Time.deltaTime / TempoZoom; 
        ZoomAtual = CinemachineCamera.Lens.OrthographicSize + PassoZoom;
        if (ZoomAtual > ZoomMinimo)
        {
            ZoomAtual = ZoomMinimo;
        }
        CinemachineCamera.Lens.OrthographicSize = ZoomAtual;
    }

    private void DetectarTouch()
    {
        // print(Time.time + " Dedos sobre a tela = " + Input.touchCount);
        if (Input.touchCount > 0) 
        {
            for (int i = 0; i < Input.touchCount; i++)
            { 
                Toque = Input.GetTouch(i);
                //Cursor.position = Camera.main.ScreenToWorldPoint(new Vector3(Toque.position.x, Toque.position.y, 1));

                if (Toque.phase == TouchPhase.Stationary || Toque.phase == TouchPhase.Moved || Toque.phase == TouchPhase.Ended)
                {
                    x = Toque.position.x - DPAD_x;
                    y = Toque.position.y - DPAD_y;

                    RaioMomentaneo = Mathf.Sqrt(x * x + y * y);

                    if (RaioMomentaneo < RaioMaximo)
                    {
                        idFinger = i;
                        //print(Time.time + " D-PAD Ativado");

                        if (RaioMomentaneo < RaioMinimo)
                        {
                            //print(Time.time + " Centro do D-PAD: Ponto Morto");
                            h = 0;
                            v = 0;
                        }
                        else
                        {
                            if (Mathf.Abs(x)  > Mathf.Abs(y))
                            {
                                if (x > 0)
                                {
                                    h = 1;
                                }
                                else
                                {
                                    h = -1;
                                }
                            }
                            else
                            {
                                if (y > 0)
                                {
                                    v = 1;
                                }
                                else
                                {
                                    v = -1;
                                }
                            }
                        }
                    }
                    else if (Toque.phase == TouchPhase.Ended && idFinger == i) 
                    {
                        //print(Time.time + " Fora do D-PAD");
                        h = 0;
                        v = 0;
                    }
                }
            
            }
        }
    }
    public void BotaoA()
    {
        BotaoJump = true;
    }
    public void BotaoB()
    {
        BotaoAttack2 = true;
    }
    public void BotaoX()
    {
        BotaoAttack1 = true;
    }
    public void BotaoY()
    {
        BotaoMenu = true;
    }
    public void HitPlayer()
    {
        PlayerAnimator.SetTrigger("Hit");
        SomDano.volume = Volume;
        SomDano.Play();
        Energia = Energia - 50;
        DisplayEnergia.text = Energia.ToString();

        if (Energia <= 0)
        {
            IDAnimacao = 5;
            PlayerAnimator.SetInteger("IDAnimacao", 5);
            PlayerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
            EliminandoPlayer = true;
            Vidas--;
            DisplayVidas.text = Vidas.ToString();
        }
    }
    public void EncerrarEliminacaoPlayer()
    {
        PlayerRigidbody.constraints = RigidbodyConstraints2D.None;
        PlayerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (Vidas == 0)
        {
            Resetar();
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            PlayerAnimator.Rebind();
            PlayerAnimator.Update(0);
            IDAnimacao = 0;
            PlayerAnimator.SetInteger("IDAnimacao", 0);
            Energia = StartEnergia;
            DisplayEnergia.text = Energia.ToString();
            transform.position = new Vector3(PlayerPrefs.GetFloat("CoordenadaX", 0), PlayerPrefs.GetFloat("CoordenadaY", 0), 0);
            transform.localScale = new Vector3(PlayerPrefs.GetFloat("EscalaX", 1), 1, 1);
        }
        EliminandoPlayer = false;
    }

    public void Resetar()
    {
        ControladorPrincipalScript.Painel.SetActive(true);
        Moedas = StartMoedas;
        Energia = StartEnergia;
        Vidas = StartVidas;
        DisplayMoedas.text = Moedas.ToString();
        DisplayEnergia.text = Energia.ToString();
        DisplayVidas.text = Vidas.ToString();
        PlayerPrefs.SetFloat("CoordenadaX", 0);
        PlayerPrefs.SetFloat("CoordenadaY", 0);
        PlayerPrefs.SetFloat("EscalaX", 1);
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = new Vector3(0, 0, 0);
        IDAnimacao = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Inimigo"))
        {
            HitPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Win")
        {
            Win = true;
            Winning();
        }
    }

    void Winning()
    {
        SceneManager.LoadScene("Inicio");
    }
}
