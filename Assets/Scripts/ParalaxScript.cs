using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxScript : MonoBehaviour
{
    public Transform ImagemADeslocar;
    public float FatorDeslocamentoX;

    private Vector3 PosicaoAnteriorCamera, PosicaoAtualCamera, NovaPosicaoImagem;
    private float DeslocamentoX;
    
    // Start is called before the first frame update
    void Start()
    {
        PosicaoAnteriorCamera = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        PosicaoAtualCamera = Camera.main.transform.position;
        DeslocamentoX = (PosicaoAnteriorCamera.x - PosicaoAtualCamera.x) * FatorDeslocamentoX;
        NovaPosicaoImagem = new Vector3(ImagemADeslocar.position.x + DeslocamentoX, PosicaoAtualCamera.y, 0);
        ImagemADeslocar.position = Vector3.Lerp(ImagemADeslocar.position, NovaPosicaoImagem, Time.deltaTime);
        PosicaoAnteriorCamera = PosicaoAtualCamera;
    }
}
