using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public static ItemHandler instance;
    [SerializeField] private int startWeapon;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        var weapons = GameObject.FindGameObjectsWithTag("Weapon");
        print(startWeapon + " request");
        foreach (var weapon in weapons)
        {
            weapon.SendMessage("EquipWeapon", startWeapon);

        }
    }
}
