using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    /// <summary>
    /// Damages main character by one if it detects collision
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay2D(Collider2D other)
    {
        MoonboyController controller = other.GetComponent<MoonboyController>();

        if (controller != null)
        {
            controller.ChangeHealth(-1);
        }
    }
}
