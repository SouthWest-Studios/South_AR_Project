using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ARBallController : MonoBehaviour
{
    public GameObject ballPrefab;   // Prefab de la bola
    private GameObject spawnedBall; // Bola instanciada
    private Camera arCamera;        // C�mara AR

    public float moveSpeed = 1f;    // Velocidad a la que se mover� la bola hacia adelante

    public GameObject canvas;
    void Start()
    {
        // Obtener la c�mara AR
        arCamera = Camera.main;  // Si solo tienes una c�mara principal, esta es la AR Camera

        // Asignar el listener al bot�n para crear la bola al pulsar
        //spawnButton.onClick.AddListener(SpawnBall);
    }

    void Update()
    {

    }

    void SpawnBall()
    {
        // Instanciar la bola frente a la c�mara
        if (arCamera != null && ballPrefab != null)
        {
            // Obtener la posici�n y la direcci�n de la c�mara
            Vector3 spawnPosition = arCamera.transform.position + arCamera.transform.forward * 2f; // 2f es la distancia frente a la c�mara

            // Instanciar la bola en esa posici�n
            spawnedBall = Instantiate(ballPrefab, spawnPosition, arCamera.transform.rotation);
            spawnedBall.transform.parent = null;
        }
    }

    public void KilEnemiesByType(int i)
    {
        KillEnemiesByType((EnemyType)i);
    }

    public void KillEnemiesByType(EnemyType type)
    {
        List<GameObject> enemyToDamage = GetEnemiesByType(type);


        foreach (GameObject enemy in enemyToDamage)
        {
            Destroy(enemy);
        }
    }


    public List<GameObject> GetEnemiesByType(EnemyType type)
    {
        List<GameObject> visibleObjects = new List<GameObject>();
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(arCamera);
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            if (GeometryUtility.TestPlanesAABB(planes, enemy.GetComponent<Renderer>().bounds) && enemy.type == type) // Si est� dentro del frustum
            {
                visibleObjects.Add(enemy.gameObject);
            }
        }

        return visibleObjects;
    }

    public void ShowCanvas()
    {
        canvas.SetActive(true); // Activar el canvas
        StartCoroutine(HideCanvasAfterDelay(1f)); // Llamar a la coroutine para ocultarlo despu�s de 1 segundo
    }

    IEnumerator HideCanvasAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Espera el tiempo especificado
        canvas.SetActive(false); // Desactivar el canvas
    }
}
