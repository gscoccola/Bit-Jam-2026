using System.Collections;
using UnityEngine;
using FMODUnity;

public class PersonManager : Singleton<PersonManager>
{
    [SerializeField] private GameObject _personPrefab;
    [SerializeField]  private Animator _windowAnimator;
    [SerializeField] private float _shockDuration = 2f;

    [HideInInspector]  public GameObject CurrentPerson;
    [SerializeField] private PrisonerInfoSO[] _prisonerInfo;

    private int _prisonerIndex;
    private Animator _personAnimator;


    [Header ("SFX")]
    [SerializeField] private StudioEventEmitter _chargeSound;
    [SerializeField] private StudioEventEmitter _shockSound;
    [SerializeField] private StudioEventEmitter _activationSound;
    [SerializeField] private StudioEventEmitter _shutterOpen;
    [SerializeField] private StudioEventEmitter _shutterClose;
    [SerializeField] private StudioEventEmitter _BGM;

    private void Start()
    {
        SpawnPerson();
    }

    private void SpawnPerson()
    {
        StartCoroutine(IPersonSpawn());
        ComputerScreen.Instance.SetPrisonerInfo(_prisonerInfo[_prisonerIndex]);
    }

    private IEnumerator IPersonSpawn()
    {
        _windowAnimator.Play("Open");
        _shutterOpen.Play();
        CurrentPerson = Instantiate(_personPrefab, transform.position, Quaternion.identity, transform);
        _personAnimator = CurrentPerson.GetComponent<Animator>();
        yield return new WaitForSeconds(3f);
        //_shutterSound.Stop();
        ComputerScreen.Instance.PreShockText();
        ControlPanel.Instance.SetSelectable(true);
    }

    public void StartShock(CurrentLevel level, float magnitude)
    {
        _activationSound.Play();
        StartCoroutine(ShockCoroutine(level, magnitude));
    }

    private IEnumerator ShockCoroutine(CurrentLevel level, float magnitude)
    {
        yield return new WaitForSeconds(0.3f);

        _chargeSound.Play();
        _shockSound.Play(); 
        _chargeSound.SetParameter("ChargeLevel", magnitude * 100f);
        _personAnimator.Play("Criminal_Shock");
        _BGM.SetParameter("LowPass", 50f);

        yield return new WaitForSeconds(_shockDuration);

        ControlPanel.Instance.StartWindown();

        yield return new WaitForSeconds(0.3f);

        StopShock(level);
        _BGM.SetParameter("LowPass", 100f);

        yield return new WaitForSeconds(0.2f);

        ComputerScreen.Instance.PostShockText(level);

        yield return new WaitForSeconds(2.8f);

        _windowAnimator.Play("Close");
        _shutterClose.Play();

        yield return new WaitForSeconds(3f);

        _shutterClose.Stop();

        yield return new WaitForSeconds(3f);

        GameManager.Instance.EvaluateAction(_prisonerInfo[_prisonerIndex], level);
        _prisonerIndex++;
        if (_prisonerIndex >= _prisonerInfo.Length)
        {
            GameManager.Instance.TriggerEnding();
            yield break;
        }
        Destroy(CurrentPerson.gameObject);
        SpawnPerson();
    }

    public void StopShock(CurrentLevel level)
    {
        _chargeSound.Stop();
        switch (level)
        {
            case CurrentLevel.Safe:
                _personAnimator.Play("Criminal_Idle");
                break;
            case CurrentLevel.Injury:
                _personAnimator.Play("Criminal_Injury");
                break;
            case CurrentLevel.Lethal:
                _personAnimator.Play("Criminal_Death");
                break;
        }
    }

}
