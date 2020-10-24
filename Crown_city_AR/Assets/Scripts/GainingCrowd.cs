using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

//Used to gain crowd by connecting with A.I Objects
public class GainingCrowd : MonoBehaviourPun
{
    [Header("Sounds")]
    public AudioSource Gained;

    Attracted attracted;
    public int crowdin = 0;
    public List<GameObject> followers; // list of object following the player

    #region unity methods
    //used to update crowd no. on text
    // Update is called once per frame
    void Update()
    {
        crowdin = followers.Count;
        if (gameObject.GetComponent<PhotonView>().IsMine)
        {
            gameObject.GetComponent<PhotonView>().RPC("Crowd", RpcTarget.AllBuffered, followers.Count.ToString());
            crowdin = followers.Count;
        }
    }

    // called when collide with anything
    private void OnTriggerEnter(Collider other)
    {
        attracted = other.GetComponent<Attracted>();
        if (other.gameObject.CompareTag("A.I OBJECTS")) // if collided with A.i object tagged as A.I OBJECTS
        {
            if (attracted.taken == false) // when object is not taken by anyone
            {
                if (gameObject.GetComponent<PhotonView>().IsMine) // called by this gameobject only
                {
                    gameObject.GetComponent<PhotonView>().RPC("AddToCrowdI", RpcTarget.AllBuffered, other.GetComponentInParent<PhotonView>().ViewID);
                }
            }
            else if (attracted.taken == true && attracted.following != gameObject)
            {
                if (gameObject.GetComponent<PhotonView>().IsMine)
                {
                    gameObject.GetComponent<PhotonView>().RPC("AddToOthersI", RpcTarget.AllBuffered, other.GetComponentInParent<PhotonView>().ViewID);
                }
            }
        }
    }

    #endregion

    #region rpc methods

    // used to show other player the total no of crowd we have and update our text amount
    [PunRPC]
    public void Crowd(string crowd)
    {
        gameObject.GetComponentInChildren<TextMesh>().text = crowd;
    }

    // used to add object in the list of the players folllowers 
    [PunRPC]
    public void AddToCrowdI(int id)
    {
        if (Values.MusicOn == true)
        { Gained.Play(); }
        GameObject obj = PhotonView.Find(id).gameObject;
        AddingFollower(obj,gameObject,null);
    }

    // used when we have to add followers to other player rather then us
    [PunRPC]
    public void AddToOthersI(int id)
    {
        GameObject obj = PhotonView.Find(id).gameObject;
        GameObject playerFollowing =  obj.GetComponent<Attracted>().following;

        if(playerFollowing.GetComponent<GainingCrowd>().followers.Count < followers.Count) // if other player crowd is more than ours
        {
            if (Values.MusicOn == true)
            { Gained.Play(); }
            AddingFollower(obj , gameObject , playerFollowing); // called adding following func
        }
        else if (playerFollowing.GetComponent<GainingCrowd>().followers.Count > followers.Count) // if other player crowd is less than ours
        {
            if(gameObject.GetComponent<GainingCrowd>().followers.Count > 0) // our crowd is more than 0
            {
                Debug.Log(followers.Count);
                GameObject RemObj = followers[followers.Count - 1];
                followers.RemoveAt(followers.Count - 1);
                AddingFollower(RemObj, playerFollowing, null);
            }
        }
    }

    #endregion

    #region user define methods

    // Adds follower in list
    public void AddingFollower(GameObject AddObj , GameObject FollowingTo ,GameObject WasFollowingTo)
    {

        if(WasFollowingTo == null) // if not following to anyone
        {
            FollowingTo.GetComponent<GainingCrowd>().followers.Add(AddObj);
            AddObj.GetComponentInChildren<Renderer>().material = FollowingTo.GetComponentInChildren<Renderer>().material;
            AddObj.GetComponent<Attracted>().taken = true;
            AddObj.GetComponent<Attracted>().following = FollowingTo;
           
        }
        else
        {
            WasFollowingTo.GetComponent<GainingCrowd>().followers.Remove(AddObj);
            FollowingTo.GetComponent<GainingCrowd>().followers.Add(AddObj);
            AddObj.GetComponentInChildren<Renderer>().material = FollowingTo.GetComponentInChildren<Renderer>().material;
            AddObj.GetComponent<Attracted>().taken = true;
            AddObj.GetComponent<Attracted>().following = FollowingTo;
        }  
    }
    #endregion

}
