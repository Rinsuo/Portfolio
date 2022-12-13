using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;

    [SerializeField] private Transform Ppos;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool melee = false;
    [SerializeField] private GameObject firePoint;
    public ParticleSystem system;

    [SerializeField] private float fireRate = 0.5f;
    private float speed;
    private Vector2 target;
    private float dist;
    [SerializeField] private float MaxTargetDist;
    [SerializeField] private float dmg;
    private bool dead = false;
    private Vector2 relativeRandomDir;
    

    private float currentHP;
    private float maxHP;
    //public EnemyHealthBar HealthBar;

    private bool debounce = false;
    private bool debo = false;
    private bool doRandomCycle = false;

    private IEnumerator coroutine;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        //HealthBar.SetHealth(currentHP, maxHP);

        coroutine = RandomMove();
        StartCoroutine(coroutine);

    }

    // Update is called once per frame
    void Update()
    {

        FollowPlayer();
        StartCoroutine(FireBullet());
        if (Game.Instance.Break == true)
        {
            Destroy(this);
        }
    }

    private void FollowPlayer()
    {
        if (dead == false)
        {
            target = new Vector2(Ppos.position.x, Ppos.position.y);
            speed = moveSpeed * Time.deltaTime;
            dist = Vector2.Distance(target, transform.position);
            if (melee == true)
            {
                if (doRandomCycle)
                {
                    if (debounce == false)
                    {
                        debounce = true;
                        relativeRandomDir = transform.position;
                        relativeRandomDir = (Vector2)relativeRandomDir + new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
                    }
                    transform.position = Vector2.MoveTowards(transform.position, relativeRandomDir, speed);
                }
                else
                {
                    if (dist < MaxTargetDist)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, target, speed);
                    }
                }
            }
            else if (melee == false)
            {
                if (debounce == false)
                {
                    debounce = true;
                    relativeRandomDir = transform.position;
                    relativeRandomDir = (Vector2)relativeRandomDir + new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
                }
                transform.position = Vector2.MoveTowards(transform.position, relativeRandomDir, speed);
                dist = Vector2.Distance(target, transform.position);
                print("range");
            }
        }
        else
        {
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator RandomMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            debounce = false;
            if (melee == true)
            {
                doRandomCycle = true;
                yield return new WaitForSeconds(0.5f);
                doRandomCycle = false;
            }
        }
    }

    private void Timer()
    {

    }

    //private IEnumerator WaitTime(float time)
    //{
    //    if (debounce == false)
    //    {
    //        debounce = true;
    //        yield return new WaitForSeconds(time);
    //        debounce = false;
    //    }
    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && dead == false)
        {
            StartCoroutine(DoPlayerDamage(collision));
            //PlayerHealthManager.instance.HurtPlayer(dmg);
        }
    }
    IEnumerator DoPlayerDamage(Collider2D collision)
    {
        if (debo == false)
        {
            debo = true;
            collision.SendMessage("RecieveDamage", dmg);
            yield return new WaitForSeconds((float)1);
            debo = false;
        }
    }
    private void EnemyDie()
    {
        StartCoroutine(EnemyDie2());
    }
    IEnumerator EnemyDie2()
    {
        if (dead == false)
        {
            print("recieved");
            dead = true;
            Game.Instance.Sound(3);
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            var emitParams = new ParticleSystem.EmitParams();
            system.Emit(emitParams, 10);
            yield return new WaitForSeconds((float)0.7);
            Destroy(gameObject);

        }
    }
    IEnumerator FireBullet()
    {
        if (debo == false && dead == false && dist < MaxTargetDist && melee == false)
        {
            debo = true;
            print("fire");
            firePoint.SendMessage("FireBullet", dmg);
            yield return new WaitForSeconds((float)fireRate);
            debo = false;
        }
    }
}
