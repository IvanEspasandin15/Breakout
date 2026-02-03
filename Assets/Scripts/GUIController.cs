using UnityEngine;
using TMPro;

public class GUIController : MonoBehaviour {
    [SerializeField] TextMeshProUGUI txtScore;  // Texto Marcador
    [SerializeField] TextMeshProUGUI txtlives;  // Texto Vidas

    // Se ejecuta varias veces por Frame, en cada evento de GUI
    private void OnGUI() {
        txtScore.text = string.Format("{0,3:D3}", GameManager.Score);  // Actualizar Texto del Marcador formateado a 3 dígitos.
        txtlives.text = GameManager.Lives.ToString();  // Actualizar Texto de las Vidas.
    }
}