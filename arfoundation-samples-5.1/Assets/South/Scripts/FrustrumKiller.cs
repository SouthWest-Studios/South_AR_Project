using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustrumKiller : MonoBehaviour
{
    private Camera arCamera;
    // Start is called before the first frame update
    void Start()
    {
        arCamera = Camera.main;
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
            if (GeometryUtility.TestPlanesAABB(planes, enemy.GetComponent<Renderer>().bounds) && enemy.type == type) // Si está dentro del frustum
            {
                visibleObjects.Add(enemy.gameObject);
            }
        }

        return visibleObjects;
    }

}
