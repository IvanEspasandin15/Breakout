using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
    [SerializeField] Transform pala;  // Objeto Pala
    [SerializeField] float duration;  // Duración de la Transición

    // Se ejecuta una vez por Frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {  // Si se pulsa la Tecla Espacio
            StartCoroutine("StartNextLevel");  // Empezar Corutina de cambio de Nivel
        }
    }

    // Empezar Siguiente Nivel
    IEnumerator StartNextLevel() {
        Vector3 scaleStart = pala.localScale;  // Obtener escala de la Pala
        Vector3 scaleEnd = new Vector3(1.5f, scaleStart.y, scaleStart.z);  // Escala Final de la Pala

        float t = 0; 
        while(t < duration) {  // Mientras el Tiempo Actual sea Menor que la Duración
            t += Time.deltaTime; // Sumar al Tiempo el Tiempo Transcurrido
            pala.localScale = Vector3.Lerp(scaleStart, scaleEnd, t/duration);  // Cambiar Escala de la Pala Progresivamente
            yield return null;  // Espera hasta el siguiente Frame
        }
        SceneManager.LoadScene(1);  // Cargar Escena con id 1
    }
}