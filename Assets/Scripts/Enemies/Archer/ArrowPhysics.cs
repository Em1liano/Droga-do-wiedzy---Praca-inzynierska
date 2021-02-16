using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Określone stany dla strzały
public enum ArrowState
{
    Aim,
    MoveForward
}
public class ArrowPhysics : MonoBehaviour
{
    [Header("References")]
    public PlayerController target;
    private Rigidbody rb;
    [Header("Machine state")]
    public ArrowState _currentState;

    [Header("Variables")]
    public float moveSpeed = 30f;
    private bool afterAim = false;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Ustawienie na początku gry stanu na Aim
        _currentState = ArrowState.Aim;
    }

    // Update is called once per frame
    void Update()
    {
        // Maszyna stanów
        switch (_currentState)
        {
            // Stan Aim
            case ArrowState.Aim:
                {
                    // Zwraca pierwszy aktywny załadowany obiekt typu PlayerController
                    target = FindObjectOfType<PlayerController>();

                    // Jeżeli cel istnieje to obraca się w jego kierunku i ustawia bool na true
                    if (target != null)
                    {
                        FaceTarget();
                        afterAim = true;

                        // Jeżeli afterAim to prawda to zmienia stan na MoveForward
                        if (afterAim)
                        {
                            _currentState = ArrowState.MoveForward;
                        }
                    }                 
                    break;
                }
            // Stan MoveForward
            case ArrowState.MoveForward:
                {
                    FaceTarget();
                    // Podążą prosto niezależnie od wydajności komputera i zależnie od ustalonej prędkości
                    transform.position += transform.forward * Time.deltaTime * moveSpeed;

                    float distance = Vector3.Distance(transform.position, target.transform.position);

                    if (distance <= 1f)
                    {
                        // TODO something?
                    }
                    break;
                }
        }
    }
    // Funkcja oblicza rotację jaką musi wykonać, aby być równo obrócona w stronę gracza
    public void FaceTarget()
    {
        Vector3 lookPos = target.transform.position - transform.position;
        lookPos.y = 0;

        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.75f);
    }
    // Jeżeli występuje kolizja
    private void OnCollisionEnter(Collision collision)
    {
        // Z tagiem Player to zabiera życie graczowi i niszczy obiekt strzały
        if (collision.gameObject.tag.Equals("Player"))
        {
            FindObjectOfType<ArcherController>().ArcherHit();
            Destroy(gameObject);
        }
    }
}
