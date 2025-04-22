using System;
using System.Collections;
using TMPro;
using UnityEngine;

[Serializable]
public class IntroText
{
    public string Text;
    public float displayDuration;
    public float fadeInDuration;
}
public class IntroCredits : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textElement;
    [SerializeField] private IntroText[] _introTexts;
    [SerializeField] private float _initialWaitTime;

    private bool _sceneLoaded;
    
    void Start()
    {
        AsynSceneLoader.Instance.OnSceneLoaded += OnMainSceneLoaded;
        AsynSceneLoader.Instance.LoadScene(AsynSceneLoader.Scenes.MainScene);
        
        Color startColor = _textElement.color;
        startColor.a = 0;
        _textElement.color = startColor;

        StartCoroutine(DoIntroSequence());
    }

    public void OnMainSceneLoaded()
    {
        AsynSceneLoader.Instance.OnSceneLoaded -= OnMainSceneLoaded;
        
        _sceneLoaded = true;
    }

    private IEnumerator DoIntroSequence()
    {
        yield return new WaitForSeconds(_initialWaitTime);

        foreach (var element in _introTexts)
        {
            // fade in
            Color startColor = _textElement.color;
            startColor.a = 0;
            _textElement.color = startColor;

            _textElement.text = element.Text;

            float elapsedTime = 0f;

            while (elapsedTime < element.fadeInDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsedTime / element.fadeInDuration);
                _textElement.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }

            _textElement.color = new Color(startColor.r, startColor.g, startColor.b, 1f);

            yield return new WaitForSeconds(element.displayDuration);
            
            // fade out
            startColor = _textElement.color;
            startColor.a = 1;
            _textElement.color = startColor;
            
            elapsedTime = 0f;

            while (elapsedTime < element.fadeInDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsedTime / element.fadeInDuration);
                _textElement.color = new Color(startColor.r, startColor.g, startColor.b, 1 - alpha);
                yield return null;
            }
            
            _textElement.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        }

        yield return new WaitForSeconds(1f);

        while (!_sceneLoaded) yield return null;
        
        AsynSceneLoader.Instance.UnloadIntroScene();
    }
}
