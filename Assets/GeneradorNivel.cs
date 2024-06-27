using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorNivel : MonoBehaviour
{
    public GameObject[] partesNivel;

    public GameObject level;

    public float distanciaMinima;

    public Transform puntoFinal;

    public int cantidadinicial;

    private Transform jugador;
    
    // Start is called before the first frame update
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < cantidadinicial; i++)
        {
            GenerarPartNivel();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(jugador.position, puntoFinal.position)<distanciaMinima){
            GenerarPartNivel();
        }
    }

    private void GenerarPartNivel()
    {
        int numeroAleatorio = Random.Range(0, partesNivel.Length);
        GameObject suelo = Instantiate(partesNivel[numeroAleatorio], puntoFinal.position, Quaternion.identity);
        suelo.transform.parent = level.transform;
        puntoFinal=BuscarPuntoFinal(suelo,"PuntoFinal");
    }

    private Transform BuscarPuntoFinal(GameObject parteNivel, string etiqueta){
        Transform punto = null;

        foreach(Transform ubi in parteNivel.transform)
        {
            if (ubi.CompareTag(etiqueta)){
                punto=ubi;
                break;
            }

        }
        return punto;
    }
}
