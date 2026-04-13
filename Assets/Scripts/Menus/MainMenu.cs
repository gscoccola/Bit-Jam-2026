using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("SFX")]
    [SerializeField] private StudioEventEmitter _menuClick;
    [SerializeField] private StudioEventEmitter _menuBack;
    [SerializeField] private StudioEventEmitter _BGM;

    private Transition _transition;

    private void Start()
    {
        _transition = FindAnyObjectByType<Transition>();
        _transition.FinishedFadeIn.AddListener(() => SceneManager.LoadScene("Level"));
        mainMenuPanel.SetActive(true);
        creditsPanel.SetActive(false);
        _BGM.Play();
    }

    public void StartGame()
    {
        StartCoroutine(IStartGame());
    }

    private IEnumerator IStartGame()
    {
        _menuClick.Play();
        yield return new WaitForSeconds(0.25f);
        _transition.StartTransition();
        _BGM.Stop();
    }

    public void ToggleCredits()
    {
        
        bool isCreditsActive = creditsPanel.activeSelf;
        if (!isCreditsActive ) _menuClick.Play(); else _menuBack.Play();
        mainMenuPanel.SetActive(isCreditsActive);
        creditsPanel.SetActive(!isCreditsActive);
    }
}
