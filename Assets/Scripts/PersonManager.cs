using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PersonManager : Singleton<PersonManager>
{
    [SerializeField] private Person _personPrefab;
    [SerializeField] private float _lenght;

    [HideInInspector]  public Person CurrentPerson;

    private void Start()
    {
        SpawnPerson();
    }

    private void SpawnPerson()
    {
        CurrentPerson = Instantiate(_personPrefab, transform.position - _lenght * Vector3.right, Quaternion.identity, transform);
        CurrentPerson.transform.DOMove(transform.position, 2f).OnComplete(() => ComputerScreen.Instance.PreShockText());
    }

    public void ShockPerson(CurrentLevel currentLevel)
    {
        CurrentPerson.Shock(currentLevel);
        ComputerScreen.Instance.PostShockText(currentLevel);
    }

    public void RemovePerson()
    {
        StartCoroutine(RemovePersonCoroutine());
    }

    private IEnumerator RemovePersonCoroutine()
    {
        CurrentPerson.transform.DOMove(transform.position + _lenght * Vector3.right, 2f);
        yield return new WaitForSeconds(2.8f);
        
        Destroy(CurrentPerson.gameObject);
        SpawnPerson();
    }
}
