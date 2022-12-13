using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    private int healthAmount;
    [SerializeField]
    private int manaAmount;
    [SerializeField]
    private int damageToGive;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Game.Instance.Sound(5);
            if (gameObject.CompareTag("HP"))
            {
                collision.SendMessage("ManageHP", healthAmount);
            }
            if (gameObject.CompareTag("MP"))
            {
                collision.SendMessage("ManageMP", manaAmount);
            }
            if (gameObject.CompareTag("fullHP"))
            {
                PlayerHealthManager.instance.SetMaxHP();
            }
            if (gameObject.CompareTag("fullMP"))
            {
                PlayerHealthManager.instance.SetMaxMP();
            }
            if (gameObject.CompareTag("upgradeHP"))
            {
                print("hp up");
                PlayerHealthManager.instance.UpgradeHP();
            }
            if (gameObject.CompareTag("upgradeMP"))
            {
                print("mp up");
                PlayerHealthManager.instance.UpgradeMP();
            }
        Destroy(gameObject);
        }
    }
}