using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject key;
    public bool isGaveKey = false;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float distance = (Vector3.Distance(transform.position, FindObjectOfType<PlayerController>().transform.position)) - 16.3f;

        if (isGaveKey == true && distance <= 0.5f)
        {
            anim.SetBool("isOpen", true);
        }
    }
    public void TurnOnKey()
    {
        key.SetActive(true);
    }
}
