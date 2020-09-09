using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Projectile : MonoBehaviour
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

    void OnCollisionEnter2D(Collision2D other)
    {
        //we also add a debug log to know what the projectile touch
        Debug.Log("Projectile Collision with " + other.gameObject);
        
        Destroy(gameObject);

        GameObject splashhit = Instantiate(splashobject, rigidbody2d.position, Quaternion.identity);
        EnemyController e = other.collider.GetComponent<EnemyController>();

        if (e != null)
        {
            e.Damage(1);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //we also add a debug log to know what the projectile touch
        Debug.Log("Projectile Collision with " + collision.gameObject);

        Destroy(gameObject);

        GameObject splashhit = Instantiate(splashobject, rigidbody2d.position, Quaternion.identity);
        EnemyController e = collision.GetComponent<EnemyController>();

        if (e != null)
        {
            e.Damage(1);
        }
    }

    IEnumerator waiter()
    {

        //Wait for 2 seconds
        yield return new WaitForSeconds(2);

        Destroy(gameObject);

    }

    IEnumerator waiter2()
    {

        //Wait for 1 seconds

        yield return new WaitForSeconds(1);
        

    }
}
