using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ProjectileEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigidbody2d;
    Animator animator;
    public GameObject splashobject;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
        StartCoroutine(waiter());

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //we also add a debug log to know what the projectile touch
        //Debug.Log("Projectile Collision with " + other.gameObject);

        bool notsplash = false;

        MoonboyController e = other.GetComponent<MoonboyController>();
        if (e != null)
        {
            //Debug.Log("ASIENDO DANIO");
            e.ChangeHealth(-1);
            if (e.isdashing)
            {
                notsplash = true;
            }
        }

        if (!notsplash)
        {
            Destroy(gameObject);
            GameObject splashhit = Instantiate(splashobject, rigidbody2d.position, Quaternion.identity);
        }
        

    }

    IEnumerator waiter()
    {
        //Debug.Log("destruyendo?");
        //Wait for 2 seconds
        yield return new WaitForSeconds(4);

        Destroy(gameObject);

    }

    IEnumerator waiter2()
    {

        //Wait for 1 seconds

        yield return new WaitForSeconds(1);
        

    }
}
