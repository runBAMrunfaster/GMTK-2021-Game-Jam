using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character2DController : MonoBehaviour
{
    [SerializeField] float MovementSpeed = 1;
    [SerializeField] float jumpForce = 1;
    private Rigidbody2D rigidbody;
    [SerializeField] float holdPointX = -0.3f;
    [SerializeField] float holdPointY = 1.5f;
    Vector2 holdSpot;
    private bool isGrounded;
    private enum HeavyState {Light, Heavy, Summoning};
    [SerializeField] private HeavyState heavyState = HeavyState.Heavy;
    private HeavyState storedHeavyState;
    bool isTouchingInteractable = false;
    GameObject touchedInteractable;
    bool isHoldingTotem = false;
    GameObject targetInteractable;
    GameObject heldTotem;
    Animator animator;

    //Ability Checks
    bool hasBlink = false;
    bool hasDoubleJump = false;
    bool hasSuperJump = false;
    bool hasDash = false;
    bool hasBaloonJump = false;

    int jumpCounter = 1;
   [SerializeField] int jumpCounterMax = 1;

    //Blink
    [SerializeField] GameObject blinkGhost;
    [SerializeField] float maxBlinkDist = 5f;
    [SerializeField] float ghostAimSpeed = 1f;
    
    //Facing tracking
    private enum Facing {Left, Right};
    private Facing currentFacing = Facing.Right;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        isGrounded = true;
        holdSpot = new Vector2(holdPointX, holdPointY);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var movement = Input.GetAxis("Horizontal");
        switch (heavyState)
        {
            
            case HeavyState.Light:
                transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
                JumpInput();
                FacingTracking();
                TotemInteraction();
                AnimationController();
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    storedHeavyState = heavyState;
                    heavyState = HeavyState.Summoning;
                }
                break;

            case HeavyState.Heavy:
                movement = Input.GetAxis("Horizontal");
                transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
                JumpInput();
                FacingTracking();
                TotemInteraction();
                AnimationController();
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    storedHeavyState = heavyState;
                    heavyState = HeavyState.Summoning;
                }
                break;

            case HeavyState.Summoning:
                gameObject.GetComponent<Rigidbody2D>().simulated = false;

                if(Input.GetKeyDown(KeyCode.LeftShift))
                {

                }

                if(Input.GetKeyUp(KeyCode.LeftShift))
                {
                    gameObject.GetComponent<Rigidbody2D>().simulated = true;
                    heavyState = storedHeavyState;
                }



                break;
        }
;
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            Blink();
        }
        

    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            jumpCounter = jumpCounterMax;
        }
    }

       void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        isTouchingInteractable = true;
        targetInteractable = other.gameObject;
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        isTouchingInteractable = false;
        targetInteractable = null;
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
                if(Input.GetButtonDown("Jump") && jumpCounter >=1)
                {
                    rigidbody.AddForce(new Vector2(0, jumpForce));
                    isGrounded = false;
                    jumpCounter -= 1;
                }
                break;
        }
    }

    private void TotemInteraction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            switch(heavyState)
            {
                case HeavyState.Light:
                if(isTouchingInteractable && targetInteractable.tag == "Totem")
                {
                    targetInteractable.transform.parent = this.gameObject.transform;
                    targetInteractable.transform.localPosition = holdSpot;
                    heldTotem = targetInteractable;
                    heavyState = HeavyState.Heavy;
                }
                break;

                case HeavyState.Heavy:
                heldTotem.transform.parent = null;
                heldTotem = null;
                heavyState = HeavyState.Light;
                break;

            }
        }
    }
    
    private void AnimationController()
    {
        if(Input.GetAxis("Horizontal") != 0)
        {
            animator.SetBool("isMoving", true);
        }

        else
        {
            animator.SetBool("isMoving", false);
        }

        if(Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger("Jump");
        }

        if(isGrounded)
        {
            animator.SetBool("isGrounded", true);
        }
        else 
        {
            animator.SetBool("isGrounded", false);
        }
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

    private void Blink()
    {

        transform.position = new Vector3(transform.position.x + (5 * transform.localScale.x), transform.position.y, transform.position.z);
       /* if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            GameObject tempGhost;
            tempGhost = Instantiate<GameObject>(blinkGhost, transform.position, transform.rotation);
            StartCoroutine("GhostTravel", tempGhost);
        }*/
        
    }
     //The key is a while statement, I know it!
    IEnumerator GhostTravel(GameObject ghost)
    {
        Vector2 ghostTargetPos = new Vector2((maxBlinkDist + ghost.transform.position.x) * transform.localScale.x , ghost.transform.position.y);
        ghost.transform.position = Vector3.Lerp(ghost.transform.position, ghostTargetPos, ghostAimSpeed);

        while(Input.GetKey(KeyCode.LeftShift))
        {
            yield return new WaitForSeconds(1);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            
            transform.position = ghost.transform.position;
            Destroy(ghost);
            //StopCoroutine("GhostTravel");
        }
        yield return null;
    }

}