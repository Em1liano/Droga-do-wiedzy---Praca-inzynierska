using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Referencje
    public Transform player;
    public Vector3 offset;
    public GameObject child;

    // Zmienne
    [Range(0.01f, 20.0f)] public float FollowSpeed = 2.0f;
    public float rotSpeed = 3f;
    public float dirY = 0f;
    public int zoomMin = 40;
    public int zoomMax = 100;


    void Start()
    {
        StartCoroutine(WaitForPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        // Sprawdzenie czy gracz istnieje i następnie stopniowo przesuwa kamerę pomiędzy pozycją kamery a pozycją gracza, aby nie było sztucznego efektu podążania
        if (player != null)
        {
            Follow();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, zoomMin, Time.deltaTime * 5);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, zoomMax, Time.deltaTime * 5);
        }
    }

    IEnumerator WaitForPlayer()
    {
        // Warunek sprawdzenia czy gracz istnieje, jeśli nie to znajduje to tagu
        while (player == null)
        {
            yield return null; // czekanie na kolejną klatkę i dalsza część kodu
            var obj = GameObject.FindGameObjectWithTag("Player");
            // Jak znajdzie gracza to przypisuje transform gracza do obj
            if (obj != null)
            {
                player = obj.transform;
                child = player.transform.Find("camera constraint").gameObject;
            }
        }
    }
    public void Follow()
    {
        transform.position = Vector3.Lerp(transform.position, child.transform.position,
            Time.deltaTime * FollowSpeed);

        Vector3 lTargerDir = player.position - transform.position;
        lTargerDir.y = dirY;
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, Quaternion.LookRotation(lTargerDir), Time.time * rotSpeed);
    }
}
