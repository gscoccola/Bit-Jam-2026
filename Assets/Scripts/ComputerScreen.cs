using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ComputerScreen : Singleton<ComputerScreen>
{
    private TextMeshProUGUI _channelText;
    private TextMeshProUGUI _infoText;
    private Dictionary<CurrentLevel, RandomText> _postShockTexts;
    private RandomText _preShockTexts;
    private RandomText _unpoweredTexts;
    private RandomText _noOutputTexts;
    private GameObject _infoTab;
    private GameObject _channelTab;

    private void Start()
    {
        _channelTab = transform.GetChild(0).gameObject;
        _channelText = _channelTab.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();

        _infoTab = transform.GetChild(1).gameObject;
        _infoText = _infoTab.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();

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

        DisplayInfo();

    }

    public void PostShockText(CurrentLevel currentLevel)
    {
        DisplayMessage(_postShockTexts[currentLevel].GetText());
    }

    public void PreShockText()
    {
        DisplayMessage(_preShockTexts.GetText());
    }

    public void UnpoweredText()
    {
        DisplayMessage(_unpoweredTexts.GetText());
    }

    public void NoOutputText()
    {
        DisplayMessage(_noOutputTexts.GetText());
    }

    private void DisplayMessage(string message)
    {
        _channelTab.SetActive(true);
        _infoTab.SetActive(false);
        _channelText.text = message;
    }

    public void DisplayInfo()
    {
        _channelTab.SetActive(false);
        _infoTab.SetActive(true);
    }
    public void DisplayChannel()
    {
        _channelTab.SetActive(true);
        _infoTab.SetActive(false);
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