using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject creditsPanel;

    private void Start()
    {
        mainMenuPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void ToggleCredits()
    {
        bool isCreditsActive = creditsPanel.activeSelf;
        mainMenuPanel.SetActive(isCreditsActive);
        creditsPanel.SetActive(!isCreditsActive);
    }
}
