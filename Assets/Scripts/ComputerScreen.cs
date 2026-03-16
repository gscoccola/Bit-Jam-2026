using FMODUnity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComputerScreen : Singleton<ComputerScreen>
{
    private TextMeshProUGUI _channelText;
    private Dictionary<CurrentLevel, RandomText> _postShockTexts;
    private RandomText _preShockTexts;
    private RandomText _unpoweredTexts;
    private RandomText _noOutputTexts;
    private GameObject _infoTab;
    private GameObject _channelTab;
    private RectTransform _infoRect;
    private TypewriterEffect _typer;
    private int _handPrintIndex;
    [HideInInspector] public bool DisplaySuspicionMessage;

    [Header("SFX")]
    [SerializeField] private StudioEventEmitter _notificationSound;
    [SerializeField] private StudioEventEmitter _tabSound;

    [Header("PrisonerInfo")]
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField]  private ScrollRect _scrollView;
    [SerializeField]  private Sprite[] _handPrints;
    [SerializeField]  private SpriteRenderer _handPrint;
    private void Start()
    {
        _channelTab = transform.GetChild(0).gameObject;
        _channelText = _channelTab.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();

        _infoTab = transform.GetChild(1).gameObject;
        _infoRect = _infoText.transform.parent.GetComponent<RectTransform>();

        _typer = GetComponent<TypewriterEffect>();

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

        DisplayInfo(false);
    }

    public void PostShockText(CurrentLevel currentLevel)
    {
        DisplayMessage(_postShockTexts[currentLevel].GetText());
    }

    public void PreShockText()
    {
        if (!DisplaySuspicionMessage) return;
        DisplaySuspicionMessage = false;
        string text = "";
        if (GameManager.Instance.Suspicion == 1) text = "You won't like the consequences of messing this up";
        else if (GameManager.Instance.Suspicion == 2) text = "THIS IS YOUR LAST WARNING, DON'T FUCK IT UP";
        else if (GameManager.Instance.Suspicion == 3) return;

        DisplayMessage(text);
    }

    public void UnpoweredText()
    {
        DisplayMessage(_unpoweredTexts.GetText());
    }

    public void NoOutputText()
    {
        DisplayMessage(_noOutputTexts.GetText());
    }

    public void DisplayMessage(string message)
    {
        _notificationSound.Play();
        _channelTab.SetActive(true);
        _infoTab.SetActive(false);
        _typer.TypeText(_channelText, message);
        //_channelText.text = message;
    }

    public void DisplayInfo(bool playSound = true)
    {
        if (playSound) _tabSound.Play();
        _channelTab.SetActive(false);
        _infoTab.SetActive(true);
    }
    public void DisplayChannel()
    {
        _tabSound.Play();
        _channelTab.SetActive(true);
        _infoTab.SetActive(false);
    }

    public void SetPrisonerInfo(PrisonerInfoSO info)
    {
        _infoText.text = "\n" + info.Information + "\n\nCharges:\n" + string.Join("\n", info.Charges) + "\n\nSentence: " + GetSentence(info.Sentence) + " \n\n" + " ";
        _handPrintIndex = (_handPrintIndex +1 )% _handPrints.Length;
        _handPrint.sprite = _handPrints[_handPrintIndex];
        //float width = _infoText.preferredWidth;
        float height = _infoText.preferredHeight;
        _infoRect.sizeDelta = new Vector2(_infoRect.sizeDelta.x, height + 25f);
        _scrollView.verticalNormalizedPosition = 1f;
    }

    private string GetSentence(CurrentLevel level)
    {
        if (level == CurrentLevel.Safe) return "Minor Corrective";
        else if (level == CurrentLevel.Injury) return "Major Corrective";
        else return "Deletion";
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