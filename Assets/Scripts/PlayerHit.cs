using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    /// <summary>
    /// Damage the enemy by 1 if hit
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
            EnemyController e = collision.GetComponent<EnemyController>();

            if (e != null)
            {
                e.Damage(1);
            }

    }
}
