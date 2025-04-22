using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Slider3D : MonoBehaviour
{
    public Action<int> OnValueChanged;
    
    [SerializeField] private TextMeshProUGUI numberText;
    [SerializeField] private float minValue = .75f;
    [SerializeField] private float maxValue = -.75f;

    [SerializeField] private Transform knob;

    public void SetText(int initialValue)
    {
        numberText.text = initialValue.ToString();
    }
    
    // value is only "got" when we are updating it
    public int GetValue()
    {
        var input = knob.localPosition.x;
        
        // Clamp the input to the expected range of [-0.75, 0.75]
        // input = Mathf.Max(Mathf.Min(input, 0.75f), -0.75f);

        // Normalize input from range [0.75, -0.75] to [0, 1]
        // TODO normalize value extension
        var totalRange = Mathf.Abs(maxValue - minValue);
        var normalized = (minValue - input) / totalRange;

        // Map normalized value to range [0, 100]
        var value = (int)(normalized * 100);

        OnValueChanged?.Invoke(value);

        numberText.text = value.ToString();
        
        return value;
    }
}
