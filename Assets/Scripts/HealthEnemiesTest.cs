using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemiesTest : MonoBehaviour
{
    public float health = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health == 0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            Destroy(this.gameObject);
            health = -1f;
        }
    }
}
