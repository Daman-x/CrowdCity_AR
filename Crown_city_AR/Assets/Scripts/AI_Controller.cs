using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

// Used to control basic behaivour of A.I Object like animation and inital position to go
public class AI_Controller : MonoBehaviour 
{
    public GameObject FinalEnd;
    public NavMeshAgent nav; // used for reference to nav agent
    Animator animator;

    public  bool _following = false; // public refernce to check if gameobject is following anyone
    // Start is called before the first frame update

    private void Awake()
    {
        FinalEnd = GameObject.FindGameObjectWithTag("End Point");
    }
    void Start()
    {
        nav.destination = FinalEnd.transform.position; // inital goes to 0,0,0 position , Could be randomize in future
        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {

        if (nav.remainingDistance > 0.1f)  // if gameobject didnt reached to the final destination
        {

            if (_following == true)  // is gameobject following anyone
            {
                animator.SetBool("IsIdle", false);
                animator.SetBool("Following", true);
            }
            else  // walk anim 
                animator.SetBool("IsWalking", true);
        }
        else // idle anim
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsIdle", true);
            animator.SetBool("Following", false);
           
        }
    }
}
