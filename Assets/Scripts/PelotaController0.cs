using UnityEngine;
using System.Collections.Generic;

public class PelotaController0 : MonoBehaviour {
    [SerializeField] float delay;
    [SerializeField] float force;
    [SerializeField] float ballSpeed;
    [SerializeField] AudioClip sfxPaddle;
    [SerializeField] AudioClip sfxBrick;
    [SerializeField] AudioClip sfxWall;
    Rigidbody2D rb;
    AudioSource sfx;
    Dictionary<string,int> ladrillos = new Dictionary<string, int>(){
        {"Ladrillo-Rojo", 25},
        {"Ladrillo-Amarillo", 20},
        {"Ladrillo-Verde", 15},
        {"Ladrillo-Azul", 10},
    };


    void Start() {
        sfx = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        Invoke("LanzarPelota", delay);
    }

    private void FixedUpdate() {
        rb.linearVelocity = rb.linearVelocity.normalized * ballSpeed;
    }

    private void LanzarPelota() {
        transform.position = Vector3.zero;
        rb.linearVelocity = Vector2.zero;
        float dirX, dirY = -1;
        dirX = Random.Range(-1f, 1f);
        Vector2 dir = new Vector2(dirX, dirY);
        dir.Normalize();

        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (ladrillos.ContainsKey(other.gameObject.tag)) {
            sfx.clip = sfxBrick;
            sfx.Play();
        }

        if (other.gameObject.tag == "Pa") {
            sfx.clip = sfxPaddle;
            sfx.Play();
        }

        if (other.gameObject.tag == "ParedeDereita" || other.gameObject.tag == "ParedeEsquerda" || other.gameObject.tag == "ParedeSuperior") {
            sfx.clip = sfxWall;
            sfx.Play();
        }
    }
}