using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBarrel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("TheBarrel"))
        {
            FindObjectOfType<DoorWeight>().isBarrelInside = true;
        }     
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("TheBarrel"))
        {
            FindObjectOfType<DoorWeight>().isBarrelInside = false;
        }
    }
}
