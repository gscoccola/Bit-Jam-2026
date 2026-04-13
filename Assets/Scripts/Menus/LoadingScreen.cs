using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    private Transition _transition;

    private void Start()
    {
        _transition = FindAnyObjectByType<Transition>();
        _transition.FinishedFadeIn.AddListener(()=> SceneManager.LoadScene("MainMenu"));
    }

    public void OnClick()
    {
        _transition.StartTransition();
    }
}
