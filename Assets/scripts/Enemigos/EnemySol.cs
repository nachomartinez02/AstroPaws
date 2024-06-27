using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySol : MonoBehaviour
{
    private bool puedeAtacar = true;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // Iniciar la corrutina para comprobar la probabilidad de ataque cada segundo
        animator = GetComponent<Animator>();
        StartCoroutine(ComprobarAtaque());
    }

    // Corrutina que comprueba la probabilidad de ataque
    IEnumerator ComprobarAtaque()
    {
        while (true)
        {
            if (puedeAtacar)
            {
                int probabilidad = Random.Range(1, 4); // Genera un número aleatorio entre 1 y 5
                if (probabilidad == 1)
                {
                    Atacar();
                    puedeAtacar = false;
                    
                    yield return new WaitForSeconds(5f); // Espera 5 segundos después de atacar
                    puedeAtacar = true;
                    animator.SetBool("AtkSol",false);
                }
            }
            

            yield return new WaitForSeconds(1f); // Espera 1 segundo antes de volver a comprobar
        }
    }

    // Método que se llama cuando el enemigo ataca
    void Atacar()
    {
        
        // Lógica del ataque del enemigo
        
        animator.SetBool("AtkSol",true);
    }
}