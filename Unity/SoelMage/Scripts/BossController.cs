using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
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
    [SerializeField] private float attackDist;
    [SerializeField] private float dmg;
    private bool dead = false;
    private Vector2 relativeRandomDir;
    private int moveMode;


    private float currentHP;
    private float maxHP;
    //public EnemyHealthBar HealthBar;

    private bool debounce = false;
    private bool debounse = false;
    private bool debo = false;
    private bool doRandomCycle = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Cycle());
    }
    private bool doMove = false;
    // Update is called once per frame
    void Update()
    {
        target = new Vector2(Ppos.position.x, Ppos.position.y);
        speed = moveSpeed * Time.deltaTime;
        if (doMove == true)
        {
            if (moveMode == 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, relativeRandomDir, speed);
            }
            else
            {
                transform.position = Vector2.Lerp(transform.position, target, speed);
            }
        }
        //Movement();
        if (Game.Instance.Break == true)
        {
            Destroy(this);
        }
    }

    private void Movement()
    {
        if (dead == false)
        {
            target = new Vector2(Ppos.position.x, Ppos.position.y);
            speed = moveSpeed * Time.deltaTime;
            dist = Vector2.Distance(target, transform.position);
            if (dist > MaxTargetDist)
            {
                relativeRandomDir = transform.position;
                relativeRandomDir = (Vector2)relativeRandomDir + new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
                moveMode = 1;
                StartCoroutine(Move());
            }
            else if (dist < MaxTargetDist)
            {
                moveMode = 2;
                StartCoroutine(Move());
                StartCoroutine(meleeAttack());
            }
        }
        else
        {

        }
    }

    IEnumerator Cycle()
    {
        while(dead == false)
        {
            yield return new WaitForSeconds((float)4);
            Movement();
        }
    }

    IEnumerator meleeAttack()
    {
        if (dist < MaxTargetDist && dead == false)
        {
            //animation thing here
            GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            yield return new WaitForSeconds((float)1);
            if (dist < attackDist && dead == false)
            {
                var p = GameObject.FindGameObjectWithTag("Player");
                p.SendMessage("RecieveDamage", dmg);
                Game.Instance.Sound(1);
            }
            GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            yield return new WaitForSeconds((float)1);
        }
    }
    private bool dowait = false;
    IEnumerator Move()
    {
        doMove = true;
        yield return new WaitForSeconds((float)0.3);
        doMove = false;
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
}
