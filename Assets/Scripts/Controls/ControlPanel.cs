using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : Singleton<ControlPanel>
{
    [Header("Parameters")]
    [SerializeField] private float _currentInjuryThreshold = 0.3f;
    [SerializeField] private float _currentLethalThreshold = 0.6f;


    [Header("Control Panel Components")]
    [SerializeField] private DiscreteButton _powerSwitch;
    [SerializeField] private PositionActivation _currentDial;
    [SerializeField] private PositionActivation _engageLever;
    [SerializeField] private PositionActivation _outputSwitch;


    [Header("Displays")]
    [SerializeField] private Slider _currentSlider;
    [SerializeField] private Sprite[] _outputIndicatorSprites;
    [SerializeField] private SpriteRenderer _outputIndicator;

    private AutoReturn _leverAutoReturn;
    private Draggable _currentDialDraggable;
    private Draggable _engageDialDraggable;
    private Draggable _outputSwitchDraggable;
    private AutoReturn _outputSwitchAutoreturn;
    protected override void Awake()
    {
        base.Awake();
        _currentDial.ValueChanged.AddListener(value => _currentSlider.value = value);
        _engageLever.Activated.AddListener(TryEngageChair);
        _outputSwitch.Activated.AddListener(isOn => _outputIndicator.sprite = isOn ? _outputIndicatorSprites[1] : _outputIndicatorSprites[0]);
        _leverAutoReturn = _engageLever.GetComponent<AutoReturn>();
        _currentDialDraggable = _currentDial.GetComponent<Draggable>();
        _engageDialDraggable = _engageLever.GetComponent<Draggable>();
        _outputSwitchDraggable = _outputSwitch.GetComponent<Draggable>();
        _outputSwitchAutoreturn = _outputSwitch.GetComponent<AutoReturn>();
    }

    private void TryEngageChair(bool isLeverEngaged)
    {
        if (isLeverEngaged)
        {
            if (_powerSwitch.IsSwitchedOn)
            {
                if (_outputSwitch.IsActivated) StartShock();
                else ComputerScreen.Instance.NoOutputText();
            }
            else ComputerScreen.Instance.UnpoweredText();
        }
        //else _engagedLight.gameObject.SetActive(false);
    }

    private void StartShock()
    {
        _leverAutoReturn.IsActive = false;
        CurrentLevel currentLevel = GetCurrentLevel();
        PersonManager.Instance.StartShock(currentLevel, _currentDial.Magnitude);
        SetSelectable(false);
    }

    public void StartWindown()
    {
        StartCoroutine(WindownCoroutine());
    }

    private IEnumerator WindownCoroutine()
    {
        _outputSwitchAutoreturn.IsActive = true;
        yield return new WaitForSeconds(0.3f);
        _outputSwitchAutoreturn.IsActive = false;
        _leverAutoReturn.IsActive = true;
        _powerSwitch.ToggleSwitch();
    }

    public void SetSelectable(bool isSelectable)
    {
        _powerSwitch.IsSelectable = isSelectable;
        _currentDialDraggable.IsSelectable = isSelectable;
        _engageDialDraggable.IsSelectable = isSelectable;
        _outputSwitchDraggable.IsSelectable = isSelectable;
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