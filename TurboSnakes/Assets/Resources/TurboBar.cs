using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurboBar : MonoBehaviour
{
    public Slider turboSlider;

    public void SetMaxTurbo(int turbo)
    {
        turboSlider.maxValue = turbo;
        turboSlider.value = turbo;
    }

    public void SetTurbo(int turbo)
    {
        turboSlider.value = turbo;
    }
}

