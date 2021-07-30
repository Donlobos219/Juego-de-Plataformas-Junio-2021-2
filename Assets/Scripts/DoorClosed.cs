using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClosed : MonoBehaviour
{
    
    public bool opennedDoor = false;
    public bool opennedDoor2 = false;
    public GameObject replacement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (opennedDoor)
        {
            GameObject.Instantiate(replacement, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        if (opennedDoor2)
        {
            GameObject.Instantiate(replacement, transform.position, transform.rotation);
            Destroy(gameObject);
        }

    }

    public void SwitchDoor()
    {
        opennedDoor = true;
    }

    public void SwitchDoor2()
    {
        opennedDoor2 = true;
    }

    
    
}
