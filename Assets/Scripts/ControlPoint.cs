using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPoint : MonoBehaviour
{
    float xRot, yRot = 0f;

    public Rigidbody personaje;

    public float rotationSpeed = 5f;

    public float shootPower = 30f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = personaje.position;
        {
            if(Input.GetMouseButton(0))
            {
                xRot += Input.GetAxis("Mouse X")*rotationSpeed;
                yRot += Input.GetAxis("Mouse Y")*rotationSpeed;
                transform.rotation = Quaternion.Euler(yRot, xRot, 0f);

            }

            if(Input.GetMouseButtonUp(0))
            {
                personaje.velocity = transform.forward * shootPower;
            }
        }
    }
}
