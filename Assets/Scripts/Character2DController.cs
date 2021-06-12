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
    [SerializeField]  bool hasBlink = false;
    bool hasDoubleJump = false;
    [SerializeField] bool hasSuperJump = false;
    bool hasDash = false;
    bool hasBaloonJump = false;

    int jumpCounter = 1;
   [SerializeField] int jumpCounterMax = 1;

    //Blink
    [SerializeField] GameObject blinkGhost;
    [SerializeField] float maxBlinkDist = 5f;
    [SerializeField] float ghostAimSpeed = 1f;
    [SerializeField] Collider2D groundCollider;
    [SerializeField] BlinkChecker blinkChecker;
    [SerializeField] int blinkCountMax = 1;
    int blinkCount;

    int jumpStage = 1;
    
    //Facing tracking
    private enum Facing {Left, Right};
    private Facing currentFacing = Facing.Right;

    //System
    bool isPaused = false;

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

        if(!isPaused)
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
                    break;

                case HeavyState.Heavy:
                    movement = Input.GetAxis("Horizontal");
                    transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
                    JumpInput();
                    FacingTracking();
                    TotemInteraction();
                    AnimationController();
                    break;

                case HeavyState.Summoning:
                    gameObject.GetComponent<Rigidbody2D>().simulated = false;
                    JumpInput();
                    break;
            }
;
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Blink();
            }
        }
        else
        {

        }
        
        

    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            jumpCounter = jumpCounterMax;
            blinkCount = blinkCountMax;
        }
    }

    public void SetIsPaused(bool pauseState)
    {
        isPaused = pauseState;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpStage = 0;
            StartCoroutine("BigJumpCounter");
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            switch (hasSuperJump)
            {
                case false:
                    if(jumpCounter >= 1)
                    {
                        rigidbody.AddForce(new Vector2(0, jumpForce));
                    }
                    
                    break;

                case true:
                    if (jumpStage <= 1 && jumpCounter >=1)
                    {
                        rigidbody.AddForce(new Vector2(0, jumpForce));
                    }
                    else if (jumpStage > 1 && jumpCounter >= 1)
                    {
                        rigidbody.AddForce(new Vector2(0, jumpForce * (1 + jumpStage) * 0.75f));
                    }

                    break;
            }
        }
    }

    IEnumerator BigJumpCounter()
    {


        Debug.Log("Starting Jump Counter");
        for( int i = 1; i < 3; i++)
        {
            if(Input.GetKeyUp(KeyCode.Space))
            {
                Debug.Log("Key released! Sending Jump Stage " + jumpStage);
                yield return jumpStage;
                break;
            }
            else
            {
                Debug.Log("Key Held. Jump Stage is now " + jumpStage);
                jumpStage = i;
                yield return jumpStage;
                yield return new WaitForSeconds(1f);
            }
            
        }

        yield return jumpStage;
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
        if(hasBlink)
        {
            bool canBlink = blinkChecker.GetCanBlink();
            if (!canBlink)
            {
                Debug.Log("Oh no! Touching the thing!");
            }

            else if (canBlink && blinkCount >= 1)
            {
                Debug.Log("We aren't touching anything!");
                transform.position = new Vector3(transform.position.x + (maxBlinkDist * transform.localScale.x), transform.position.y, transform.position.z);
                blinkCount -= 1;
            }
        }
        

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