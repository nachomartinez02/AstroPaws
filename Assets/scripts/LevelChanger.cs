using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public int sceneBuildIndex;
    public Salida fadeInSceneChanger;
    public string siguienteEscena;

    private void OnTriggerEnter2D(Collider2D other){
        print("Trigger activado");

        if (other.tag == "Player"){
            fadeInSceneChanger.StartFadeInAndChangeScene(siguienteEscena);
        }
    }
}
