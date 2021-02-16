using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerUICanvas : MonoBehaviour
{
    // Referencje do UI życia
    [Header("Health")]
    [SerializeField] private Image foregroundImage;
    // Referencje do UI many
    [Header("Mana")]
    [SerializeField] private Image foregroundManaImage;

    // Referencje i zmienne potrzebne do funkcjonalności umiejętności nr1 
    [Header("Ability 1")]
    public Image abilityImage1;
    public float cooldown1 = 5f;
    [SerializeField] bool isCooldown = false;
    public KeyCode ability1;

    // Referencje i zmienne potrzebne do funkcjonalności umiejętności nr2
    [Header("Ability 2")]
    public Image abilityImage2;
    public float cooldown2 = 5f;
    [SerializeField] bool isCooldown2 = false;
    public KeyCode ability2;

    // Referencje i zmienne potrzebne do funkcjonalności umiejętności nr3
    [Header("Ability 3")]
    public Image abilityImage3;
    public float cooldown3 = 5f;
    [SerializeField] bool isCooldown3 = false;
    public KeyCode ability3;

    // Referencje i zmienne potrzebne do funkcjonalności umiejętności nr4
    [Header("Ability 4")]
    public Image abilityImage4;
    public float cooldown4 = 5f;
    [SerializeField] bool isCooldown4 = false;
    public KeyCode ability4;

    // Referencje i zmienne potrzebne do funkcjonalności umiejętności nr5
    [Header("Ability 5")]
    public Image abilityImage5;
    public float cooldown5 = 5f;
    [SerializeField] bool isCooldown5 = false;
    public KeyCode ability5;


    private void Awake()
    {
        GetComponentInParent<PlayerHealth>().OnHealthPctChanged += HandleHealthChanged;
        GetComponentInParent<Mana>().OnManaPctChanged += HandleManaChanged;
    }

    private void Start()
    {
        // Na początku ustawienie umiejętności jako możliwych do użycia
        abilityImage1.fillAmount = 1;
        abilityImage2.fillAmount = 1;
        abilityImage3.fillAmount = 1;
        abilityImage4.fillAmount = 1;
        abilityImage5.fillAmount = 1;
    }

    private void HandleHealthChanged(float pct)
    {
        foregroundImage.fillAmount = pct;
    }
    private void HandleManaChanged(float pct)
    {
        foregroundManaImage.fillAmount = pct;
    }

    private void Update()
    {
        // Wywołanie funkcji umiejętnośći
        Ability1();
        Ability2();
        Ability3();
        //Ability4();
        //Ability5();
    }
    void Ability1()
    {
        // jeżeli gracz wciśnie przypisany w inspektorze klawisz i jeżeli można użyć umiejętności
        if (Input.GetKey(ability1) && isCooldown == false && FindObjectOfType<PlayerController>().isMoving == true)
        {
            // to resetuje zmienna isCooldown
            isCooldown = true;
            FindObjectOfType<PlayerController>().canDash = false;
            abilityImage1.fillAmount = 1;
        }

        if (isCooldown)
        {
            // Odliczanie czasu odnowienia w zależności od upływu czasu
            abilityImage1.fillAmount -= 1 / cooldown1 * Time.deltaTime;

            if (abilityImage1.fillAmount <= 0)
            {
                abilityImage1.fillAmount = 0;
                isCooldown = false;
            }
        }

        if (isCooldown == false)
        {
            FindObjectOfType<PlayerController>().canDash = true;
            abilityImage1.fillAmount = 1;
        }
    }

    void Ability2()
    {
        if (Input.GetKey(ability2) && isCooldown2 == false)
        {
            isCooldown2 = true;
            FindObjectOfType<PlayerController>().canPassive = false;
            abilityImage2.fillAmount = 1;
        }

        if (isCooldown2)
        {
            abilityImage2.fillAmount -= 1 / cooldown2 * Time.deltaTime;

            if (abilityImage2.fillAmount <= 0)
            {
                abilityImage2.fillAmount = 0;
                isCooldown2 = false;
            }
        }

        if (isCooldown2 == false)
        {
            FindObjectOfType<PlayerController>().canPassive = true;
            abilityImage2.fillAmount = 1;
        }
    }

    void Ability3()
    {
        if (Input.GetKey(ability3) && isCooldown3 == false && FindObjectOfType<PlayerController>().isMoving == false)
        {
            isCooldown3 = true;
            FindObjectOfType<PlayerController>().canSpell = false;
            abilityImage3.fillAmount = 1;
        }

        if (isCooldown3)
        {
            abilityImage3.fillAmount -= 1 / cooldown3 * Time.deltaTime;

            if (abilityImage3.fillAmount <= 0)
            {
                abilityImage3.fillAmount = 0;
                isCooldown3 = false;
            }
        }

        if (isCooldown3 == false)
        {
            abilityImage3.fillAmount = 1;
            FindObjectOfType<PlayerController>().canSpell = true;
        }
    }

    void Ability4()
    {
        if (Input.GetKey(ability4) && isCooldown4 == false)
        {
            isCooldown4 = true;
            abilityImage4.fillAmount = 1;
        }

        if (isCooldown4)
        {
            abilityImage4.fillAmount -= 1 / cooldown4 * Time.deltaTime;

            if (abilityImage4.fillAmount <= 0)
            {
                abilityImage4.fillAmount = 0;
                isCooldown4 = false;
            }
        }

        if (isCooldown4 == false)
        {
            abilityImage4.fillAmount = 1;
        }
    }

    void Ability5()
    {
        if (Input.GetKey(ability5) && isCooldown5 == false)
        {
            isCooldown5 = true;
            abilityImage5.fillAmount = 1;
        }

        if (isCooldown5)
        {
            abilityImage5.fillAmount -= 1 / cooldown5 * Time.deltaTime;

            if (abilityImage5.fillAmount <= 0)
            {
                abilityImage5.fillAmount = 0;
                isCooldown5 = false;
            }
        }

        if (isCooldown5 == false)
        {
            abilityImage5.fillAmount = 1;
        }
    }

}
