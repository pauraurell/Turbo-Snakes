using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakBar : MonoBehaviour
{
    public Slider breakSlider;

    public void SetMaxBreak(int turbo)
    {
        breakSlider.maxValue = turbo;
        breakSlider.value = turbo;
    }

    public void SetBreak(int turbo)
    {
        breakSlider.value = turbo;
    }
}

