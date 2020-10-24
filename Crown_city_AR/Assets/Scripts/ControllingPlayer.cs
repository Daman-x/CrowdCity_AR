using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to control character movement and animation
public class ControllingPlayer : MonoBehaviour 
{
  
    public Joystick joystick; // joystick used
    public float speed = 6f;

    CharacterController CharacterController;
    private Animator animator;

    private float _y;
    private float _SmoothVelocityRef; // reference valueto store smooth velocity in smooth velocity
   
    // Start is called before the first frame of Update
    private void Start()
    {
        CharacterController = gameObject.GetComponent<CharacterController>(); // reference to charcter controller
        animator = gameObject.GetComponent<Animator>(); // reference to animator
        joystick = GameObject.FindObjectOfType<Joystick>();
        _y = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = joystick.Horizontal; // taking input from joystick
        float vertical = joystick.Vertical;

        Vector3 direction = new Vector3(horizontal, 0f, vertical); // new vector3 for direction to move

        if(direction.magnitude >= 0.1f && Values.MATCHISON == true) // Is joystick moved and match is on
        {
            animator.SetBool("IsMoving", true); // run animation will be true to execute
            float look_direction = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; // used to rotate player where the joystick is pointing or player is going
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, look_direction, ref _SmoothVelocityRef, 0.1f); // smoothing velocity to rotate smoothly
            transform.rotation = Quaternion.Euler(0f, angle, 0f); // applying smooth rotation
            CharacterController.Move(direction * speed * Time.deltaTime); // moving character
        }
        else
            animator.SetBool("IsMoving", false); // to execite idle state if not moving


        if(transform.position.y > _y)
        {
            Vector3 vector = new Vector3(0f, (transform.position.y - _y) , 0f);
            transform.position -= vector;
        }

    } 
}
