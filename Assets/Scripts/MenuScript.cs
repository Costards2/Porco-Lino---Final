using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuScript : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Volume, Pagina1, Pagina2, Pagina3;
    public UnityEngine.UI.Slider ControleVolume;
    public Transform Barris, Baus, CheckPoints, Coletaveis;
    private bool SaveAtivo;
    private string NomeArquivo;
    private PlayerScript PlayerScript;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerScript = FindAnyObjectByType<PlayerScript>();
        ControleVolume.value = PlayerScript.Volume;
    }

    public void AlterarVolume()
    {
        PlayerScript.Volume = ControleVolume.value;
        PlayerScript.Musica.volume = ControleVolume.value;
    }

    public void PressionarBotao()
    {
        EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>().fontSize = 30;
    }

    public void LiberarBotao()
    {
        EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>().fontSize = 24;
    }

    public void SaveBotao()
    {
        SaveAtivo = true;
        GetComponent<Animator>().SetTrigger("VirarPagina");
    }

    public void LoadBotao()
    {
        SaveAtivo = false;
        GetComponent<Animator>().SetTrigger("VirarPagina");
    }

    public void RestartBotao()
    {
        GetComponent<Animator>().SetTrigger("Fechar");
        SceneManager.LoadScene("PorcoLino");
        PlayerScript.Resetar();
    }

    public void QuitBotao()
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void SaveSlot1()
    {
        NomeArquivo = Application.persistentDataPath + "/Slot1.dat";
        Save();
    }

    public void SaveSlot2()
    {
        NomeArquivo = Application.persistentDataPath + "/Slot2.dat";
        Save();
    }

    public void SaveSlot3()
    {
        NomeArquivo = Application.persistentDataPath + "/Slot3.dat";
        Save();
    }

    public void LoadSlot1()
    {
        NomeArquivo = Application.persistentDataPath + "/Slot1.dat";
        Load();
    }

    public void LoadSlot2()
    {
        NomeArquivo = Application.persistentDataPath + "/Slot2.dat";
        Load();
    }

    public void LoadSlot3()
    {
        NomeArquivo = Application.persistentDataPath + "/Slot3.dat";
        Load();
    }

    public void DesativarPaginas()
    {
        Volume.SetActive(false);
        Pagina1.SetActive(false);
        Pagina2.SetActive(false);
        Pagina3.SetActive(false);
    }

    public void AtivarPagina1()
    {
        Volume.SetActive(true);
        Pagina1.SetActive(true);
    }

    public void AtivarPagina2ou3()
    {
        Volume.SetActive(true); 
        if (SaveAtivo)
        {
            Pagina2.SetActive(true);
        }
        else
        {
            Pagina3.SetActive(true);
        }
    }

    public void FecharLivro()
    {
        gameObject.SetActive(false);
    }

    public void Save()
    {
        BinaryFormatter DadosEmBinario = new BinaryFormatter();
        FileStream Arquivo = File.Create(NomeArquivo);
        DadosSlot DadosAGravar = new DadosSlot();

        DadosAGravar.Identificacao = DateTime.Now.ToString();
        DadosAGravar.Player_X = PlayerScript.transform.position.x;
        DadosAGravar.Player_Y = PlayerScript.transform.position.y;
        DadosAGravar.PlayerScale_X = PlayerScript.transform.localScale.x;
        DadosAGravar.CheckPoint_X = PlayerPrefs.GetFloat("CheckPoint_X", 0);
        DadosAGravar.CheckPoint_Y = PlayerPrefs.GetFloat("CheckPoint_Y", 0);
        DadosAGravar.CheckPointScale_X = PlayerPrefs.GetFloat("CheckPointScale_X", 1);
        DadosAGravar.Moedas = PlayerScript.Moedas;
        DadosAGravar.Energia = PlayerScript.Energia;
        DadosAGravar.Vidas = PlayerScript.Vidas;

        int i = 0;
        foreach (Transform t in Barris)
        {
            DadosAGravar.Barril[i] = t.gameObject.activeSelf;
            DadosAGravar.Barril_Contador[i] = t.gameObject.GetComponentInChildren<BarrilScript>().Contador;
            i++;
        }

        i = 0;
        foreach (Transform t in Baus)
        {
            DadosAGravar.Bau[i] = t.gameObject.activeSelf;
            DadosAGravar.Bau_Aberto[i] = t.gameObject.GetComponent<BauScript>().BauAberto;
            print("Bau " + i + " Status = " + DadosAGravar.Bau_Aberto[i]);
            i++;
        }

        i = 0;
        foreach (Transform t in CheckPoints)
        {
            DadosAGravar.CheckPoint[i] = t.gameObject.activeSelf;
            DadosAGravar.CheckPoint_Ativado[i] = t.gameObject.GetComponent<CheckPointScript>().CheckPointAtivado;         
            i++;
        }

        i = 0;
        foreach (Transform t in Coletaveis)
        {
            DadosAGravar.Coletavel[i] = t.gameObject.activeSelf;
            i++;
        }

        DadosEmBinario.Serialize(Arquivo, DadosAGravar);
        Arquivo.Close();

        GetComponent<Animator>().SetTrigger("Fechar");
    }

    public void Load()
    {
        if (!File.Exists(NomeArquivo))  
        {
            return;
        }

        PlayerScript.CarregandoCena = true;
        SceneManager.LoadScene("PorcoLino");
        StartCoroutine("AguardarCarregamentoCena");        
    }

    IEnumerator AguardarCarregamentoCena()
    {
        while (PlayerScript.CarregandoCena) yield return null;
        BinaryFormatter DadosEmBinario = new BinaryFormatter();
        FileStream Arquivo = File.OpenRead(NomeArquivo);
        DadosSlot DadosLidos = (DadosSlot)DadosEmBinario.Deserialize(Arquivo);
        Arquivo.Close();        

        PlayerScript.transform.position = new Vector3(DadosLidos.Player_X, DadosLidos.Player_Y, 0);
        PlayerScript.transform.localScale = new Vector3(DadosLidos.PlayerScale_X, 1, 1);
        PlayerPrefs.SetFloat("CheckPoint_X", DadosLidos.CheckPoint_X);
        PlayerPrefs.SetFloat("CheckPoint_Y", DadosLidos.CheckPoint_Y);
        PlayerPrefs.SetFloat("CheckPointScale_X", DadosLidos.CheckPointScale_X);
        PlayerScript.Moedas = DadosLidos.Moedas;
        PlayerScript.DisplayMoedas.text = DadosLidos.Moedas.ToString();
        PlayerScript.Energia = DadosLidos.Energia;
        PlayerScript.DisplayEnergia.text = DadosLidos.Energia.ToString();
        PlayerScript.Vidas = DadosLidos.Vidas;
        PlayerScript.DisplayVidas.text = DadosLidos.Vidas.ToString();

        GetComponent<MenuScript>().Barris = GameObject.Find("Barris").GetComponent<Transform>();
        GetComponent<MenuScript>().Baus = GameObject.Find("Baus").GetComponent<Transform>();
        GetComponent<MenuScript>().CheckPoints = GameObject.Find("CheckPoints").GetComponent<Transform>();
        GetComponent<MenuScript>().Coletaveis = GameObject.Find("Coletaveis").GetComponent<Transform>();

        int i = 0;
        foreach (Transform t in Barris)
        {
            t.gameObject.SetActive(DadosLidos.Barril[i]);
            t.gameObject.GetComponentInChildren<BarrilScript>().Contador = DadosLidos.Barril_Contador[i];
            i++;
        }

        i = 0;
        foreach (Transform t in Baus)
        {
            t.gameObject.SetActive(DadosLidos.Bau[i]);
            t.gameObject.GetComponent<BauScript>().BauAberto = DadosLidos.Bau_Aberto[i];
            if (DadosLidos.Bau_Aberto[i])
            {
                t.gameObject.GetComponent<Animator>().SetTrigger("Abrir");
            }
            else
            {
                t.gameObject.GetComponent<Animator>().SetTrigger("Fechar");
            }
            i++;
        }

        i = 0;
        foreach (Transform t in CheckPoints)
        {
            t.gameObject.SetActive(DadosLidos.CheckPoint[i]);
            t.gameObject.GetComponent<CheckPointScript>().CheckPointAtivado = DadosLidos.CheckPoint_Ativado[i];
            i++;
        }

        i = 0;
        foreach (Transform t in Coletaveis)
        {
            t.gameObject.SetActive(DadosLidos.Coletavel[i]);
            i++;
        }
        GetComponent<Animator>().SetTrigger("Fechar");
    }
}

[Serializable]
class DadosSlot
{
    public string Identificacao;
    public float Player_X, Player_Y, PlayerScale_X;
    public float CheckPoint_X, CheckPoint_Y, CheckPointScale_X;
    public int Moedas, Energia, Vidas;
    public bool[] Barril = new bool[11];
    public int[] Barril_Contador = new int[11];
    public bool[] Bau = new bool[3];
    public bool[] Bau_Aberto = new bool[3];
    public bool[] CheckPoint = new bool[4];
    public bool[] CheckPoint_Ativado = new bool[4];
    public bool[] Coletavel = new bool[8];
}
