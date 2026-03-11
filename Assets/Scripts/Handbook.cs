using UnityEngine;
using UnityEngine.EventSystems;

public class Handbook : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Sprite[] _interiorSprites;

    private Animator _bookAnim;
    private Animator _pageAnim;
    private int _pageIndex;
    private int _pageCount = 3;
    private BookState _state;
    private SpriteRenderer _interiorPage;
    public bool IsDisabled;
    private BoxCollider2D _backCollider;

    private void Awake()
    {
        _bookAnim = GetComponent<Animator>();
        _pageAnim = transform.GetChild(0).GetComponent<Animator>();
        _interiorPage = transform.GetChild(1).GetComponent<SpriteRenderer>();
        _backCollider = transform.GetChild(2).GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (!IsDisabled) return;

        AnimatorStateInfo stateInfo = _state == BookState.Turning ? _pageAnim.GetCurrentAnimatorStateInfo(0): _bookAnim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1.0f)
        {
            OnAnimationComplete();
            Debug.Log("Animation complete");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        OnClick();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    private void OnClick()
    {
        if (IsDisabled) return;

        IsDisabled = true;

        if (_state == BookState.Closed)
        {
            _state = BookState.Opened;
            _bookAnim.SetTrigger("Toggle");
            _interiorPage.sprite = _interiorSprites[0];
        }
        else if (_pageIndex == _pageCount - 1)
        {
            _pageIndex = 0;
            _state = BookState.Closed;
            _bookAnim.SetTrigger("Toggle");
            _backCollider.enabled = false;
        }
        else
        {
            _state = BookState.Turning;
            _pageAnim.SetTrigger("NextPage");
            _pageIndex++;
            _interiorPage.sprite = _interiorSprites[_pageIndex];
        }
    }

    public void GoBackwards()
    {
        if (IsDisabled) return;

        IsDisabled = true;
        if (_pageIndex == 0)
        {
            _pageIndex = 0;
            _state = BookState.Closed;
            _bookAnim.SetTrigger("Toggle");
            _backCollider.enabled = false;
        }
        else
        {
            _state = BookState.Turning;
            _pageAnim.SetTrigger("PreviousPage");
            _pageIndex--;
        }
    }

    public void OnAnimationComplete()
    {
        IsDisabled = false;
        _interiorPage.sprite = _interiorSprites[_pageIndex];
        if (_state == BookState.Opened)
        {
            _backCollider.enabled = true;
        }
        if (_state == BookState.Turning)
        {
            _state = BookState.Opened;
        }
    }
    
    public enum BookState
    {
        Closed,
        Opened,
        Turning,
    }
}
