using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{

    public static PlayerHealthManager instance;

    public string charName;         //hahmon nimi
    public int playerLevel = 1;     //aloitustaso
    public float currentEXP;
    public float maxEXP = 0;        //seuraavaan tasoon tarvittava exp määrä
    public float currentHP = 0;
    [SerializeField]
    private float maxHP = 100;
    public float currentMP = 0;
    [SerializeField]
    private float maxMP = 100;
    private bool dead = false;

    public Image playerHealthbar;
    public Text HPText;
    public Image playerManabar;
    public Text MPText;
    public float lerpSpeed;         //palkin nopeus säädin
    //public Image EXPbar;
    public Text EXPText;
    public Text playerLevelText;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentHP = maxHP;
        currentMP = maxMP;
        maxEXP = Mathf.Floor(100 * playerLevel * Mathf.Pow(playerLevel, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerStatus();
    }

    private void CheckPlayerStatus()
    {
        //hp muuttunut?
        if (currentHP != playerHealthbar.fillAmount)
        {
            //päivitetään hp palkki
            playerHealthbar.fillAmount = Mathf.Lerp(playerHealthbar.fillAmount,
                currentHP / maxHP, Time.deltaTime * lerpSpeed);
            //päivitetään hp palkin teksti
            HPText.text = "HP: " + Mathf.Round(playerHealthbar.fillAmount * maxHP) + " / " + maxHP;
        }

        //mp muuttunut?
        if (currentMP != playerManabar.fillAmount)
        {
            //päivitetään mp palkki
            playerManabar.fillAmount = Mathf.Lerp(playerManabar.fillAmount,
                currentMP / maxMP, Time.deltaTime * lerpSpeed);
            //päivitetään mp palkin teksti
            MPText.text = "MP: " + Mathf.Round(playerManabar.fillAmount * maxMP) + " / " + maxMP;
        }
        /*
        if (currentEXP != EXPbar.fillAmount)
        {
            EXPbar.fillAmount = Mathf.Lerp(EXPbar.fillAmount, currentEXP / maxEXP,
                Time.deltaTime * lerpSpeed);
            EXPText.text = "EXP: " + currentEXP + " / " + maxEXP;
        }
        if (currentEXP >= maxEXP)
        {
            playerLevel += 1;
            playerLevelText.text = playerLevel.ToString();
            maxEXP = Mathf.Floor(100 * playerLevel * Mathf.Pow(playerLevel, 0.5f));
            currentEXP = 0;
            print("level up");
        }
        */
    }

    public void RecieveDamage(float v)
    {
        if (dead == false)
        {
            ManageHP(-v);
            StartCoroutine(dmgEffect());
        }
    }

    private void ManageHP(float v)
    {
        currentHP += v;
        if (currentHP < 0)
        {
            currentHP = 0;
        }
        else if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        if (currentHP == 0)
        {
            Die();
            return;
        }
    }

    private void Die()
    {
        dead = true;
        print(charName + " kuoli");
        SendMessage("PlayerDie");
    }

    private void ManageMP(float v)
    {
        currentMP += v;
        if (currentMP < 0)
        {
            currentMP = 0;
        }
        else if (currentMP > maxMP)
        {
            currentMP = maxMP;
        }
        if (currentMP == 0)
        {
            print("no mana");
            return;
        }
    }

    public void SetMaxHP()
    {
        currentHP = maxHP;
    }

    public void SetMaxMP()
    {
        currentMP = maxMP;
    }

    public void AddPEXP(int EXPamount)
    {
        currentEXP += EXPamount;
    }
    
    public void UpgradeHP()
    {
        if (maxHP < 150)
        {
            maxHP += 10;
            currentHP += 10;
        }
    }

    public void UpgradeMP()
    {
        if (maxMP < 150)
        {
            maxMP += 10;
            currentMP += 10;
        }
    }

    IEnumerator dmgEffect()
    {
        Game.Instance.Sound(8);
        GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        yield return new WaitForSeconds((float)0.2);
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }
}

