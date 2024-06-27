using UnityEngine;

public class LightController : MonoBehaviour
{
    public float range = 10f;
    public float intensity = 1f;
    public Color lightColor = Color.white;

    public Light pointLight;

    void Start()
    {
        // Añadir una Light al objeto
        

        // Configurar la luz como Point Light
        pointLight.type = LightType.Point;
        pointLight.range = range;
        pointLight.intensity = intensity;
        pointLight.color = lightColor;
    }

    void Update()
    {
        // Opcional: Puedes actualizar las propiedades de la luz en tiempo de ejecución
        pointLight.range = range;
        pointLight.intensity = intensity;
        pointLight.color = lightColor;
    }
}
