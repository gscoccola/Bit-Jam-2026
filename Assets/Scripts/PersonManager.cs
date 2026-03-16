using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonManager : Singleton<PersonManager>
{
    [SerializeField] private GameObject _personPrefab;
    [SerializeField]  private Animator _windowAnimator;
    [SerializeField] private float _shockDuration = 2f;

    [HideInInspector]  public GameObject CurrentPerson;
    [SerializeField] private PrisonerInfoSO[] _prisonerInfo;

    private int _prisonerIndex;
    private Animator _personAnimator;
    private int _personType;

    [Header ("SFX")]
    [SerializeField] private StudioEventEmitter _chargeSound;
    [SerializeField] private StudioEventEmitter _shockSound;
    [SerializeField] private StudioEventEmitter _activationSound;
    [SerializeField] private StudioEventEmitter _shutterOpen;
    [SerializeField] private StudioEventEmitter _shutterClose;
    [SerializeField] private StudioEventEmitter _sitChair;
    [SerializeField] private StudioEventEmitter _footstep;

    private StudioEventEmitter _BGM;
    public Dictionary<CurrentLevel, ActionResult>[] ActionResults;
    public Dictionary<CurrentLevel, string>[] ActionMessage;

    private void Start()
    {
        SetupDictionary();
        _BGM = GameManager.Instance.GetComponent<StudioEventEmitter>();
        SpawnPerson();
    }




    private void SpawnPerson()
    {
        StartCoroutine(IPersonSpawn());
        ComputerScreen.Instance.SetPrisonerInfo(_prisonerInfo[_prisonerIndex]);
    }

    private IEnumerator IPersonSpawn()
    {

        _footstep.Play();

        yield return new WaitForSeconds(1.5f);

        _sitChair.Play();
        yield return new WaitForSeconds(0.5f);


        _windowAnimator.Play("Open");
        _shutterOpen.Play();
        CurrentPerson = Instantiate(_personPrefab, transform.position, Quaternion.identity, transform);
        _personAnimator = CurrentPerson.GetComponent<Animator>();

        if (_personType == 0) _personAnimator.Play("Criminal_Idle");
        else _personAnimator.Play("Criminal_Idle2");


        

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
        if (_personType == 0) _personAnimator.Play("Criminal_Shock");
        else _personAnimator.Play("Criminal_Shock2");

        _BGM.SetParameter("LowPass", 50f);

        yield return new WaitForSeconds(_shockDuration);

        ControlPanel.Instance.StartWindown();

        yield return new WaitForSeconds(0.3f);

        StopShock(level);
        _BGM.SetParameter("LowPass", 100f);

        yield return new WaitForSeconds(0.2f);

        ComputerScreen.Instance.DisplayMessage(ActionMessage[_prisonerIndex][level]);

        yield return new WaitForSeconds(2.8f);

        _windowAnimator.Play("Close");
        _shutterClose.Play();

        yield return new WaitForSeconds(3f);

        _shutterClose.Stop();

        yield return new WaitForSeconds(1.5f);

        _sitChair.Play();

        yield return new WaitForSeconds(1.5f);



 
        GameManager.Instance.EvaluateAction(ActionResults[_prisonerIndex][level]);
        if (GameManager.Instance.IsGameOver) yield break;

        _prisonerIndex++;
        if (_prisonerIndex >= _prisonerInfo.Length)
        {
            GameManager.Instance.EvaluateEnding();
            yield break;
        }
        Destroy(CurrentPerson.gameObject);
        _personType = (_personType + 1) % 2;
        SpawnPerson();
    }

    public void StopShock(CurrentLevel level)
    {
        _chargeSound.Stop();
        switch (level)
        {
            case CurrentLevel.Safe:
                if (_personType == 0) _personAnimator.Play("Criminal_Idle");
                else _personAnimator.Play("Criminal_Idle2");
                break;
            case CurrentLevel.Injury:
                if (_personType == 0) _personAnimator.Play("Criminal_Injury");
                else _personAnimator.Play("Criminal_Injury2");
                break;
            case CurrentLevel.Lethal:
                if (_personType == 0) _personAnimator.Play("Criminal_Death");
                else _personAnimator.Play("Criminal_Death2");
                break;
        }
    }

    private void SetupDictionary()
    {
        ActionResults = new Dictionary<CurrentLevel, ActionResult>[_prisonerInfo.Length];
        ActionMessage = new Dictionary<CurrentLevel, string>[_prisonerInfo.Length];

        //obviously guilty, set for execution
        ActionResults[0] = new Dictionary<CurrentLevel, ActionResult>
        {
            { CurrentLevel.Safe, new ActionResult (false, false, true) },
            { CurrentLevel.Injury, new ActionResult (false, false, true) },
            { CurrentLevel.Lethal, new ActionResult (false, false, false) }
        };
        ActionMessage[0] = new Dictionary<CurrentLevel, string>
        {
            { CurrentLevel.Safe, "That wasn't supposed to happen, this is not a good look for us" },
            { CurrentLevel.Injury, "Your current was a bit short, try better next time" },
            { CurrentLevel.Lethal, "Good riddance..." }
        };

        // probably innocent, chosen to make an example, set for execution
        /*ActionResults[1] = new Dictionary<CurrentLevel, ActionResult>
        {
            { CurrentLevel.Safe, new ActionResult (true, false, true) },
            { CurrentLevel.Injury, new ActionResult (false, false, false) },
            { CurrentLevel.Lethal, new ActionResult (false, false, false) }
        };
        ActionMessage[1] = new Dictionary<CurrentLevel, string>
        {
            { CurrentLevel.Safe, "You can't just do what you want" },
            { CurrentLevel.Injury, "He still lives. Maybe they got the message either way" },
            { CurrentLevel.Lethal, "It's unfortunate, but it's for the greater good" }
        };*/

        //rebel graffiti, set for major corrective
        ActionResults[1] = new Dictionary<CurrentLevel, ActionResult>
        {
            { CurrentLevel.Safe, new ActionResult (true, true, true) },
            { CurrentLevel.Injury, new ActionResult (false, false, false) },
            { CurrentLevel.Lethal, new ActionResult (false, false, false) }
        };
        ActionMessage[1] = new Dictionary<CurrentLevel, string>
        {
            { CurrentLevel.Safe, "You wouldn't happen to be a sympathizer?" },
            { CurrentLevel.Injury, "Well done, he will think twice next time" },
            { CurrentLevel.Lethal, "You went a bit too far, but he was rebel scum anyway" }
        };

        //obviously guilty, set for major corrective
        ActionResults[2] = new Dictionary<CurrentLevel, ActionResult>
        {
            { CurrentLevel.Safe, new ActionResult (false, false, true) },
            { CurrentLevel.Injury, new ActionResult (false, false, false) },
            { CurrentLevel.Lethal, new ActionResult (false, false, false) }
        };
        ActionMessage[2] = new Dictionary<CurrentLevel, string>
        {
            { CurrentLevel.Safe, "The evidence was pretty clear this time, perhaps you're just incompetent?" },
            { CurrentLevel.Injury, "Justice was served" },
            { CurrentLevel.Lethal, "Woops... current a bit too high..." }
        };

        //probably innocent, caught to close a case, set for execution
        ActionResults[3] = new Dictionary<CurrentLevel, ActionResult>
        {
            { CurrentLevel.Safe, new ActionResult (true, false, true) },
            { CurrentLevel.Injury, new ActionResult (false, false, true) },
            { CurrentLevel.Lethal, new ActionResult (false, false, false) }
        };
        ActionMessage[3] = new Dictionary<CurrentLevel, string>
        {
            { CurrentLevel.Safe, "You can't make up your own rules" },
            { CurrentLevel.Injury, "You missed the mark that time" },
            { CurrentLevel.Lethal, "He was just at the wrong place at the wrong time I guess" }
        };

        //rebel terrorist, responsible for multiple attacks, set for death
        ActionResults[4] = new Dictionary<CurrentLevel, ActionResult>
        {
            { CurrentLevel.Safe, new ActionResult (false, true, true) },
            { CurrentLevel.Injury, new ActionResult (false, true, true) },
            { CurrentLevel.Lethal, new ActionResult (false, false, false) }
        };
        ActionMessage[4] = new Dictionary<CurrentLevel, string>
        {
            { CurrentLevel.Safe, "I'm really hoping you're just having an off day" },
            { CurrentLevel.Injury, "That's not enough, not by a long shot" },
            { CurrentLevel.Lethal, "Finally, it's over" }
        };


    }

}
