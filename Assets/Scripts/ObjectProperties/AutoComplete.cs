using UnityEngine;

public class AutoComplete : MonoBehaviour
{
    [SerializeField] private float _force;
    private PositionActivation _positionActivation;
    private Draggable _draggable;
    private Rigidbody2D _rb;
    [HideInInspector] public bool IsActive;

    private void Awake()
    {
        _positionActivation = GetComponent<PositionActivation>();
        _draggable = GetComponent<Draggable>();
        _rb = GetComponent<Rigidbody2D>();
        IsActive = true;
    }

    private void FixedUpdate()
    {
        if (!IsActive) return;
        int direction = _positionActivation.IsActivated ? 1 : -1;
        if (!_draggable.IsSelected)
        {
            _rb.AddForce(direction * _force * Time.fixedDeltaTime * Vector2.down, ForceMode2D.Impulse);
        }
    }
}
