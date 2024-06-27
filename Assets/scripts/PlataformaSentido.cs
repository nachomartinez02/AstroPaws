using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaSentido : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.GetComponent<Collider2D>(),false);
        }
    }
}
