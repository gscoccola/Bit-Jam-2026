using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class DiscreteButton : MonoBehaviour, IPointerDownHandler
{
    public bool IsSelectable = true;
    public bool IsSwitchedOn;

    public UnityEvent<bool> OnSwitched;

    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private float _switchTime = 0.15f;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsSelectable) return;
        ToggleSwitch();
    }

    public void ToggleSwitch()
    {
        StartCoroutine(SwitchCoroutine());
    }

    public IEnumerator SwitchCoroutine()
    {
        IsSelectable = false;
        _spriteRenderer.sprite = _sprites[1];
        yield return new WaitForSeconds(_switchTime);
        IsSelectable = true;
        Switch(!IsSwitchedOn);
    }

    private void Switch(bool isOn)
    {
        _spriteRenderer.sprite = isOn ? _sprites[2] : _sprites[0];
        IsSwitchedOn = isOn;
        OnSwitched?.Invoke(true);
    }

}

