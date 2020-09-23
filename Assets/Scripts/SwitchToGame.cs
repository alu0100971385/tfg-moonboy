﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SwitchToGame : MonoBehaviour
{
    /// <summary>
    /// Switch to a certain scene
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}