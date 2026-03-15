using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{

    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        SceneManager.LoadScene("MainMenu");
    }
}
