using FMODUnity;
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

    [Header("SFX")]
    [SerializeField] private StudioEventEmitter _powerOnSound;
    [SerializeField] private StudioEventEmitter _powerOffSound;


    private SpriteRenderer _spriteRenderer;
    private bool _isSwitching;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsSelectable || _isSwitching) return;
        ToggleSwitch();
    }

    public void ToggleSwitch()
    {
        StartCoroutine(SwitchCoroutine());
    }

    public IEnumerator SwitchCoroutine()
    {
        _isSwitching = true;
        _spriteRenderer.sprite = _sprites[1];
        if (!IsSwitchedOn) _powerOnSound.Play(); else _powerOffSound.Play();
        yield return new WaitForSeconds(_switchTime);
        _isSwitching = false;
        Switch(!IsSwitchedOn);
    }

    private void Switch(bool isOn)
    {
        _spriteRenderer.sprite = isOn ? _sprites[2] : _sprites[0];
        IsSwitchedOn = isOn;
        OnSwitched?.Invoke(true);
    }

}

