using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class ShatterOnCollider : MonoBehaviour
{
    public GameObject replacement;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "SuperShoot")
        {
            GameObject.Instantiate(replacement, transform.position, transform.rotation);
            Destroy(gameObject);

        }
    }

}
