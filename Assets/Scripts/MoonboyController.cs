using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState
{
    idle,
    running,
    attacking,
    dashing,
    shooting,
    blocking
}

public class MoonboyController : MonoBehaviour
{

    Animator animator;
    public int maxHealth = 10;
    Vector2 lookDirection = new Vector2(0, -1);

    public GameObject projectilePrefab;

    public int health { get { return currentHealth; } }

    int currentHealth;
    Rigidbody2D rigidbody2d;
    float speed = 4.0f;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    public float dashCooldown = 0.5f;
    bool isdashblocked;
    float dashTimer;
    public bool isdashing;

    public float shootcooldown = 2.0f;
    public float shoottimer;

    Vector2 change;

    public PlayerState currentState;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        currentState = PlayerState.idle;

        animator.SetFloat("Look X", -1);
        animator.SetFloat("Look Y", 0);

        shoottimer = shootcooldown;

        isdashing = false;

        //QualitySettings.vSyncCount = 1;
        //Application.targetFrameRate = 144;
    }

    // Update is called once per frame
    void FixedUpdate() //Located here because it moves the rigidbody?
    {

        if (currentState != PlayerState.attacking && currentState != PlayerState.blocking)
            CheckForMoving();


    }

    private void Update()
    {

        shoottimer -= Time.deltaTime;

        StartCoroutine(Dash());

        if (Input.GetKeyDown(KeyCode.C))
        {

            if(shoottimer <= 0)
            {
                Launch();
                shoottimer = shootcooldown;
            }
            
        }

        if (Input.GetButtonDown("attack") && currentState != PlayerState.attacking && currentState != PlayerState.blocking) //it is not already attacking
        {
            StartCoroutine(AttackCo());
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(BlockCo());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    IEnumerator BlockCo()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (!Mathf.Approximately(change.x, 0.0f) || !Mathf.Approximately(change.y, 0.0f))
        {
            if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
            {
                if (horizontal > 0)
                {

                    animator.SetFloat("Look X", 1);
                }
                else
                {
                    animator.SetFloat("Look X", -1);

                }

                animator.SetFloat("Look Y", 0);
            }
            else
            {

                if (vertical > 0)
                {
                    animator.SetFloat("Look Y", 1);

                }
                else
                {
                    animator.SetFloat("Look Y", -1);

                }
                animator.SetFloat("Look X", 0);
            }
        }

        animator.SetBool("Block", true);
        currentState = PlayerState.blocking;
        yield return null;
        animator.SetBool("Block", false);
        yield return new WaitForSeconds(1.0f);
        currentState = PlayerState.idle;
    }

    void CheckForMoving()
    {
        change = Vector2.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        

        if (change != Vector2.zero)
        {
            currentState = PlayerState.running;

            Vector2 act_pos = transform.position;
            change.Normalize();
            rigidbody2d.MovePosition(
                act_pos + change * speed * Time.deltaTime);
        }
        else
        {
            currentState = PlayerState.idle;
        }

        //Debug.Log("1: " + lookDirection);

        if (!Mathf.Approximately(change.x, 0.0f) || !Mathf.Approximately(change.y, 0.0f))
        {
            lookDirection.Set(change.x, change.y);
            lookDirection.Normalize();
        }


        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", change.magnitude);


        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
    }

    IEnumerator Dash()
    {
        //Debug.Log(dashTimer);

        if (isdashblocked)
        {
            dashTimer -= Time.deltaTime;

            if (dashTimer < 0)
            {
                isdashblocked = false;
            }
        }

        
       
        

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("yuh");
            if (!isdashblocked)
            {

                //Debug.Log("forcee");
                isdashblocked = true;
                dashTimer = dashCooldown;
                Vector2 act_pos = transform.position;
                //rigidbody2d.MovePosition(act_pos + direction * 50.0f * Time.deltaTime);
                //rigidbody2d.AddForce(direction * 50.0f);
                speed = 25.0f;
                currentState = PlayerState.dashing;
                isdashing = true;
                yield return new WaitForSeconds(0.075f);
                speed = 4.0f;
                isdashing = false;
                //Debug.Log(direction * 25.0f);

            }

        }

        yield return null;
    }



    public void ChangeHealth(int amount)
    {

        if (amount < 0)
        {
            if ((isInvincible) || currentState == PlayerState.blocking || isdashing)
                return;

            animator.SetTrigger("Hit");

            isInvincible = true;
            invincibleTimer = timeInvincible;
            currentHealth += amount;
        }
        else
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        }

        

        Debug.Log(currentHealth);

        if(currentHealth <= 0)
        {
            SceneManager.LoadScene("DeathMenu");
        }

        
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 500);

        animator.SetTrigger("Launch");
    }

    private IEnumerator AttackCo()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (!Mathf.Approximately(change.x, 0.0f) || !Mathf.Approximately(change.y, 0.0f))
        {
            if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
            {
                if (horizontal > 0)
                {

                    animator.SetFloat("Look X", 1);
                }
                else
                {
                    animator.SetFloat("Look X", -1);

                }

                animator.SetFloat("Look Y", 0);
            }
            else
            {

                if (vertical > 0)
                {
                    animator.SetFloat("Look Y", 1);

                }
                else
                {
                    animator.SetFloat("Look Y", -1);

                }
                animator.SetFloat("Look X", 0);
            }
        }
        
        animator.SetBool("Attack", true);
        currentState = PlayerState.attacking;
        yield return null;
        animator.SetBool("Attack", false);
        yield return new WaitForSeconds(0.3f);
        currentState = PlayerState.idle;

    }


}
