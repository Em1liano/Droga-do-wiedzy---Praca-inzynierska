using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Down : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("MovableObject"))
        {
            //GetComponentInParent<DoorWeight>().isInside = true;
        }
        else if (other.gameObject.tag.Equals("Player"))
        {
            //GetComponentInParent<DoorWeight>().isInside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("MovableObject"))
        {
            //GetComponentInParent<DoorWeight>().isInside = false;
        }
        else if (other.gameObject.tag.Equals("Player"))
        {
            //GetComponentInParent<DoorWeight>().isInside = false;
        }
    }
}
