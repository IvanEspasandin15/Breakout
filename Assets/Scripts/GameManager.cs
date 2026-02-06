using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {
    public static int Score { get; private set; } = 0;  // Puntuación.
    public static int Lives { get; private set; } = 5;  // Vidas del jugador.
    public static int[] totalBricks = new int[] {0, 32, 32};  // Asociar número de Ladrillos Destructibles a cada Escena

    // Se ejecuta cuando se inicia el Script, antes del Primer Frame
    void Start() {
        Cursor.visible = false;  // Desactivar el Cursor del Ratón
    }

    // Se ejecuta una vez por Frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {  // Si se pulsa la Tecla Escape
            Application.Quit();  // Salir del Juego Compilado

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;  // Salir del Juego en el Editor Unity
            #endif
        }
    }

    // Incrementar Puntuación
    public static void UpdateScore(int points) {
        Score += points;
    }

    // Decrementar Vidas
    public static void Updatelives() {
        Lives--;
    }

    // Resetear Juego
    public static void ResetGame() {
        Score = 0;
        Lives = 5;
        SceneManager.LoadScene(0);
    }
}