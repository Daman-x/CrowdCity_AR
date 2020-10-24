using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Used to attract A.I object with different players
public class Attracted : MonoBehaviour
{
    public Vector3 StoppingDistance;
    public bool taken = false; // is someone taken this object in his crew
    public GameObject following; // whom to follow
    AI_Controller ac;
    void Start() 
    {
        ac = gameObject.GetComponent<AI_Controller>(); 
    }

    // Update is called once per frame
    // Update A.I bodies location
    void Update()
    {

        if(taken == true)  // if following someone then
        {
            ac._following = true;
            if (following == null) // if not following anyone or player leaves
            {
                ac.nav.destination = new Vector3(0, 0, 0);
                ac._following = false;
                ac.nav.speed = 0.5f;
                taken = false;
            }
            else
            {
                ac.nav.destination = following.transform.position - StoppingDistance; // follow the leader
                ac.nav.speed = 0.9f;
            }
          
        }  
    }

    // called when a.i collide with player
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Player") && following != other.gameObject && taken == true)
        {
            if(other.gameObject.GetComponent<GainingCrowd>().crowdin == 0)
            {
                other.gameObject.GetComponent<PlayerSetup>().Isdead = true;
                other.gameObject.GetComponent<GainingCrowd>().enabled = false;
                other.gameObject.GetComponent<PlayerSetup>().KilledBy = following.GetComponent<PhotonView>().Owner.NickName;
            }
        }
    }
}
