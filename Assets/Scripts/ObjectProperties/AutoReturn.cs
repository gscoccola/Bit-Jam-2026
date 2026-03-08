using UnityEngine;

public class AutoReturn : MonoBehaviour
{
    [SerializeField] private float _force;

    private Draggable _draggable;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _draggable = GetComponent<Draggable>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!_draggable.IsSelected)
        {
            _rb.AddForce(_force * Time.fixedDeltaTime * Vector2.up, ForceMode2D.Impulse);
        }
    }
}
