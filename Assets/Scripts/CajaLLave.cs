using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CajaLLave : MonoBehaviour
{
    public GameObject replacement;
    public GameObject llave;
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
        if(other.gameObject.tag == "Ground")
        {
            GameObject.Instantiate(replacement, transform.position, transform.rotation);
            GameObject.Instantiate(llave, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
