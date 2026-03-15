using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int Suspicion;
    public int Morality;
    public int Defiance;

    public void EvaluateAction(PrisonerInfoSO prisonerInfo, CurrentLevel appliedCurrent)
    {

    }

    public void TriggerEnding()
    {
        Debug.Log("Game Ended");
    }
}

public enum Endings
{
    Executed,
    Just,
    Rebel,
    Incospicuous
}
