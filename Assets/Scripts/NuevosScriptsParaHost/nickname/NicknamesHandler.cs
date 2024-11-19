using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicknamesHandler : MonoBehaviour
{
    public static NicknamesHandler Instance { get; private set; }

    [SerializeField] private NicknameItem _nicknameItemPrefab;

    private List<NicknameItem> _allNicknames;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _allNicknames = new List<NicknameItem>();
    }


    public NicknameItem CreateNewNicknameItem(ShowNickname owner)
    {
        //crea un Nickname Item
        var newNickname = Instantiate(_nicknameItemPrefab, transform)
                            .Initialize(owner.transform);

        //agrega a la lista
        _allNicknames.Add(newNickname);

        //suscribe al evento de muerte del jugador para que cuando se ejecute, se destruya el nickname que le pertenece y lo saque de la lista
        owner.OnDespawn += () =>
        {
            Destroy(newNickname.gameObject);
            _allNicknames.Remove(newNickname);
        };

        //4- Devuelve el nickname creado
        return newNickname;
    }
     
    private void LateUpdate()
    {
        foreach (var nicknameItem in _allNicknames)
        {
            nicknameItem.UpdatePosition();
        }
    }
}
