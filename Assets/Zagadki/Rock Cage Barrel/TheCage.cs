﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheCage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("TheCage"))
        {
            FindObjectOfType<DoorWeight>().isCageInside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("TheCage"))
        {
            FindObjectOfType<DoorWeight>().isCageInside = false;
        }
    }
}
