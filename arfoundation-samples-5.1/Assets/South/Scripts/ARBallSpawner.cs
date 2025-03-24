using UnityEngine;
using UnityEngine.UI;

public class ARBallController : MonoBehaviour
{
    public GameObject ballPrefab;   // Prefab de la bola
    public Button spawnButton;      // Botón para aparecer la bola
    private GameObject spawnedBall; // Bola instanciada
    private Camera arCamera;        // Cámara AR

    public float moveSpeed = 1f;    // Velocidad a la que se moverá la bola hacia adelante

    void Start()
    {
        // Obtener la cámara AR
        arCamera = Camera.main;  // Si solo tienes una cámara principal, esta es la AR Camera

        // Asignar el listener al botón para crear la bola al pulsar
        spawnButton.onClick.AddListener(SpawnBall);
    }

    void Update()
    {
        // Si la bola fue instanciada, moverla hacia adelante
        if (spawnedBall != null)
        {
            // Mover la bola en la dirección en la que está mirando la cámara
            spawnedBall.transform.Translate(spawnedBall.transform.forward * moveSpeed * Time.deltaTime);
        }
    }

    void SpawnBall()
    {
        // Instanciar la bola frente a la cámara
        if (arCamera != null && ballPrefab != null)
        {
            // Obtener la posición y la dirección de la cámara
            Vector3 spawnPosition = arCamera.transform.position + arCamera.transform.forward * 2f; // 2f es la distancia frente a la cámara

            // Instanciar la bola en esa posición
            spawnedBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            spawnedBall.transform.rotation = Quaternion.Euler(0, arCamera.transform.rotation.y, 0);
        }
    }
}
