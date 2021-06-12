using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character2DController : MonoBehaviour
{
    [SerializeField] float MovementSpeed = 1;
    [SerializeField] float jumpForce = 1;
    private Rigidbody2D rigidbody;
    [SerializeField] Vector2 holdSpot = new Vector2(-0.3, 1.5);
    private bool isGrounded;
    private enum HeavyState {Light, Heavy};
    [SerializeField] private HeavyState heavyState = HeavyState.Heavy;
    bool isTouchingInteractable = false;
    
    //Facing tracking
    private enum Facing {Left, Right};
    private Facing currentFacing = Facing.Right;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        var movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
        JumpInput();
        FacingTracking();
       
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        isTouchingInteractable = true;
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        isTouchingInteractable = false;
    }

    private void JumpInput()
    {
        switch(heavyState)
        {
            case HeavyState.Light:
                if(Input.GetButtonDown("Jump"))
                {
                    rigidbody.AddForce(new Vector2(0, jumpForce));
                    isGrounded = false;           
                }
                break;

            case HeavyState.Heavy:
                if(Input.GetButtonDown("Jump") && isGrounded == true)
                {
                    rigidbody.AddForce(new Vector2(0, jumpForce));
                    isGrounded = false;           
                }
                break;
        }
    }

    private void TotemInteraction()
    {
        
    }


    private void FacingTracking()
    {
        if(Input.GetKeyDown(KeyCode.A) && currentFacing != Facing.Left)
        {
            currentFacing = Facing.Left;
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }

        if(Input.GetKeyDown(KeyCode.D) && currentFacing != Facing.Right)
        {
            currentFacing = Facing.Right;
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    private void GroundDetection()
    {

    }

}