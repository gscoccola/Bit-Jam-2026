using UnityEngine;

[CreateAssetMenu(fileName = "NewPrisonerInfo", menuName = "ScriptableObjects/PrisonerInfo", order = 1)]
public class PrisonerInfoSO : ScriptableObject
{
    [TextArea(10, 100)]
    public string Information;

    public string[] Charges;

    public CurrentLevel Sentence;

    public int RebelScale;

    public int MoralityScale;
}
