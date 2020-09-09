﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    chasing,
    waiting,
    shooting,
    parried,
    dying
}



public class EnemyController : MonoBehaviour
{

    protected Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    public EnemyState currentState;

    public float time_parried;
    public float max_parried;

    /// <summary>
    /// Data values
    /// </summary>
    /// 
    public string enemyName;
    public int baseAttack;
    public Animator animator;
    public float speed = 1.0f;

    public float shootcool;
    public float shoottimer;


    public bool vertical;
    public float changeTime = 3.0f;

    protected Rigidbody2D rigidbody2D;
    protected TextMesh texto;
    protected float timer;
    int direction = 1;
    protected Vector3 dir = new Vector3();

    int next_move = 0;

    /// <summary>
    /// Health values
    /// </summary>
    protected int maxhealth;
    protected int currenthealth;
    public int health { set { currenthealth = value; }
                        get { return currenthealth; }
    }

    public GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.waiting;
        maxhealth = 5;
        currenthealth = maxhealth;
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetFloat("Move X", 0);
        animator.SetFloat("Move Y", -1);
        timer = changeTime;
        shootcool = 3.0f;
        max_parried = 2.0f;

        chaseRadius = 5.0f;
        attackRadius = 0.5f;
        baseAttack = 1;

        texto = transform.GetChild(4).GetComponent<TextMesh>();
        string life = maxhealth + "/" + maxhealth;
        texto.text = life;

        target = GameObject.FindWithTag("Player").transform;
    }

    protected virtual void Launch()
    {

        //Debug.Log("LANSANDO");
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);

        ProjectileEnemy projectile = projectileObject.GetComponent<ProjectileEnemy>();
        projectile.Launch(dir, 120);
    }

    protected virtual void CheckDistance()
    {
        
        if ((Vector3.Distance(target.position, transform.position) <= chaseRadius) && (Vector3.Distance(target.position, transform.position) > attackRadius))
        {
            if (currentState != EnemyState.dying)
            {
                //currentState = EnemyState.chasing;
                Vector3 old = new Vector3();
                old = transform.position;
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                old = target.position - transform.position;
                dir = old;
                animator.SetFloat("Move X", old.x);
                animator.SetFloat("Move Y", old.y);

            }

        }
        
    }


    protected virtual void Update()
    {

        //Debug.Log(currentState);

        if (currentState != EnemyState.parried)
        {
            CheckDistance();

            shoottimer -= Time.deltaTime;

            //Debug.Log(currentState);



            //Debug.Log("attacking");


            if (currentState != EnemyState.dying)
            {
                currentState = EnemyState.chasing;
                if (Vector3.Distance(target.position, transform.position) <= attackRadius)
                {
                    StartCoroutine("Attack");
                    next_move = 0;
                    //currentState = EnemyState.waiting;
                }



                //Debug.Log("shootin");
                //currentState = EnemyState.chasing;

                if (shoottimer - Time.deltaTime < 0)
                {
                    if (Vector3.Distance(target.position, transform.position) <= 3)
                    {
                        Launch();
                        shoottimer = shootcool;
                        next_move = 0;
                        //currentState = EnemyState.waiting;
                    }
                }
            }
        }
        else
        {
            time_parried -= Time.deltaTime;
            if(time_parried < 0)
            {
                currentState = EnemyState.idle;
            }
        }
            
            
        
    }

        

    // Update is called once per frame
    void FixedUpdate()
    {
        
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        Vector2 position = rigidbody2D.position;

        /*if (vertical)
        {
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
            position.y = position.y + Time.deltaTime * speed * direction;
        }
        else
        {
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
            position.x = position.x + Time.deltaTime * speed * direction;
        }

        rigidbody2D.MovePosition(position);*/
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        /*MoonboyController player = other.gameObject.GetComponent<MoonboyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }*/
    }

    public virtual void Damage(int amount)

    {

        currenthealth -= amount;

        if (currenthealth > 0)
        {
            string life = currenthealth + "/" + maxhealth;

            texto.text = life;

        }
        else if(currenthealth == 0)
        {
            string life = currenthealth + "/" + maxhealth;

            texto.text = life;
            currentState = EnemyState.dying;
            StartCoroutine("Waiter");
        }


        




    }

    IEnumerator Waiter()
    {

        
        animator.SetTrigger("Die");
        //Wait for 2 seconds
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);

    }

    protected virtual IEnumerator Attack()
    {
        float horizontal = dir.x;
        float vertical = dir.y;

        if (!Mathf.Approximately(horizontal, 0.0f) || !Mathf.Approximately(vertical, 0.0f))
        {

            //Debug.Log("lo arreglamos");
            if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
            {
                if (horizontal > 0)
                {

                    animator.SetFloat("Move X", 1);
                }
                else
                {
                    animator.SetFloat("Move X", -1);

                }

                animator.SetFloat("Move Y", 0);
            }
            else
            {

                if (vertical > 0)
                {
                    animator.SetFloat("Move Y", 1);

                }
                else
                {
                    animator.SetFloat("Move Y", -1);

                }
                animator.SetFloat("Move X", 0);
            }
        }

        animator.SetBool("Attack", true);
        //currentState = PlayerState.attacking;
        yield return null;
        animator.SetBool("Attack", false);
        yield return new WaitForSeconds(0.3f);
        //currentState = PlayerState.idle;
        yield return new WaitForSeconds(3.0f);

    }
}