using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public static Weapon instance;

    [SerializeField] private int weaponId;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletDmg;
    [SerializeField] private float bulletSize;
    [SerializeField] private int manaCost;
    [SerializeField] public float wpCdTmr;
    [SerializeField] private Color bulletColor;
    [SerializeField] private GameObject player;
    [SerializeField] private float distance = 0.3f;
    [SerializeField] private float height = 0.3f;
    [SerializeField] private GameObject extraEffect;
    private float angle;
    private Vector3 v3pos;
    private bool faceUp = false;

    public Transform firePoint;
    public GameObject bulletPrefab;
    private int clickAmount = 0;
    private bool debounce;
    [SerializeField] private bool equipped = false;
    private bool switchCD = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        extraEffect.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (equipped == true)
        {
            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && equipped == true && faceUp == false)
            {
                faceUp = true;
                GetComponent<Renderer>().enabled = false;
                player.GetComponent<Renderer>().sortingOrder = 3;
            }
            if (!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) && equipped == true && faceUp == true)
            {
                faceUp = false;
                GetComponent<Renderer>().enabled = true;
                player.GetComponent<Renderer>().sortingOrder = 0;
            }
            if (Input.GetButton("Fire1"))
            {
                Shoot();
            }

            //ei mit‰‰ ideaa miten t‰‰ toimii kopioin vaa jostai
            v3pos = Input.mousePosition;
            v3pos.z = (player.transform.position.z - Camera.main.transform.position.z);
            v3pos = Camera.main.ScreenToWorldPoint(v3pos);
            v3pos = v3pos - player.transform.position;
            angle = Mathf.Atan2(v3pos.y, v3pos.x) * Mathf.Rad2Deg;
            if (angle < 0.0f) angle += 360.0f;
            transform.localEulerAngles = new Vector3(0, 0, angle);
            transform.localEulerAngles = new Vector3(0, 0, angle);
            float xPos = Mathf.Cos(Mathf.Deg2Rad * angle) * distance;
            float yPos = Mathf.Sin(Mathf.Deg2Rad * angle) * distance;
            transform.localPosition = new Vector3(player.transform.position.x + xPos, (float)(player.transform.position.y + yPos - height), 0);

        }
        if (Game.Instance.Break == true)
        {
            if (equipped == true)
            {
                GetComponent<Renderer>().material.color = new Color(0, 0, 0);
            }
            Destroy(this);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && equipped == false)
        {
            print("over");
            if (Input.GetKeyDown(KeyCode.F) && switchCD == false)
            {
                switchCD = true;
                StartCoroutine(WeaponSwitch());
                var weapons = GameObject.FindGameObjectsWithTag("Weapon");
                print(weaponId + " request");
                foreach (var weapon in weapons)
                {
                    weapon.SendMessage("EquipWeapon", weaponId);
                }


            }
        }
    }

    public void EquipWeapon(int v)
    {
        if (v == weaponId)
        {
            if (!equipped)
            {
                print("equip" + weaponId);
                equipped = true;
                Game.Instance.Sound(6);
            }
        }
        else
        {
            equipped = false;
            GetComponent<Renderer>().enabled = true;
        }
    }

    void Shoot()
    {
        if (debounce == false && PlayerHealthManager.instance.currentMP > 0 && PlayerHealthManager.instance.currentHP > 0)
        {
            debounce = true;
            Game.Instance.Sound(4);
            player.SendMessage("ManageMP", -manaCost);
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            BulletMove.instance.BulletDetails(bulletDmg, bulletSpeed, bulletSize, bulletColor, true);
            StartCoroutine(ExtraE());
        }
    }
    IEnumerator ExtraE()
    {
        clickAmount++;
        extraEffect.GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds((float)wpCdTmr);
        debounce = false;
        yield return new WaitForSeconds((float)0.1);
        clickAmount--;
        if (clickAmount == 0) { extraEffect.GetComponent<Renderer>().enabled = false; }
    }

    IEnumerator WeaponSwitch()
    {
        yield return new WaitForSeconds(1);
        switchCD = false;
    }
}