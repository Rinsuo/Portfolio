using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    private float currentHP = 0;
    [SerializeField]
    private float maxHP;
    private bool dead;

    //public Image enemyHealthbar;
    //public Text HPText;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    public void SetMaxHP()
    {
        currentHP = maxHP;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

        }
    }

    private void RecieveDamage(float v)
    {
        if (dead == false)
        {
            ManageHP(-v);
            Game.Instance.Sound(7);
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
            SendMessage("EnemyDie");
            dead = true;
            return;
        }
        print(currentHP);
    }
}
