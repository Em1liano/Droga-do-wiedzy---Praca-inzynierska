using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWeight : MonoBehaviour
{
    [Header("The Objects")]
    public GameObject theCage;
    public GameObject theBarrel;
    public GameObject theRock;

    public bool isCageInside = false;
    public bool isBarrelInside = false;
    public bool isRockInside = false;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRockInside && isBarrelInside && isCageInside)
        {
            anim.SetBool("isOpen", true);
        }
        else
        {
            anim.SetBool("isOpen", false);
        }
    }
}
