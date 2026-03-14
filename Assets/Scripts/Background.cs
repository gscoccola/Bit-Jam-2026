using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private float _frequency = 0.1f;
    [SerializeField] private float _amplitude = 0.3f;

    private Vector3 _initialPos;

    private void Start()
    {
        _initialPos = transform.position;
    }

    private void Update()
    {
        //transform.localPosition = _initialPos + Vector3.right * Mathf.PingPong(Time.time * _frequency, _amplitude) ;
        transform.localScale = Vector3.one + Vector3.right * Mathf.PingPong(Time.time * _frequency, _amplitude) ;
    }
}
