using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    private TextMeshProUGUI text;

    private Vector3 _startingScale;
    [SerializeField] private float _hoverScaleFactor = 1.1f;
    [SerializeField] private float _hoverScaleDuration = 0.15f;

    // public delegate void HoverAction();
    // public event HoverAction OnHoverEnter;
    // public event HoverAction OnHoverExit;

    private void Awake()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        _startingScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHovered();
        // text.outlineWidth = .2f;
        // OnHoverEnter?.Invoke();
    }

    private void OnHovered()
    {
        LeanTween.scale(gameObject, _startingScale * _hoverScaleFactor, _hoverScaleDuration).setEaseInOutElastic().setIgnoreTimeScale(true);;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnUnhovered();
        // text.outlineWidth = 0;
        // OnHoverExit?.Invoke();
    }

    private void OnUnhovered()
    {
        LeanTween.scale(gameObject, _startingScale, _hoverScaleDuration).setEasePunch().setEaseInOutElastic().setIgnoreTimeScale(true);;
    }
}