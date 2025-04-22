using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderHoldable : Holdable
{
    public Action<int> OnValueChanged;
    
    [SerializeField] private float minValue = .75f;
    [SerializeField] private float maxValue = -.75f;

    [SerializeField] private Slider3D slider3d;

    [SerializeField] private SliderHoldable otherSlider;

    private float startY;
    private float startZ;
    
    public void SetupInitialValue(int initialValue)
    {
        UpdateKnobValue(initialValue);
    }
    
    private void Update()
    {
        if (!IsHeld) return;

        var xValue = transform.localPosition.x;

        if (xValue > minValue)
            xValue = minValue;
        else if (xValue < maxValue)
            xValue = maxValue;

        transform.localPosition = new Vector3(xValue, startY, startZ);

        slider3d.GetValue();
    }

    private void UpdateKnobValue(int initialValue)
    {
        slider3d.SetText(initialValue);
        
        var percentage = initialValue / 100f;
        
        var totalRange = Mathf.Abs(maxValue - minValue);
        
        var value = percentage * totalRange;
        
        transform.localPosition = new Vector3(minValue - value, startY, startZ);
    }

    protected override void OnStart()
    {
        base.OnStart();
        
        startY = transform.localPosition.y;
        startZ = transform.localPosition.z;

        slider3d.OnValueChanged += ValueChanged;

        otherSlider.OnValueChanged += OtherSliderChanged;
    }

    private void OtherSliderChanged(int value)
    {
        UpdateKnobValue(value);
    }

    private void ValueChanged(int newValue)
    {
        OnValueChanged?.Invoke(newValue);
    }

    protected override void OnHovered()
    {
        
    }

    protected override void OnUnHovered()
    {
        
    }

    protected override void OnHeld()
    {
        
    }

    protected override void OnDropped()
    {
        
    }
}
