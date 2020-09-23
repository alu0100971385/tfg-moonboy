using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ExitOnClick : MonoBehaviour
{
    /// <summary>
    /// Exits game
    /// </summary>
    public void ExitGame()
    {
        Debug.Log("saliendo");
        Application.Quit();
    }
}
