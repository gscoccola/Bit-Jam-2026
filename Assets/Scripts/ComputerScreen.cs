using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ComputerScreen : Singleton<ComputerScreen>
{
    [SerializeField] private TextMeshProUGUI _text;
    private Dictionary<CurrentLevel, RandomText> _postShockTexts;
    private RandomText _preShockTexts;
    private RandomText _unpoweredTexts;
    private RandomText _noOutputTexts;

    private void Start()
    {
        _postShockTexts = new Dictionary<CurrentLevel, RandomText>
        {
            { CurrentLevel.Safe, new RandomText( new[]{"Mhh... Suspicious", "What Happened?"}) },
            { CurrentLevel.Injury, new RandomText(new[]{"Not enough", "Unfortunate"}) },
            { CurrentLevel.Lethal, new RandomText(new[]{"Well done!", "Crispy..."}) },
        };

        _preShockTexts = new RandomText( new string[]
        {
            "Do it",
            "Dispense some justice",
            "Here we go!"
        });

        _unpoweredTexts = new RandomText(new string[]
        {
            "Power is off",
            "Switch on the power first!",
            "Pull the power switch...",
        });

        _noOutputTexts = new RandomText(new string[]
        {
            "No Output",
            "Switch on the output",
            "Pull the output switch...",
        });
    }

    public void PostShockText(CurrentLevel currentLevel)
    {
        _text.text = _postShockTexts[currentLevel].GetText();
    }

    public void PreShockText()
    {
        _text.text = _preShockTexts.GetText();
    }

    public void UnpoweredText()
    {
        _text.text = _unpoweredTexts.GetText();
    }

    internal void NoOutputText()
    {
        _text.text = _noOutputTexts.GetText();
    }
}


public class RandomText
{
    private string[] _texts;
    private bool _wasUsed;
    private int _lastIndex;
    public RandomText(string[] texts)
    {
        _texts = texts;
    }
    public string GetText()
    {
        int index;

        if (!_wasUsed || _texts.Length == 1)
        {
            index = Random.Range(0, _texts.Length);
        }
        else
        {
            index = Random.Range(0, _texts.Length - 1);
            if (index >= _lastIndex) index = (index + 1);
        }


        _wasUsed = true;
        _lastIndex = index;
        return _texts[index];
    }
}