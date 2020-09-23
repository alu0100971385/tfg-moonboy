using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    
    /// <summary>
    /// On collision, it damages the main character unless it's blocking
    /// </summary>
    /// <param name="collision"></param>
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

    /// <summary>
    /// Parrying function
    /// </summary>
    /// <param name="ec"></param>
    /// <returns></returns>
    IEnumerator Parried(EnemyController ec)
    {
        ec.currentState = EnemyState.parried;
        ec.time_parried = ec.max_parried;
        yield return new WaitForSeconds(1.0f);

    }
}
