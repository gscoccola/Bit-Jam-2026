using FMODUnity;
using System.Collections;
using UnityEngine;

public class MusicAttempt : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<StudioEventEmitter>().Play();
    }
}
