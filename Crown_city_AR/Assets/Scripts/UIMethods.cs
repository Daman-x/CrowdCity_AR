using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class UIMethods : MonoBehaviourPunCallbacks
{

    [SerializeField] GameObject multiplayerManager;
    [SerializeField] Button SearchMatch;

    #region unity methods

    // Start is called before the first frame update
    void Start()
    {
        SearchMatch.gameObject.SetActive(true);
    }

    #endregion
    // join room method called
    #region u.i methods

    // called when click search for match button
    public void OnSearchForMatchClicked()
    {
        multiplayerManager.GetComponent<MultiplayerManager>().JoiningRooms();
        SearchMatch.gameObject.SetActive(false);
        Values.Started = true;
    }

    // Called when quit button clicked 'quit the application'
    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    #endregion
}
