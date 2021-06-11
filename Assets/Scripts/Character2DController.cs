using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character2DController : MonoBehaviour
{
    [SerializeField] float MovementSpeed = 1;
    [SerializeField] float jumpForce = 1;
    private Rigidbody2D rigidbody;
    private bool isGrounded;
    private enum HeavyState {Light, Heavy};
    [SerializeField] private HeavyState heavyState = HeavyState.Heavy;

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
        
       
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
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

    private void GroundDetection()
    {

    }

}