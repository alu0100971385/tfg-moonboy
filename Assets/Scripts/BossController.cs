using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    // Start is called before the first frame update

    bool crossdir;
    void Start()
    {

        Debug.Log("MOCOS");
        currentState = EnemyState.waiting;
        maxhealth = 20;
        currenthealth = maxhealth;
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = changeTime;
        shootcool = 3.0f;
        max_parried = 2.0f;

        chaseRadius = 5.0f;
        attackRadius = 0.5f;
        baseAttack = 1;
        crossdir = true;
        target = GameObject.FindWithTag("Player").transform;
        texto = transform.GetChild(0).GetComponent<TextMesh>();
        string life = maxhealth + "/" + maxhealth;
        texto.text = life;

        
    }

    // Update is called once per frame
    protected override void Update()
    {
        CheckDistance();
        if (currentState == EnemyState.shooting)
        {
            Debug.Log("lets goo!");
            Launch();
        }
    }

    protected override void CheckDistance()
    {

        if ((Vector3.Distance(target.position, transform.position) <= chaseRadius))
        {
            if (currentState != EnemyState.dying)
            {

                currentState = EnemyState.shooting;


            }

        }
        else
        {
            currentState = EnemyState.idle;
        }

    }

    protected override void Launch()
    {

        shoottimer -= Time.deltaTime;
        Debug.Log(shoottimer);
        if (shoottimer <= 0)
        {
            Debug.Log("venga vamos");  
            //currentState = EnemyState.shooting;
            StartCoroutine(LaunchCo());
            shoottimer = shootcool;

        }

        
        
    }

    IEnumerator LaunchCo()
    {
       

        if (crossdir)
        {

            for(int i = 0; i < 10; i++)
            {

                int randvalue = Random.Range(0, 5);

                //Debug.Log(randvalue);

                if(randvalue%2 == 0)
                {
                    LaunchProjectile(new Vector2(1, 0), 120);
                    LaunchProjectile(new Vector2(-1, 0), 120);
                    LaunchProjectile(new Vector2(0, -1), 120);
                    LaunchProjectile(new Vector2(0, 1), 120);

                    LaunchProjectile(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)), 120);
                    //LaunchProjectile(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)), 120);

                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    LaunchProjectile(new Vector2(1, 1), 120);
                    LaunchProjectile(new Vector2(-1, 1), 120);
                    LaunchProjectile(new Vector2(-1, -1), 120);
                    LaunchProjectile(new Vector2(1, -1), 120);

                    //LaunchProjectile(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)), 120);
                    //LaunchProjectile(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)), 120);

                    yield return new WaitForSeconds(0.5f);
                }
                
            }

        }

        yield return null;
    }

    void LaunchProjectile(Vector2 dir, float force)
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2D.position + Vector2.up * 1f, Quaternion.identity);
        ProjectileEnemy projectile = projectileObject.GetComponent<ProjectileEnemy>();

        projectile.Launch(dir, force);

    }

}