using System.Collections;
using UnityEngine;

public class ColliderDragon : MonoBehaviour
{
    private PolygonCollider2D polygonCollider;
    public float activeDuration = 2f; // Duración en segundos del colisionador activado
    public float inactiveDuration1 = 2.5f; // Duración en segundos del colisionador desactivado después de la primera activación
    public float inactiveDuration2 = 4.5f; // Duración en segundos del colisionador desactivado después de la segunda activación

    void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        if (polygonCollider == null)
        {
            Debug.LogError("PolygonCollider2D not found on " + gameObject.name);
            return;
        }

        StartCoroutine(ManageCollider());
    }

    IEnumerator ManageCollider()
    {
        while (true)
        {
            // Activar el colisionador
            polygonCollider.enabled = true;
            Debug.Log("Collider activated");
            yield return new WaitForSeconds(activeDuration);

            // Desactivar el colisionador
            polygonCollider.enabled = false;
            Debug.Log("Collider deactivated");
            yield return new WaitForSeconds(inactiveDuration1);

            // Activar el colisionador de nuevo
            polygonCollider.enabled = true;
            Debug.Log("Collider reactivated");
            yield return new WaitForSeconds(activeDuration+1);

            // Desactivar el colisionador por más tiempo
            
        }
    }
}