using UnityEngine;
using UnityEngine.UI;

public class ARBallController : MonoBehaviour
{
    public GameObject ballPrefab;   // Prefab de la bola
    public Button spawnButton;      // Bot�n para aparecer la bola
    private GameObject spawnedBall; // Bola instanciada
    private Camera arCamera;        // C�mara AR

    public float moveSpeed = 1f;    // Velocidad a la que se mover� la bola hacia adelante

    void Start()
    {
        // Obtener la c�mara AR
        arCamera = Camera.main;  // Si solo tienes una c�mara principal, esta es la AR Camera

        // Asignar el listener al bot�n para crear la bola al pulsar
        spawnButton.onClick.AddListener(SpawnBall);
    }

    void Update()
    {
        // Si la bola fue instanciada, moverla hacia adelante
        if (spawnedBall != null)
        {
            // Mover la bola en la direcci�n en la que est� mirando la c�mara
            spawnedBall.transform.Translate(spawnedBall.transform.forward * moveSpeed * Time.deltaTime);
        }
    }

    void SpawnBall()
    {
        // Instanciar la bola frente a la c�mara
        if (arCamera != null && ballPrefab != null)
        {
            // Obtener la posici�n y la direcci�n de la c�mara
            Vector3 spawnPosition = arCamera.transform.position + arCamera.transform.forward * 2f; // 2f es la distancia frente a la c�mara

            // Instanciar la bola en esa posici�n
            spawnedBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            spawnedBall.transform.rotation = Quaternion.Euler(0, arCamera.transform.rotation.y, 0);
        }
    }
}
