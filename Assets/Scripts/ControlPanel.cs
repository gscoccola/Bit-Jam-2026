using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float _shockDuration = 2f;
    [SerializeField] private float _currentInjuryThreshold = 0.3f;
    [SerializeField] private float _currentLethalThreshold = 0.6f;


    [Header("Control Panel Components")]
    [SerializeField] private PositionActivation _powerSwitch;
    [SerializeField] private PositionActivation _currentDial;
    [SerializeField] private PositionActivation _engageLever;

    [Header("Displays")]
    [SerializeField] private Image _powerLight;
    [SerializeField] private Slider _currentSlider;
    [SerializeField] private Image _engagedLight;

    private AutoReturn _leverAutoReturn;
    private AutoReturn _switchAutoReturn;
    private AutoComplete _switchAutoComplete;
    private void Awake()
    {
        _powerSwitch.Activated.AddListener(_powerLight.gameObject.SetActive);
        _currentDial.ValueChanged.AddListener(value => _currentSlider.value = value);
        _engageLever.Activated.AddListener(TryEngageChair);
        _leverAutoReturn = _engageLever.GetComponent<AutoReturn>();
        _switchAutoReturn = _powerSwitch.GetComponent<AutoReturn>();
        _switchAutoComplete = _powerSwitch.GetComponent<AutoComplete>();
    }

    private void TryEngageChair(bool isLeverEngaged)
    {
        if (isLeverEngaged)
        {
            if (_powerSwitch.IsActivated)
            {
                _engagedLight.gameObject.SetActive(true);
                StartShock();
            }
            else ComputerScreen.Instance.UnpoweredText();
        }
        else _engagedLight.gameObject.SetActive(false);
    }

    private void StartShock()
    {
        StartCoroutine(ShockCoroutine());
    }

    private IEnumerator ShockCoroutine()
    {
        _leverAutoReturn.IsActive = false;
        _engageLever.GetComponent<Draggable>().IsSelectable = false;
        PersonManager.Instance.ShockPerson(GetCurrentLevel());
        yield return new WaitForSeconds(_shockDuration);
        _switchAutoReturn.IsActive = true;
        _switchAutoComplete.IsActive = false;
        yield return new WaitForSeconds(0.3f);
        _switchAutoComplete.IsActive = true;
        _engageLever.GetComponent<Draggable>().IsSelectable = true;
        _switchAutoReturn.IsActive = false;
        _leverAutoReturn.IsActive = true;
        yield return new WaitForSeconds(0.2f);
        PersonManager.Instance.RemovePerson();
    }

    private CurrentLevel GetCurrentLevel()
    {
        float currentValue = _currentDial.Magnitude;
        if (currentValue < _currentInjuryThreshold) return CurrentLevel.Safe;
        else if (currentValue < _currentLethalThreshold) return CurrentLevel.Injury;
        else return CurrentLevel.Lethal;
    }
}


public enum CurrentLevel
{
    Safe,
    Injury,
    Lethal
}