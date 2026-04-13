using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Transition : MonoBehaviour
{
    [SerializeField] private float _transitionDuration = 2f;
    private Material _objectMaterial;
    private bool _fadeIn;
    private float _elapsedTime;
    public UnityEvent FinishedFadeIn;
    private Image _image;

    private void Start()
    {
         _image  = GetComponent<Image>();
        _objectMaterial = _image.material;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;  

        if (!_fadeIn)
        {
            if (_elapsedTime < _transitionDuration) _objectMaterial.SetFloat("_Dither", 2f * (1f - _elapsedTime / _transitionDuration));
            else _image.enabled = false;
        }

        else
        {
            _objectMaterial.SetFloat("_Dither", 2f * _elapsedTime / _transitionDuration);
            if (_elapsedTime > _transitionDuration)
            {
                _fadeIn = false;
                FinishedFadeIn.Invoke();
            }
        }
    }

    public void StartTransition()
    {
        _fadeIn = true;
        _image.enabled = true;
        _elapsedTime = 0f;
    }
}
