using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;

// Used to define local player functions 
public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] Color color;
    [SerializeField] Color KilledColor; // used to image background of warning text for killed
    [SerializeField] Color WonColor;
    [SerializeField] Color Other_Color;
    [SerializeField] AudioSource Died;

    GameObject maingame;
    public string KilledBy;
    public bool assignedcolor = false; // color assigned or not to this gameobject used in assigncolor class
    public bool Isdead = false;
    public bool IsWon = false; // used by playerwon script 

    GameObject warning;
    private float _timer = 0f;
    private string _Won = "You Won!";

    // Start is called before the first frame update
    void Start()
    {
        // used so our player dont control other players movement
        if (photonView.IsMine) // if we are the player
        {
            transform.GetComponent<ControllingPlayer>().enabled = true;
            
        }
        else // if remote player
        {
            transform.GetComponent<ControllingPlayer>().enabled = false;
            transform.GetComponent<PlayerSetup>().enabled = false;
            //  transform.GetComponent<GainingCrowd>().enabled = false;
        }
        PlayerNumberColor(); // used to mark the player
        warning = GameObject.FindGameObjectWithTag("Finish");
        maingame = GameObject.Find("Main Game");
    }

    // check u died or won 
    void Update()
    {
        if (Isdead == true) // player died or not
        {
            Debug.Log("not here");
            if (photonView.IsMine)
            {
                maingame.SetActive(false);
                GetComponent<ControllingPlayer>().enabled = false;
                warning.GetComponent<Text>().text = "Killed By " + KilledBy;
                warning.GetComponentInChildren<Image>().color = KilledColor;
                if (Values.MusicOn == true)
                { Died.Play(); }

                if (PhotonNetwork.InRoom == true && _timer > 5f)
                {
                    warning.GetComponent<Text>().text = "";
                    Leave();
                }
                _timer += Time.deltaTime;
            }
        }
        if (IsWon == true) // player won or not
        {
            if (photonView.IsMine)
            {
                GetComponent<ControllingPlayer>().enabled = false;
                warning.GetComponent<Text>().text = _Won;
                warning.GetComponentInChildren<Image>().color = WonColor;
            }
            else
            {
                GetComponent<ControllingPlayer>().enabled = false;
                warning.GetComponent<Text>().text = "Better Luck Next Time";
                warning.GetComponentInChildren<Image>().color = KilledColor;
            }
        }
    }

    #region user define method

    // used to assign different colors to textmesh of other players and ours also
    void PlayerNumberColor()
    {
        if (photonView.IsMine)
        {
            gameObject.GetComponentInChildren<TextMesh>().color = color;
        }
        else
        {
            gameObject.GetComponentInChildren<TextMesh>().color = Other_Color;
        }
    }

    // used to leave the room and go back to menu scene
    public void Leave()
    {
        Values.Started = false;
       // Values.MATCHISON = false;
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Menu Scene");
    }
    #endregion
}
