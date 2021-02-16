using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Referencja obrazu dla życia
    [SerializeField] private Image foregroundImage;
    // Zmienna trzymająca czas aktualizacji w sekundach
    [SerializeField] float updateSpeedSeconds = 0.5f;


    private void Awake()
    {
        GetComponentInParent<EnemyHealth>().OnHealthPctChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(float pct)
    {
        StartCoroutine(ChangeToPct(pct));
    }
    
    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = foregroundImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }
        foregroundImage.fillAmount = pct;
    }

    private void LateUpdate()
    {
        // Obrót canvasu życia, aby był widoczny wprost do pozycji kamery aktualnej
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
