using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool IsGameOver;
    public int Suspicion;
    public int Morality;
    public int Defiance;
    private StudioEventEmitter _studioEventEmitter;
    [SerializeField] private TextMeshProUGUI _endingText;
    [SerializeField] private TextMeshProUGUI _flavorText;

    private void Start()
    {
        _studioEventEmitter = GetComponent<StudioEventEmitter>();
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
        _studioEventEmitter.SetParameter("Suspicion", Suspicion * 33f);

        if (Suspicion == 3)
        {
            ComputerScreen.Instance.DisplayMessage("You were warned...");
            StartCoroutine(IBadEnding());
        }

    }

    private IEnumerator IBadEnding()
    {
        yield return new WaitForSeconds(2f);
        TriggerEnding(Endings.Executed);
    }

    public void EvaluateEnding()
    {
        IsGameOver = true;
        if (Morality == 2) TriggerEnding(Endings.Just);
        else if (Defiance == 2) TriggerEnding(Endings.Rebel);
        else TriggerEnding(Endings.Incospicuous);
    }

    private void TriggerEnding(Endings ending)
    {
        Debug.Log("Game Ended");
        transform.GetChild(0).gameObject.SetActive(true);
        _endingText.text = $"Ending {((int)ending + 1).ToString()}/4, {ending.ToString()}";
        if (ending == Endings.Executed) _flavorText.text = "You were executed for your crimes";
        else if (ending == Endings.Rebel) _flavorText.text = "You helped the rebellion";
        else if (ending == Endings.Incospicuous) _flavorText.text = "You just did your job";
        else if (ending == Endings.Just) _flavorText.text = "You stood up for the innocent";
    }


    public void MainMenu()
    {
        _studioEventEmitter.Stop();
        SceneManager.LoadScene("MainMenu");
    }

}

public enum Endings
{
    Executed,
    Just,
    Rebel,
    Incospicuous
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