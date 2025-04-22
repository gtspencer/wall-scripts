using System;
using System.Collections.Generic;
using UnityEngine;

public class WaitForLetterSystemWord : ScriptedEvent
{
    // sticker system is on side 1
    
    //I think you're being avoidant.  Do you do this with everyone in your life?
    //I think we should stay on topic.
    
    [SerializeField] private string _wordToWaitFor;

    [Tooltip("Custom responses to words typed in")]
    [Header("Custom Responses")]
    [SerializeField] private bool _customResponse;

    [SerializeField] private CustomResponse[] _customResponses;
    private Dictionary<string, string> _customResponseDict = new Dictionary<string, string>();
    
    [Space]
    [Tooltip("Cycle responses to push player in certain direction")]
    [Header("Cycle Responses")]
    [SerializeField] private bool _cycleResponse;
    [SerializeField] private string[] _cycleResponses;
    private int _cycleResponseIndex;

    private void Start()
    {
        if (!_customResponse) return;

        foreach (var customResponse in _customResponses)
            _customResponseDict.Add(customResponse.Key, customResponse.Value);
    }

    public void SetWord(string word)
    {
        if (string.Equals(word, _wordToWaitFor, StringComparison.InvariantCultureIgnoreCase))
        {
            Complete();
            return;
        }

        if (_customResponseDict.TryGetValue(word, out string response))
        {
            MainWallCanvas.Instance.WriteImmediateSide2(response);
        }
    }
    
    protected override void OnStart()
    {
        _cycleResponseIndex = 0;
        LetterStickerSystemManager.Instance.OnSentenceChanged += SetWord;
    }

    protected override void OnUpdate()
    {
    }

    protected override void OnEnd()
    {
        LetterStickerSystemManager.Instance.OnSentenceChanged -= SetWord;
    }

    protected override void OnEnteredSide1()
    {
    }

    protected override void OnEnteredSide2()
    {
        if (!_cycleResponse) return;
        
        if (_cycleResponseIndex >= _cycleResponses.Length)
            _cycleResponseIndex = 0;
        
        MainWallCanvas.Instance.WriteImmediateSide2(_cycleResponses[_cycleResponseIndex]);
        _cycleResponseIndex++;
    }
}

[Serializable]
public struct CustomResponse
{
    public string Key;
    public string Value;
}