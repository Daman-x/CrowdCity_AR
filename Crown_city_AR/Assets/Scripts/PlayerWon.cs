using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// used to determine which player won.
public class PlayerWon : MonoBehaviour
{
    [SerializeField] GameObject[] PlayersInMatch;
    [SerializeField] GameObject timerVisual;
    private float _timerLatter = 00;
    private int _timerFront = 2;

   [SerializeField] GameObject Winnner;

    private int _placeholder = 0;
    private Text text;
    private float _timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        _timerFront = 2;
        _timerLatter = 00;
        text = timerVisual.GetComponentInChildren<Text>();
        timerVisual.SetActive(false);
        PlayersInMatch = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        if(Values.Started == true)
        {
            timerVisual.SetActive(true);
        }

        if(Values.MATCHISON == true)
        {
            if (_timerLatter <= 0f && _timerFront >= 1)
            {
                _timerFront -= 1;
                _timerLatter = 60;
            }
            text.text = (_timerLatter < 10) ? _timerFront + ":" + _placeholder + (int)_timerLatter : _timerFront + ":" + (int)_timerLatter;  // Timer visuals

            // calling playerwinning func
            PlayerWining();
            _timerLatter -= Time.deltaTime;

            if (_timerLatter <= 0f && _timerFront == 0) // if timer is equals 0:00
            {
                Won(); // won called
            }
            if(PhotonNetwork.CurrentRoom.PlayerCount == 1) // if only one player is in the room
            {
                Won();
                Debug.Log("win");
            }
            
        }
        
    }

    #region user define methods

    // used to find all the players in the game and store the record of the player with the highest crowd
    void PlayerWining()
    {
        PlayersInMatch = GameObject.FindGameObjectsWithTag("Player");
        int i = 0;
       
        if(Winnner == null)
        {
            Winnner = PlayersInMatch[i];
        }

        while (PlayersInMatch.Length > i)
        {
            if(Winnner.GetComponent<GainingCrowd>().crowdin < PlayersInMatch[i].GetComponent<GainingCrowd>().crowdin)
            {
                Winnner = PlayersInMatch[i];
            }
            i++;
        }
    }

    // used to show the player that u win and others lose and exit the match
    void Won()
    {
        Values.Started = false;
        timerVisual.SetActive(false);
        _timer += Time.deltaTime;
        Winnner.GetComponent<PlayerSetup>().IsWon = true; // iswon in playersteup script
        if(_timer > 3f)
        {
            Debug.Log("in");
          //  PhotonNetwork.CurrentRoom.IsVisible = false;
            int i = 0;
            while(i < PlayersInMatch.Length)
            {
                PlayersInMatch[i].GetComponent<PlayerSetup>().Leave(); // leave fun in playersetup script
                i++;
            }
        }
    }

    #endregion
}
