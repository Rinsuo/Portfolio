using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public static BulletMove instance;
    private float dmg;
    [SerializeField] private Rigidbody2D rb2d;
    private bool friendly;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    public void BulletDetails(float dmg2,float speed,float size, Color bColor, bool friend)
    {
        transform.localScale = new Vector3(size, size, size);
        GetComponent<Renderer>().material.color = bColor;
        rb2d.velocity = transform.right * speed;
        dmg = dmg2;
        friendly = friend;
        if (friend == false)
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Untagged"))
        {
            Destroy(gameObject);
        }

        if (collision.CompareTag("Enemy") && friendly == true)
        {
            collision.SendMessage("RecieveDamage", dmg);
            Destroy(gameObject);
        }

        if (collision.CompareTag("Player") && friendly == false)
        {
            collision.SendMessage("RecieveDamage", dmg);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Game.Instance.Break == true)
        {
            rb2d.velocity = Vector2.zero;
        }
    }
}