using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedGunShotPool : Singleton<SharedGunShotPool>
{
    [SerializeField]
    private float _poolSize;
    [SerializeField] 
    private GameObject _shotPrefab;
    [SerializeField] 
    private GameObject _shotsParentObject;
    
    private Queue<GameObject> _gunShotsPool;

    protected override void Awake()
    {
        base.Awake();

        _gunShotsPool = new Queue<GameObject>();
        for (var i = 0; i < _poolSize; i++)
        {
            var shot = Instantiate(_shotPrefab, _shotsParentObject.transform);
            shot.SetActive(false);
            _gunShotsPool.Enqueue(shot);
        }
    }

    public GameObject GetNextShot()
    {
        return _gunShotsPool.Dequeue();
    }

    public void ReturnShot(GameObject shot)
    {
        StartCoroutine(DisableShot(shot));
        _gunShotsPool.Enqueue(shot);
    }

    private IEnumerator DisableShot(GameObject shot)
    {
        yield return new WaitForSeconds(0.5f);
        shot.SetActive(false);
    }
}