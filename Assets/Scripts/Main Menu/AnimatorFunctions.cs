using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    public bool disableOnce;
    
    void PlaySound(AudioClip whichSound)
    {
        if (!disableOnce)
        {
            // tu będzie włączany dźwięk kliknięcia w menu
        }
        else
        {
            disableOnce = false;
        }
    }

    
}
