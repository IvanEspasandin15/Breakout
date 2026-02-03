using UnityEngine;
using TMPro;
using System.Collections;

public class TextColorLerp : MonoBehaviour {
    [SerializeField] TextMeshProUGUI msg;  // Texto de Inicio del Juego
    [SerializeField] float duration;  // Intervalo del Parpadeo del Texto

    // Se ejecuta cuando se inicia el Script, antes del Primer Frame
    void Start() {
        StartCoroutine("ChangeColor");  
    }

    // Cambiar Color del Texto
    IEnumerator ChangeColor() {
        float t = 0;  // Contador de Tiempo
        while(t < duration) {  // Mientras el Momento Actual sea Menor que la Duración
            t += Time.deltaTime;  // Incrementar el Contador de Tiempo con el Tiempo Transcurrido
            msg.color = Color.Lerp(Color.black, Color.white, t/duration);  // Transición de Color del Texto
            yield return null;  // Esperar hasta el Siguiente Frame
        }

        StartCoroutine("ChangeColor");  
    }
}