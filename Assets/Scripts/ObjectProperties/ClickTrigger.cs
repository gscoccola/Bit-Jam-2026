using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickTrigger : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent _onClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        _onClick.Invoke();
    }
}
