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

    }

    void SpawnBall()
    {
        // Instanciar la bola frente a la cámara
        if (arCamera != null && ballPrefab != null)
        {
            // Obtener la posición y la dirección de la cámara
            Vector3 spawnPosition = arCamera.transform.position + arCamera.transform.forward * 2f; // 2f es la distancia frente a la cámara

            // Instanciar la bola en esa posición
            spawnedBall = Instantiate(ballPrefab, arCamera.transform);
            spawnedBall.transform.parent = null;
        }
    }
}
