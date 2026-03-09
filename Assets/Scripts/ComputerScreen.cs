using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ComputerScreen : Singleton<ComputerScreen>
{
    [SerializeField] private TextMeshProUGUI _text;
    private Dictionary<CurrentLevel, string[]> _postShockTexts;
    private string[] _preShockTexts;
    private string[] _unpoweredTexts;

    private void Start()
    {
        _postShockTexts = new Dictionary<CurrentLevel, string[]>
        {
            { CurrentLevel.Safe, new[]{"Mhh... Suspicious", "What Happened?"} },
            { CurrentLevel.Injury, new[]{"Not enough", "Unfortunate"} },
            { CurrentLevel.Lethal, new[]{"Well done!", "Crispy..."} },
        };

        _preShockTexts = new string[]
        {
            "Do it",
            "Dispense some justice",
            "Here we go!"
        };

        _unpoweredTexts = new string[]
        {
            "Power is off",
            "Switch on the power first!",
        };
    }

    public void PostShockText(CurrentLevel currentLevel)
    {
        _text.text = _postShockTexts[currentLevel][Random.Range(0, _postShockTexts[currentLevel].Length)];
    }

    public void PreShockText()
    {
        _text.text = _preShockTexts[Random.Range(0, _preShockTexts.Length)];
    }

    public void UnpoweredText()
    {
        _text.text = _unpoweredTexts[Random.Range(0, _unpoweredTexts.Length)];
    }
}
