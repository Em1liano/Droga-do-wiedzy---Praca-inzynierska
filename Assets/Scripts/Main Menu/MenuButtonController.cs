using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour
{
    // Zmienne
    
    // CanvasType trzymający wyliczenia
    public CanvasType canvasType;
    // Aktualny index kafelek
    public int index;
    // Zmienna odpowiedzialna za opóźnienie pomiędzy kolejmymi przełączeniami pomiędzy indexami
    [SerializeField] bool keyDown;
    // Maksymalny index kafel
    [SerializeField] int maxIndex;

    // Update is called once per frame
    void Update()
    {
        // Sprawdzanie czy wciskamy przycisk w dół lub w górę
        if (Input.GetAxis("Vertical") != 0)
        {
            // jeżeli nie jest równy 0
            if (!keyDown)
            {
                // Jeżeli wciskamy w dół
                if (Input.GetAxis("Vertical") < 0)
                {
                    if (index < maxIndex)
                    {
                        // Zwiększanie indexu jeśli jest mniejszy od maxIndex
                        // stworzenie "pętli"
                        SoundManager.PlaySound(SoundManager.Sound.MenuChange);
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }
                }
                // jeżeli wciskamy w góre
                else if (Input.GetAxis("Vertical") > 0)
                {
                    if (index > 0)
                    {
                        // Zmniejszanie indexu o 1
                        SoundManager.PlaySound(SoundManager.Sound.MenuChange);
                        index--;
                    }
                    else
                    {
                        index = maxIndex;
                    }
                }
                keyDown = true;
            }
        }
        else
        {
            keyDown = false;
        }
    }
    }
