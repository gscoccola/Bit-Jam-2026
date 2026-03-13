using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class WorldButton : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent OnClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClick.Invoke();
    }
}
