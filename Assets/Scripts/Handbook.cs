using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

public class Handbook : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private GameObject[] _interiorPages;

    [Header("SFX")]
    [SerializeField] private StudioEventEmitter _openSound;
    [SerializeField] private StudioEventEmitter _pageTurnSound;

    private GameObject _zoomedIn;
    private GameObject _zoomedOut;
    private Animator _bookAnim;
    private Animator _pageAnim;


    private int _pageIndex;
    private int _pageCount = 3;
    private BookState _state;
    public bool IsDisabled;
    private string _animName;
    //private bool _isZoomedIn;

    private void Awake()
    {
        _zoomedOut = transform.GetChild(0).gameObject;

        _zoomedIn = transform.GetChild(1).gameObject;
        _bookAnim = _zoomedIn.transform.GetChild(0).GetComponent<Animator>();
        _pageAnim = _zoomedIn.transform.GetChild(1).GetComponent<Animator>();


        _zoomedOut.SetActive(true);
        _zoomedIn.SetActive(false);

        foreach (GameObject page in _interiorPages)
        {
            page.SetActive(false);
        }
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

        if (_state == BookState.Closed) Open();
        else if (_pageIndex == _pageCount - 1) Close();
        
        else
        {
            IsDisabled = true;
            _pageTurnSound.Play();
            _state = BookState.Turning;
            _pageAnim.Play("NextPage", 0, 0f);
            _animName = "NextPage";
            _pageIndex++;
            SetInteriorPage(_pageIndex);
        }
    }

    private void SetInteriorPage(int index)
    {
        foreach (GameObject page in _interiorPages)
        {
            page.SetActive(false);
        }
        _interiorPages[index].SetActive(true);
    }

    public void GoBackwards()
    {
        if (IsDisabled) return;

        if (_pageIndex == 0) Close();
        
        else
        {
            IsDisabled = true;
            _pageTurnSound.Play();
            _state = BookState.Turning;
            _pageAnim.Play("PreviousPage", 0, 0f);
            _animName = "PreviousPage";
            _pageIndex--;
        }
    }


    private void Open()
    {
        IsDisabled = true;
        _openSound.Play();
        ToggleZoom();
        _state = BookState.Opened;
        _pageIndex = 0;
        _bookAnim.Play("BookOpen", 0, 0f);
        _animName = "BookOpen";
        SetInteriorPage(0);
    }

    public void Close()
    {
        if (IsDisabled) return;

        IsDisabled = true;

        _openSound.Play();
        _state = BookState.Closed;
        _bookAnim.Play("BookClose", 0, 0f);
        _animName = "BookClose";
    }

    private void ToggleZoom()
    {
        /*Vector3 translation = new Vector3(-1f, -2f, 0f);
        if (_isZoomedIn)
        {
            transform.localScale = Vector2.one;
            transform.position -= translation;
        }
        else
        {
            transform.localScale = Vector2.one * 2f;
            transform.position += translation;
        }
        _isZoomedIn = !_isZoomedIn;*/
        _zoomedOut.SetActive(!_zoomedOut.activeSelf);
        _zoomedIn.SetActive(!_zoomedIn.activeSelf);
    }

    public void OnAnimationComplete()
    {
        IsDisabled = false;
        SetInteriorPage(_pageIndex);
        if (_state == BookState.Closed)
        {
            ToggleZoom();
        }
        if (_state == BookState.Turning)
        {
            _state = BookState.Opened;
        }

    }

    public void OnGameOver()
    {
        if (_state == BookState.Opened)
        {
            Close();
        }
        IsDisabled = true;
    }
    
    public enum BookState
    {
        Closed,
        Opened,
        Turning,
    }
}
