using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    public bool IsGameOver;
    public int Suspicion;
    public int Morality;
    public int Defiance;
    [SerializeField] private TextMeshProUGUI _endingText;
    [SerializeField] private TextMeshProUGUI _flavorText;
    [SerializeField] private float _bagOverHeadDuration = 0.8f;
    [SerializeField] private float _footApproachDuration = 4f;
    [SerializeField] private Handbook _handbook;
    private Transform _bagOverHeadSprite;
    private Vector3 _coffeeCupInitialPos;

    [Header("SFX")]
    [SerializeField] private StudioEventEmitter musicSound;
    [SerializeField] private StudioEventEmitter _executeSound;

    [SerializeField] private StudioEventEmitter _cupMove;
    [SerializeField] private StudioEventEmitter _cupSlurp;
    [SerializeField] private StudioEventEmitter _cupPlace;

    [SerializeField] private Transform _coffeeMug;
    [SerializeField] private Animator _coffeeMugBelow;
    [SerializeField] private float _coffeeMoveDuration = 1.2f;

    private void Start()
    {
        _bagOverHeadSprite = transform.GetChild(1);
        _coffeeCupInitialPos = _coffeeMug.position;
    }


    public void EvaluateAction(ActionResult result)
    {
        
        if (result.RaisedMorality) Morality++;
        if (result.RaisedDefiance) Defiance++;
        if (result.RaisedSuspicion)
        {
            Suspicion++;
            ComputerScreen.Instance.DisplaySuspicionMessage = true;
        }
        musicSound.SetParameter("Suspicion", Suspicion * 33f);

        if (Suspicion == 3)
        {
            ComputerScreen.Instance.DisplayMessage("You were warned...");
            StartCoroutine(IBadEnding());
            return;
        }
        //EvaluateEnding();
    }

    private IEnumerator IBadEnding()
    {
        IsGameOver = true;
        _handbook.OnGameOver();
        yield return new WaitForSeconds(1f);
        _executeSound.Play();
        yield return new WaitForSeconds(_footApproachDuration);
        _bagOverHeadSprite.DOMove(Vector2.down * 1.8f, _bagOverHeadDuration);
        yield return new WaitForSeconds(8f - _footApproachDuration);
        TriggerEndingScreen(Endings.Executed);
    }

    public void EvaluateEnding()
    {
        if (IsGameOver) return;
        _handbook.OnGameOver();
        IsGameOver = true;
        StartCoroutine(IGoodEnding());
    }

    private IEnumerator IGoodEnding()
    {
        yield return new WaitForSeconds(1f);
        _coffeeMug.DOMove(new Vector2(0.3f, -7.3f), _coffeeMoveDuration);
        _cupMove.Play();
        yield return new WaitForSeconds(_coffeeMoveDuration);

        _cupSlurp.Play();
        _coffeeMugBelow.Play("Drink");

        yield return new WaitForSeconds(3f);
        _coffeeMug.DOMove(_coffeeCupInitialPos, _coffeeMoveDuration).OnComplete(() => _cupPlace.Play());
        yield return new WaitForSeconds(_coffeeMoveDuration + 1f);

        if (Morality == 2) TriggerEndingScreen(Endings.Just);
        else if (Defiance == 2) TriggerEndingScreen(Endings.Rebel);
        else TriggerEndingScreen(Endings.Inconspicuous);
    }

    private void TriggerEndingScreen(Endings ending)
    {
        Debug.Log("Game Ended");
        transform.GetChild(0).gameObject.SetActive(true);
        _endingText.text = $"Ending {((int)ending + 1).ToString()}/4, {ending.ToString()}";
        if (ending == Endings.Executed) _flavorText.text = "You were executed for your disobedience";
        else if (ending == Endings.Rebel) _flavorText.text = "You helped the rebellion grow";
        else if (ending == Endings.Just) _flavorText.text = "You stood up for the innocent";
        else if (ending == Endings.Inconspicuous) _flavorText.text = "You just did your job";
    }


    public void MainMenu()
    {
        musicSound.Stop();
        SceneManager.LoadScene("MainMenu");
    }

}

public enum Endings
{
    Executed,
    Just,
    Rebel,
    Inconspicuous
}

[System.Serializable]
public class ActionResult
{
    public bool RaisedMorality;
    public bool RaisedDefiance;
    public bool RaisedSuspicion;

    public ActionResult(bool raisedMorality, bool raisedDefiance, bool raisedSuspicion)
    {
        RaisedMorality = raisedMorality;
        RaisedDefiance = raisedDefiance;
        RaisedSuspicion = raisedSuspicion;
    }

}