using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform graphics;

    private Vector2 direction;
    public float rotationSpeed = 3;

    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector2(Random.Range(-rotationSpeed, rotationSpeed), Random.Range(-rotationSpeed/2, rotationSpeed/2));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        graphics.Rotate(direction);
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
