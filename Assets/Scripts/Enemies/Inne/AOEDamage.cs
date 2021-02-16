using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // jeżeli gracz jest w zasięgu triggera podczasu wybuchu to otrzymuje obrażenia
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<EnemyKamikazeController>().Hit();
        }
    }

}
