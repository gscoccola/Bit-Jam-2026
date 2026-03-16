using UnityEngine;

[CreateAssetMenu(fileName = "NewPrisonerInfo", menuName = "ScriptableObjects/PrisonerInfo", order = 1)]
public class PrisonerInfoSO : ScriptableObject
{
    public int Index;

    [TextArea(10, 100)]
    public string Information;

    public string[] Charges;

    public CurrentLevel Sentence;
}
