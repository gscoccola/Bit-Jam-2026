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

    private void Start()
    {
        mainMenuPanel.SetActive(true);
        creditsPanel.SetActive(false);
        StartCoroutine(IMusicStart());
    }

    private IEnumerator IMusicStart()
    {
        _menuClick.Play();
        yield return new WaitForSecondsRealtime(1.5f);
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
        _BGM.Stop();
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene("Level");
    }

    public void ToggleCredits()
    {
        
        bool isCreditsActive = creditsPanel.activeSelf;
        if (!isCreditsActive ) _menuClick.Play(); else _menuBack.Play();
        mainMenuPanel.SetActive(isCreditsActive);
        creditsPanel.SetActive(!isCreditsActive);
    }
}
