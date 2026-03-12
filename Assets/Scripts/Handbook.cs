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
    private string _animName;

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
        if (stateInfo.IsName(_animName) &&  stateInfo.normalizedTime >= 0.9f)
        {
            OnAnimationComplete();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        GoForward();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    private void GoForward()
    {
        if (IsDisabled) return;
        IsDisabled = true;

        if (_state == BookState.Closed) Open();
        else if (_pageIndex == _pageCount - 1) Close();
        else
        {
            _state = BookState.Turning;
            _pageAnim.Play("NextPage", 0, 0f);
            _animName = "NextPage";
            _pageIndex++;
            _interiorPage.sprite = _interiorSprites[_pageIndex];
        }
    }

    public void GoBackwards()
    {
        if (IsDisabled) return;

        IsDisabled = true;
        if (_pageIndex == 0) Close();
        else
        {
            _state = BookState.Turning;
            _pageAnim.Play("PreviousPage", 0, 0f);
            _animName = "PreviousPage";
            _pageIndex--;
        }
    }


    private void Open()
    {
        _state = BookState.Opened;
        _pageIndex = 0;
        _bookAnim.Play("BookOpen", 0, 0f);
        _animName = "BookOpen";
        //_bookAnim.ResetTrigger("Toggle");
        _backCollider.enabled = true;
        _interiorPage.sprite = _interiorSprites[0];
    }

    private void Close()
    {
        _state = BookState.Closed;
        _bookAnim.Play("BookClose", 0, 0f);
        _animName = "BookClose";
        //_bookAnim.ResetTrigger("Toggle");
        _backCollider.enabled = false;
    }


    public void OnAnimationComplete()
    {
        IsDisabled = false;
        _interiorPage.sprite = _interiorSprites[_pageIndex];
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
