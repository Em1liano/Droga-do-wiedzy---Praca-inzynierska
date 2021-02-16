using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheRock : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("TheRock"))
        {
            FindObjectOfType<DoorWeight>().isRockInside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("TheRock"))
        {
            FindObjectOfType<DoorWeight>().isRockInside = false;
        }
    }
}
