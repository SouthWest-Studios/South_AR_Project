using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimBullet : MonoBehaviour
{
    public float moveSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
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
