using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, offset.z + target.position.z);

        //vector lerp can be replaced by "newPosition", we used this for camera smoothness
        transform.position = Vector3.Lerp(transform.position, newPosition, 10 * Time.fixedDeltaTime); 
        
        
    }
}
