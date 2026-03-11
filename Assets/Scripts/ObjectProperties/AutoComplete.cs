using UnityEngine;

public class AutoComplete : MonoBehaviour
{
    [SerializeField] private float _force;
    private PositionActivation _positionActivation;
    private Draggable _draggable;
    private Rigidbody2D _rb;
    [HideInInspector] public bool IsActive;
    private AutoReturn _autoReturn;

    private void Awake()
    {
        _positionActivation = GetComponent<PositionActivation>();
        _draggable = GetComponent<Draggable>();
        _rb = GetComponent<Rigidbody2D>();
        _autoReturn = GetComponent<AutoReturn>();
        IsActive = true;
    }

    private void FixedUpdate()
    {
        if (!IsActive || _autoReturn.IsActive) return;
        int direction = _positionActivation.IsActivated ? 1 : -1;
        if (!_draggable.IsSelected)
        {
            _rb.AddForce(direction * _force * Time.fixedDeltaTime * Vector2.down, ForceMode2D.Impulse);
        }
    }
}
