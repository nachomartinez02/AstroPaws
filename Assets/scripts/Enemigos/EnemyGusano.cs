using System.Collections;
using UnityEngine;

public class EnemyGusano : MonoBehaviour
{
    private bool puedeAtacar = true;
    private Animator animator;

    void Start()
    {
        
        animator = GetComponent<Animator>();
        StartCoroutine(ComprobarAtaque());
    }

    
    IEnumerator ComprobarAtaque()
    {
        while (true)
        {
            if (puedeAtacar)
            {
                int probabilidad = Random.Range(1, 4); 
                if (probabilidad == 1)
                {
                    Atacar();
                    puedeAtacar = false;
                    
                    yield return new WaitForSeconds(5f); 
                    puedeAtacar = true;
                    animator.SetBool("Ataca",false);
                }
            }
            yield return new WaitForSeconds(1f); 
        }
    }

    void Atacar()
    {
        animator.SetBool("Ataca",true);
    }
}