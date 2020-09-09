using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MoonboyController e = collision.GetComponent<MoonboyController>();

        if (e != null)
        {

            if(e.currentState == PlayerState.blocking)
            {
                Debug.Log(transform.parent.GetComponent("EnemyController"));

                EnemyController ec = transform.parent.GetComponent<EnemyController>();

                StartCoroutine(Parried(ec));

                return;
            }
            else
            {
                e.ChangeHealth(-1);
            }
            
        }

    }

    IEnumerator Parried(EnemyController ec)
    {
        ec.currentState = EnemyState.parried;
        ec.time_parried = ec.max_parried;
        yield return new WaitForSeconds(1.0f);

    }
}
