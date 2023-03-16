using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tilesc : MonoBehaviour
{
    public List<GameObject> planes;
    Vector3 direction;
    Vector3 rotation;
    int directionVectorId;
    int beforedirectionVectorId;
    Vector3 rightVec = new Vector3(0f,90f,0f);
    Vector3 leftVec = new Vector3(0f,0f,0f);
    private void Start()
    {
        beforedirectionVectorId = 1;
        directionVectorId = 1;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("plane"))
        {
            other.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            beforedirectionVectorId = directionVectorId;
            directionVectorId = Random.Range(0, 2);

            if (directionVectorId == 0)                 // saðýna ekler
            {
                direction = new Vector3(10f, 0f, 0f);
                rotation = new Vector3(0f, 90f, 0f);
            }
            else                                       // önüne ekler
            {
                direction = new Vector3(0f, 0f, 10f);
                rotation = new Vector3(0f,-transform.rotation.y,0f);
            }

            if(directionVectorId == 0 && beforedirectionVectorId == 1)
            {
                planes[planes.Count - 1].transform.Rotate(0f, 90f, 0f);
            }
            else if(directionVectorId == 1 && beforedirectionVectorId == 0)
            {
               planes[planes.Count - 1].transform.Rotate(0f, -90f, 0f);
            }

            if(planes[0].transform.rotation.y == 0 && planes[1].transform.rotation.y == 0) // Düz
            {
        
                transform.rotation = Quaternion.Euler(leftVec);
            }
            else if (planes[0].transform.rotation.y == 0 && planes[1].transform.rotation.y > 0)  // Dön
            {
                
                transform.rotation = Quaternion.Euler(rightVec);
            
            }
            planes.Remove(other.gameObject);
            other.gameObject.transform.position = planes[planes.Count-1].transform.position + direction;
            other.gameObject.transform.Rotate(rotation);
            planes.Add(other.gameObject);
        }
    }
}
