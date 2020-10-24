using Photon.Pun;
using UnityEngine;

// used to assign Colors to local player 
public class AssignColor : MonoBehaviourPun
{
    public Material[] _materials;
    private GameObject[] _players;

    // find all the materials
    // assign materials to the player acc. to the no of players
    private void Start()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");
        if(photonView.IsMine)
        {
            gameObject.GetComponentInChildren<Renderer>().material = _materials[_players.Length - 1];
            GetComponent<PlayerSetup>().assignedcolor = true;
        }
    }
    private void Update()
    {
        if(photonView.IsMine && Values.Started == true)
        {
            int i = 0;
            _players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject obj in _players)
            {
                if(obj.GetComponent<PlayerSetup>().assignedcolor == true)
                {
                    i++;
                }
                else
                {
                    obj.GetComponentInChildren<Renderer>().material = _materials[i];
                    i++;
                }
            }
        }
        this.enabled = false;
    }


}
