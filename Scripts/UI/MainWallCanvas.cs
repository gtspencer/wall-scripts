using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainWallCanvas : MonoBehaviour
{
    public static MainWallCanvas Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }

    [SerializeField] private TextMeshProUGUI side1Text;
    [SerializeField] private TextMeshProUGUI side2Text;

    private void Start()
    {
        RemoveTextSide1();
        RemoveTextSide2();
    }

    public void WriteImmediateSide1(string text)
    {
        side1Text.text = text;
    }
    
    public void WriteImmediateSide2(string text)
    {
        side2Text.text = text;
    }

    public void RemoveTextSide1()
    {
        WriteImmediateSide1("");
    }
    
    public void RemoveTextSide2()
    {
        WriteImmediateSide2("");
    }

    public void ToggleTextHidden(bool hidden)
    {
        side1Text.gameObject.SetActive(!hidden);
        side2Text.gameObject.SetActive(!hidden);
    }
}
