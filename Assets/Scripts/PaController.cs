using UnityEngine;

public class PaController : MonoBehaviour {
    [SerializeField] float speed;  // Velocidad de la Pala
    const float MAX_X = 3f;  // Posición Máxima en X
    const float MIN_X = -3f;  // Posición Mínima en X

    // Se ejecuta una vez por Frame
    void Update() {
        float x = transform.position.x;  // Posición de la Pala

        if (x > MIN_X && Input.GetKey("left")) {  // Si la Posición es mayor que el Mínimo y se pulsa la Tecla hacia la Izquierda
            // Con Time.deltaTime se obtiene una referencia de la velocidad independiente del hardware.
            transform.Translate(-speed * Time.deltaTime, 0, 0);  // Velocidad Negativa para moverse hacia la Izquierda

        } else if (x < MAX_X && Input.GetKey("right")) {  // Si la Posición es menor que el Máximo y se pulsa la Tecla hacia la Derecha
            transform.Translate(speed * Time.deltaTime, 0, 0);  // Velocidad Positiva para moverse hacia la Derecha
        }
    }
}