using UnityEngine;

public class Person : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Shock(CurrentLevel level)
    {
        switch (level)
        {
            case CurrentLevel.Safe:
                
                break;
            case CurrentLevel.Injury:
                _spriteRenderer.color = Color.yellow;
                break;
            case CurrentLevel.Lethal:
                _spriteRenderer.color = Color.red;
                break;
        }
    }
}
