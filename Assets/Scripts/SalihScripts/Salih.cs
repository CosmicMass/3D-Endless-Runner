using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salih : MonoBehaviour
{
    public List<GameObject> plains;
    Vector3 direction;
    int randomDirection;
    Vector3 _rotation;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            randomDirection = Random.Range(0, 2);
            if(randomDirection == 0) //Saða ekler
            {
                direction = new Vector3(40f, 0f, 0f);
                _rotation = new Vector3(0f, 90f, 0f);
            }
            else //Düz ekler
            {
                direction = new Vector3(0f, 0f, 40f);
            }


            plains.Remove(other.gameObject);
            other.gameObject.transform.position = plains[plains.Count - 1].transform.position + direction;
            other.gameObject.transform.Rotate(_rotation);
            plains.Add(other.gameObject);

        }
    }

}
