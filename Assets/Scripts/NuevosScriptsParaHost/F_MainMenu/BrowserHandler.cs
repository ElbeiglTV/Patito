using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BrowserHandler : MonoBehaviour
{
    [SerializeField] SessionItem _sessionItemPrefab;
    [SerializeField] NetworkRunnerHandler _networkRunnerHandler;
    [SerializeField] TMP_Text _statusText;
    [SerializeField] VerticalLayoutGroup _verticalLayout;

    private void OnEnable()
    {
        _networkRunnerHandler.OnSessionListUpdate += UpdateList;
    }

    private void OnDisable()
    {
        _networkRunnerHandler.OnSessionListUpdate -= UpdateList;
    }

    void ClearBrowser()//borra todo el browser
    {
        foreach (Transform session in _verticalLayout.transform)
        {
            Destroy(session.gameObject);
        }

        _statusText.gameObject.SetActive(false);
    }

    private void UpdateList(List<SessionInfo> sessions)//metodo que actualiza el panel de salas activas con la lista de sesiones, usa clear browser para no duplicar las anteriores
    {
        ClearBrowser();

        if (sessions.Count == 0 ) 
        {
            NoSessionsFound();
            return;
        }

        foreach (SessionInfo session in sessions)
        {
            AddNewSessionToBrowser(session);
        }
    }

    void NoSessionsFound()//metodo que activa el texto Sessions Not Found
    {
        _statusText.text = "Sessions Not Found";
        _statusText.gameObject.SetActive(true);
    }

    void AddNewSessionToBrowser(SessionInfo newSession)//metodo que crea una nueva sesion en el browser
    {
        var sessionItem = Instantiate(_sessionItemPrefab, _verticalLayout.transform);
        sessionItem.Initialize(newSession);
        sessionItem.OnSessionItemClick += JoinSelectedSession;
    }

    void JoinSelectedSession(SessionInfo sessionInfo)
    {
        _networkRunnerHandler.JoinGame(sessionInfo);
    }
}
