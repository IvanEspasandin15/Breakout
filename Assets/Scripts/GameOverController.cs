using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour {
    [SerializeField] TextMeshProUGUI GameOver; // Texto de Game Over
    bool gameOver = false;

    // Se ejecuta cuando se inicia el Script, antes del Primer Frame
    void Start() {
        GameOver.gameObject.SetActive(false);  // Desactivar Texto
    }

    // Se ejecuta una vez por Frame
    void Update() {
        if (!gameOver && GameManager.Lives <= 0) {  // Si aún no se está en Game Over y las Vidas son Menores o Iguales a 0
            GameOver.gameObject.SetActive(true);  // Se activa el Texto de Game Over
            gameOver = true;
        }
        
        if (gameOver && Input.anyKeyDown) {  // Si se está en Game Over y se pulsa una tecla se Reinicia el Juego
            GameManager.ResetGame();
        }
    }
}