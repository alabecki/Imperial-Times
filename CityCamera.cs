using UnityEngine;
using System.Collections;

public class CityCamera : MonoBehaviour
{

    float inputX = Input.GetAxis("Horizontal");
    float inputZ = Input.GetAxis("Vertical");
    void Update()
    {
      
    
        if (inputX != 0)
            rotate();
        if (inputZ != 0)
            move();
    }

    void rotate()
    {
        transform.Rotate(new Vector3(0f, inputX * Time.deltaTime, 0f));
    }

    void move()
    {
        transform.position += transform.forward * inputZ * Time.deltaTime;
    }
}