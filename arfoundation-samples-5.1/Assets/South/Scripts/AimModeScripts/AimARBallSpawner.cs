using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AimARBallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;   // Prefab de la bola
    public Button spawnButton;      // Botón para aparecer la bola
    private GameObject spawnedBall; // Bola instanciada
    private Camera arCamera;        // Cámara AR

    public float moveSpeed = 1f;    // Velocidad a la que se moverá la bola hacia adelante

    public int lastTouched = 0;

    public GameObject canvas;
    void Start()
    {
        // Obtener la cámara AR
        arCamera = Camera.main;  // Si solo tienes una cámara principal, esta es la AR Camera

        // Asignar el listener al botón para crear la bola al pulsar
        spawnButton.onClick.AddListener(SpawnBall);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if(lastTouched < Input.touchCount)
            {
                lastTouched = Input.touchCount;
                SpawnBall();
            }
        }
        else
        {
            lastTouched = 0;
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
            spawnedBall = Instantiate(ballPrefab, spawnPosition, arCamera.transform.rotation);
            spawnedBall.transform.parent = null;
        }
    }

    public void ShowCanvas()
    {
        canvas.SetActive(true); // Activar el canvas
        StartCoroutine(HideCanvasAfterDelay(1f)); // Llamar a la coroutine para ocultarlo después de 1 segundo
    }

    IEnumerator HideCanvasAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Espera el tiempo especificado
        canvas.SetActive(false); // Desactivar el canvas
    }
}
