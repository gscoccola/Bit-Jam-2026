using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PositionActivation : MonoBehaviour
{
    [SerializeField] private ActivationType _activationType;
    [SerializeField] private PositionCheck _positionCheck;
    [SerializeField] private float _threshold;
    [SerializeField] private GameObject _target;

    public bool IsActivated;
    public float Magnitude;

    [HideInInspector] public UnityEvent<float> ValueChanged;
    [HideInInspector] public UnityEvent<bool> Activated;

    private float _initialValue;
    private float _currentValue;
    private HingeJoint2D _hingeJoint;
    private Slider _slider;
    private Image _image;
    private float _range = 1f;

    private void Awake()
    {
        if (_positionCheck == PositionCheck.Rotation)
        {
            _hingeJoint = GetComponent<HingeJoint2D>();
            if (_hingeJoint != null ) _range = _hingeJoint.limits.max - _hingeJoint.limits.min;
        } 

        if (_activationType == ActivationType.Continuous)
        {
            _slider = _target.GetComponent<Slider>();
            if (_slider == null)
            {
                Debug.LogError("Target GameObject must have a Slider component for Continuous activation.");
            }
        }
        else if (_activationType == ActivationType.Discrete)
        {
            _image = _target.GetComponent<Image>();
            if (_image == null)
            {
                Debug.LogError("Target GameObject must have an Image component for Discrete activation.");
            }
        }
    }

    private void Start()
    {
        _initialValue = GetCurrentValue();
        _currentValue = _initialValue;
    }

    private void FixedUpdate()
    {
        if (_currentValue == GetCurrentValue()) return;
        _currentValue = GetCurrentValue();

        if (_activationType == ActivationType.Continuous)
        {
            Magnitude = Mathf.Abs(_currentValue - _initialValue) / _range;
            _slider.value = Magnitude;
            ValueChanged?.Invoke(Magnitude);
        }
        else if (_activationType == ActivationType.Discrete)
        {
            bool wasActivated = IsActivated;
            IsActivated = Mathf.Abs(_currentValue - _initialValue) >= _threshold;
            _image.enabled = IsActivated;
            if (IsActivated != wasActivated)
            {
                Activated?.Invoke(IsActivated);
            }
        }
    }

    private float GetCurrentValue()
    {
        return _positionCheck switch
        {
            PositionCheck.XAxis => transform.position.x,
            PositionCheck.YAxis => transform.position.y,
            PositionCheck.Rotation => _hingeJoint.jointAngle,
            _ => 0f
        };
    }
}


public enum ActivationType
{
    Discrete,
    Continuous,
}

public enum PositionCheck
{
    XAxis,
    YAxis,
    Rotation,
}