using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TypewriterEffect : MonoBehaviour
{
    public float TypingSpeed = 0.05f;

    public void TypeText(TextMeshProUGUI target, string content)
    {
        StartCoroutine(Typewriter(target, content));
    }

    private IEnumerator Typewriter(TextMeshProUGUI target, string content)
    {
        target.text = content;
        target.maxVisibleCharacters = 0;

        for (int i = 0; i <= content.Length; i++)
        {
            target.maxVisibleCharacters = i;
            yield return new WaitForSeconds(TypingSpeed);
        }
    }
}