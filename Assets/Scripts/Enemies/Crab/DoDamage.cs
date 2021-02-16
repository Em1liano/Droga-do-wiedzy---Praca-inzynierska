using Assets.Scripts.Enemies.Crab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            GetComponentInParent<CrabController>().HitMelee();
        }
    }
}
