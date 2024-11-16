using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarHandler : MonoBehaviour
{
    public static LifeBarHandler Instance { get; private set; }

    [SerializeField] LifeBarItem _lifeBarItemPrefab; 

    private List<LifeBarItem> _allLifeBars;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _allLifeBars = new List<LifeBarItem>();
    }


    public LifeBarItem CreateNewLifeBarItem(LifeHandler owner)
    {
        var newLifeBar = Instantiate(_lifeBarItemPrefab, transform)
                            .Initialize(owner.transform);

        _allLifeBars.Add(newLifeBar);

        owner.OnDespawn += () =>
        {
            Destroy(newLifeBar.gameObject);
            _allLifeBars.Remove(newLifeBar);
        };

        return newLifeBar;
    }

    private void LateUpdate()
    {
        foreach (var lifeBarItem in _allLifeBars)
        {
            lifeBarItem.UpdatePosition();
        }
    }
}
