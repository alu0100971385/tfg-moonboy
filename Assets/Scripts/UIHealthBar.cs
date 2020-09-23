using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{

    public static UIHealthBar instance { get; private set; }

    public Image mask;
    float originalSize;

    /// <summary>
    /// Instantiates the bar on wake up
    /// </summary>
    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Sets original size
    /// </summary>
    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }

    /// <summary>
    /// Resize the bar to fit the actual health status
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
}
