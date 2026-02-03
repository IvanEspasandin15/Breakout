using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PelotaController : MonoBehaviour {
    [SerializeField] float delay;  // Retraso al lanzar la Pelota
    [SerializeField] float force;  // Fuerza al lanzar la Pelota
    [SerializeField] float ballSpeed;  // Velocidad de la Pelota
    [SerializeField] AudioClip sfxPaddle;  // Audio de la pelota al colisionar con la pala
    [SerializeField] AudioClip sfxBrick;  // Audio de la pelota al colisionar con un ladrillo
    [SerializeField] AudioClip sfxWall;  // Audio de la pelota al colisionar con un muro
    [SerializeField] AudioClip sfxFail;  // Audio de la pelota al salir del área de juego por la parte inferior
    Rigidbody2D rb;  // RigidBody de la Pelota
    GameObject pala;  // Objeto Pala
    AudioSource sfx;  // Componente AudioSource
    int contadorGolpes = 0;  // Contador de Golpes Pala-Pelota
    bool halved = false;  // Valor que controla si la Pala se ha Reducido
    float initialBallSpeed;  // Velocidad Inicial de la Pelota
    int brickCount;  // Contador de Ladrillos Destruidos
    int sceneId;  // Id de la Escena Actual
    Dictionary<string, int> ladrillos = new Dictionary<string, int>(){  // Diccionario de Tags y Valores
        {"Ladrillo-Rojo", 25},
        {"Ladrillo-Amarillo", 20},
        {"Ladrillo-Verde", 15},
        {"Ladrillo-Azul", 10},
        {"Ladrillo-Atravesable", 25},
    };

    // Se ejecuta cuando se inicia el Script, antes del Primer Frame
    void Start() {
        initialBallSpeed = ballSpeed;
        pala = GameObject.FindWithTag("Pa");  // Buscar Objeto Pala por Tag
        sfx = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        sceneId = SceneManager.GetActiveScene().buildIndex;  // Inicializar id de la Escena Actual
        IniciarReiniciarPelota();
    }

    private void FixedUpdate() {
        rb.linearVelocity = rb.linearVelocity.normalized * ballSpeed;  // Mantener Velocidad Constante
    }

    // Lanzar Pelota
    private void LanzarPelota() {
        float dirX, dirY = -1;  // Dar a ambos Vectores de Velocidad el valor -1
        dirX = Random.Range(-1f, 1f);  // Dar al Vector de Velocidad Horizontal un valor aleatorio entre -1 (Izquierda) y 1 (Derecha)
        Vector2 dir = new Vector2(dirX, dirY);  // Crear Vector compuesto de Velocidad
        dir.Normalize();  // Normalizar Vector para que tenga Magnitud 1

        rb.AddForce(dir * force, ForceMode2D.Impulse);  // Aplicar Fuerza a la Pelota
    }

    // Reiniciar Pelota
    private void IniciarReiniciarPelota() {
        transform.position = Vector3.zero;  // Colocar Pelota en el Centro de la Pantalla
        rb.linearVelocity = Vector2.zero;  // Asignar Velocidad 0 a la Pelota
        ballSpeed = initialBallSpeed;  // Asignar la Velocidad Inicial a la Pelota
        if (halved) {  // Si la Pala ha sido Reducida
            halvePaddle(false);
        }
        Invoke("LanzarPelota", delay);  // Invocar función para lanzar la Pelota con el Retraso Correspondiente
    }

    // Se ejecuta cuando un Collider configurado como Trigger detecta otro Collider
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "ParedeInferior") {  // Si la Pelota atraviesa la Pared Inferior
            reproducirSonido(sfxFail);
            GameManager.Updatelives();  // Restar una vida

            if (GameManager.Lives <= 0) {
                rb.linearVelocity = Vector2.zero;  // Detener la Pelota
                gameObject.SetActive(false);  // Desactivar la Pelota
                return;  // Salir de la Función para qie no se Relance
            }

            IniciarReiniciarPelota();
        }

        if(other.gameObject.tag == "Ladrillo-Atravesable") {  // Si la Pelota atraviesa un Ladrillo Atravesable
            ++brickCount;
            GameManager.UpdateScore(ladrillos[other.tag]);  // Sumar los Puntos
            reproducirSonido(sfxBrick);
            other.enabled = false;  // Desactivar el Collider para no volver a sumar Puntos con ese Ladrillo

            // Si el Contador de Ladrillos llega al número total de Ladrillos de la Escena
            if(brickCount == GameManager.totalBricks[sceneId]) {
                rb.linearVelocity = Vector2.zero;
                Invoke("NextScene", 3);
            }
        }
    }

    // Se ejecuta cuando dos Colliders comienzan a tocarse (Es necesario que uno de los dos tenga un RigidBody2D)
    private void OnCollisionEnter2D(Collision2D other) {
        string tag = other.gameObject.tag;  // Obtener el Tag del Objeto al que impacta la Pelota

        if (ladrillos.ContainsKey(tag) && tag != "Ladrillo-Atravesable") {  // Si la Pelota impacta con un Ladrillo que no se pueda Atravesar
            ++brickCount;
            reproducirSonido(sfxBrick);
            GameManager.UpdateScore(ladrillos[tag]);  // Actualizar la Puntuación
            Destroy(other.gameObject, 0.05f);  // Destruir Ladrillo

            // Si el Contador de Ladrillos llega al número total de Ladrillos de la Escena
            if(brickCount == GameManager.totalBricks[sceneId]) {
                rb.linearVelocity = Vector2.zero;
                Invoke("NextScene", 3);
            }
        }

        if (tag == "Pa") {  // Si la Pelota impacta con la Pala
            contadorGolpes++;  // Incrementar el Contador de Golpes
            if (contadorGolpes%4 == 0) {
                ballSpeed++;  // Cada 4 golpes se incrementa la Velocidad de la Pelota
            }
            reproducirSonido(sfxPaddle);
            Vector3 pala = other.gameObject.transform.position;  // Obtener Posición de la Pala
            Vector2 contact = other.GetContact(0).point;  // Obtener el primer Punto de Contacto de la Pelota con la Pala
            
            // Invertir Dirección de la Pelota si va hacia la Derecha y da en el lado Izquierdo de la Pala y viceversa
            if(rb.linearVelocity.x < 0 && contact.x > pala.x || rb.linearVelocity.x > 0 && contact.x < pala.x) {
                rb.linearVelocity = new Vector2(-rb.linearVelocity.x, rb.linearVelocity.y);
            }
        }

        if (tag == "ParedeDereita" || tag == "ParedeEsquerda" || tag == "ParedeSuperior" || tag == "Ladrillo-Indestructible") {
            reproducirSonido(sfxWall);  // Si la Pelota impacta con una Pared se reproduce su Sonido
        }

        if (!halved && tag == "ParedeSuperior") {  // Si la Pala no se ha Reducido y la Pelota impacta en la Pared Superior
            halvePaddle(true);
        }
    }

    // Reproducir un Sonido
    private void reproducirSonido(AudioClip audioClip) {
        sfx.clip = audioClip;  // Asignar Clip correspondiente al AudioSource
        sfx.Play();  // Reproducir Clip del AudioSource
    }

    // Reducir Pala
    private void halvePaddle(bool reducir) {
        halved = reducir;  // Asignar a la Variable Controladora el Valor actual
        Vector3 escalaActual = pala.transform.localScale;  // Obtener Escala Actual de la Pala

        // Si hay que reducir la pala, se multiplica su dimensión x por 0.5, si no se multiplica por 2
        pala.transform.localScale = reducir ? new Vector3(escalaActual.x*0.5f, escalaActual.y, escalaActual.z) : new Vector3(escalaActual.x*2f, escalaActual.y, escalaActual.z);
    }

    // Cargar siguiente Escena
    void NextScene() {
        int nextId = sceneId + 1;  // Obtener id de la siguiente Escena
        if(nextId == SceneManager.sceneCountInBuildSettings) {  // Si ya no hay más Escenas
            nextId = 0;  // Establecer la escena de inicio como la siguiente
        }
        SceneManager.LoadScene(nextId);  // Cargar Escena
    }
}