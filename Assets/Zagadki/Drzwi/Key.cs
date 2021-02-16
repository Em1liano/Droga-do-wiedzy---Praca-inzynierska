using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            GetComponentInParent<Door>().isGaveKey = true;
            gameObject.SetActive(false);
        }
    }
}
