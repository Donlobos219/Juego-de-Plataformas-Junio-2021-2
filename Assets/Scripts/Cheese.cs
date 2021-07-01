using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheese : MonoBehaviour
{

    public static Vector3 cheesePos;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TrackCheese());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TrackCheese()
    {
        while (true)
        {
            cheesePos = gameObject.transform.position;
            yield return null;
        }
    }
}
