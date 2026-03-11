using UnityEngine;

public class AutoReturn : MonoBehaviour
{
    [SerializeField] private float _force;
    [SerializeField] private bool _startsActive;

    private Draggable _draggable;
    private Rigidbody2D _rb;
    public bool IsActive;

    private void Awake()
    {
        _draggable = GetComponent<Draggable>();
        _rb = GetComponent<Rigidbody2D>();
        IsActive = _startsActive;

    }

    private void FixedUpdate()
    {
        if (IsActive && !_draggable.IsSelected)
        {
            _rb.AddForce(_force * Time.fixedDeltaTime * Vector2.up, ForceMode2D.Impulse);
        }
    }
}
