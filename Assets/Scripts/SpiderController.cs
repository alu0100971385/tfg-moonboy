using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class SpiderController : EnemyController
{
    /// <summary>
    /// New data values for this child
    /// </summary>
    bool dash_selected;
    Vector3 enemy_located;
    Vector3 change;
    bool attacking;

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.waiting;
        maxhealth = 3;
        currenthealth = maxhealth;
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetFloat("Move X", 0);
        animator.SetFloat("Move Y", -1);
        shootcool = 1.0f;
        speed = 0.0f;

        currentState = EnemyState.idle;

        chaseRadius = 5.0f;
        attackRadius = 5.0f;
        baseAttack = 1;

        texto = transform.GetChild(0).GetComponent<TextMesh>();
        string life = maxhealth + "/" + maxhealth;
        texto.text = life;

        target = GameObject.FindWithTag("Player").transform;
    }

    protected override void CheckDistance()
    {

        if (currentState != EnemyState.dying)
        {
            if ((Vector3.Distance(target.position, transform.position) <= chaseRadius))
            {

                attacking = true;

                Vector3 old = new Vector3();
                old = target.position - transform.position;

                if (!dash_selected)
                {
                    Debug.Log(target.position);
                    enemy_located = old;
                    dash_selected = true;


                }

                if (currentState == EnemyState.shooting)
                {

                    if (speed >= 0.0f)
                    {
                        rigidbody2D.MovePosition(transform.position + enemy_located * speed * Time.deltaTime);

                    }

                    dir = old;
                    animator.SetFloat("Move X", old.x);
                    animator.SetFloat("Move Y", old.y);
                }

            }
        }
    }

    protected override void Update()
    {
        
        
        StartCoroutine(Dash());

    }

    void FixedUpdate()
    {
        CheckDistance();
    }

    

    IEnumerator Dash()
    {

        float horizontal = dir.x;
        float vertical = dir.y;

        Vector2 direction = new Vector2(horizontal, vertical);

        if (currentState == EnemyState.idle && attacking)
        {
            if (!Mathf.Approximately(horizontal, 0.0f) || !Mathf.Approximately(vertical, 0.0f))
            {

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

            //Debug.Log("forcee");
            currentState = EnemyState.shooting;
            shoottimer = shootcool;
            Vector2 act_pos = transform.position;
            //rigidbody2D.MovePosition(act_pos + direction * 50.0f * Time.deltaTime);
            //rigidbody2d.AddForce(direction * 50.0f);
            speed = 10f;
            animator.SetBool("Attack", true);
            yield return new WaitForSeconds(0.1f);
            animator.SetBool("Attack", false);
            attacking = false;

            speed = 0.0f;

            dash_selected = false;

            

           

            yield return new WaitForSeconds(1.0f);
            if(currentState != EnemyState.dying)
            {
                currentState = EnemyState.idle;

            }
            //Debug.Log(direction * 25.0f);

        }



        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Projectile Collision with " + collision.gameObject);

        MoonboyController e = collision.GetComponent<MoonboyController>();

        if (e != null)
        {
            e.ChangeHealth(-2);
        }
    }


}
