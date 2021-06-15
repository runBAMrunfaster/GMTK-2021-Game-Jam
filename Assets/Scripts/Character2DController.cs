using System;
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

    [SerializeField] GroundChecker groundChecker;
    private bool isGrounded;
    int jumpStage = 1;
    
    //Facing tracking
    private enum Facing {Left, Right};
    private Facing currentFacing = Facing.Right;
    [SerializeField] GameObject textBox;

    //System
    bool isPaused = false;
    bool isAnimPaused = false;
    AudioSource audioSource;


    //Sound Effects
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip land;
    [SerializeField] AudioClip superJump;
    [SerializeField] AudioClip charge;
    [SerializeField] AudioClip blink;
    [SerializeField] AudioClip blinkFail;
    [SerializeField] AudioClip itemGet;
    [SerializeField] AudioClip damage;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        isGrounded = true;
        holdSpot = new Vector2(holdPointX, holdPointY);
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAnimPaused)
        {
            AnimationController();
        }
        if(!isPaused)
        {
            GroundCheck();
            Movement();
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Blink();
            }
        }
        else
        {

        }
        
        

    }

    private void Movement()
    {
        var movement = Input.GetAxis("Horizontal");
        switch (heavyState)
        {

            case HeavyState.Light:
                transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
                JumpInput();
                FacingTracking();
                TotemInteraction();
                PortalInteraction();
                break;

            case HeavyState.Heavy:
                movement = Input.GetAxis("Horizontal");
                transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
                JumpInput();
                FacingTracking();
                TotemInteraction();
                PortalInteraction();
                break;

            case HeavyState.Summoning:
                gameObject.GetComponent<Rigidbody2D>().simulated = false;
                JumpInput();
                break;
        }
    }

    private void GroundCheck()
    {
        isGrounded = groundChecker.GetIsGrounded();
    }

    private void JumpReset()
    {
        isGrounded = true;
        jumpCounter = jumpCounterMax;
        blinkCount = blinkCountMax;
        animator.SetInteger("ChargeCount", 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(isGrounded)
        {
            audioSource.PlayOneShot(land);
        }
    }

    public void SetIsPaused(bool pauseState)
    {
        isPaused = pauseState;
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
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpStage = 0;
            StartCoroutine("BigJumpCounter");
            
        }*/

        if(Input.GetKeyUp(KeyCode.Space))
        {
            switch (hasSuperJump)
            {
                case false:
                    if(jumpCounter >= 1)
                    {
                        rigidbody.AddForce(new Vector2(0, jumpForce));
                        audioSource.PlayOneShot(jump);

                        if(heavyState == HeavyState.Heavy)
                        {
                            jumpCounter -= 1;
                        }
                        
                    }
                    
                    break;

                case true:
                    if (jumpStage <= 1 && jumpCounter >= 1)
                    {
                        rigidbody.AddForce(new Vector2(0, jumpForce));
                        audioSource.PlayOneShot(jump);
                        if (heavyState == HeavyState.Heavy)
                        {
                            jumpCounter -= 1;
                        }
                    }
                    else if (jumpStage > 1 && jumpCounter >= 1 && isGrounded)
                    {
                        rigidbody.AddForce(new Vector2(0, jumpForce * (1 + jumpStage) * 0.75f));
                        audioSource.Stop();
                        audioSource.PlayOneShot(superJump);
                        if (heavyState == HeavyState.Heavy)
                        {
                            jumpCounter -= 1;
                        }
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
                        int helmetID = targetInteractable.GetComponent<PickupFirstHelmet>().GetHelmetID();
                        targetInteractable.GetComponent<PickupFirstHelmet>().PlayCutscene(helmetID);


                        targetInteractable.transform.parent = this.gameObject.transform;
                        targetInteractable.transform.localPosition = holdSpot;
                        heldTotem = targetInteractable;
                         heavyState = HeavyState.Heavy;
                    }
                    break;

                case HeavyState.Heavy:
                    if(targetInteractable.tag == "Portal")
                    {

                    }

                    else if(targetInteractable.tag == "Altar")
                    {
                        
                        heavyState = HeavyState.Light;
                        targetInteractable.GetComponent<AltarInterract>().AcceptHelmet(heldTotem);
                        targetInteractable.GetComponent<AltarInterract>().PlayHelmCutscene(heldTotem);
                        //Activate generalized method in an Altar script here to play a relavant cutscene.

                        heldTotem.transform.parent = null;
                        heldTotem = null;
                    }
                    break;

            }
        }
    }

    private void PortalInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isTouchingInteractable && targetInteractable.tag == "Portal")
            {
                transform.position = targetInteractable.GetComponent<TeleportInteract>().GetPortalDest().transform.position;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            switch (hasSuperJump)
            {
                case false:
                    if (jumpCounter >= 1)
                    {
                        animator.SetTrigger("Jump");
                    }

                    break;

                case true:

                    if (jumpCounter >= 1 && isGrounded && animator.GetInteger("ChargeCount") == 0)
                    {
                        
                        Debug.Log("Animation things should be happening right now!");
                        animator.SetInteger("ChargeCount", 1);
                        StartCoroutine("JumpChargeTimer");
                    }

                    else if (jumpStage <= 1 && jumpCounter >= 1 && Input.GetKeyUp(KeyCode.Space))
                    {
                        animator.SetTrigger("Jump");
                        
                    }


                    break;
            }
        }

       
        /*
        else if (hasSuperJump)
        {
            if (isGrounded && Input.GetButtonUp("Jump"))
            {
                animator.SetTrigger("Jump");
                audioSource.pitch = 1;
                audioSource.Stop();
                audioSource.PlayOneShot(superJump);
                animator.SetInteger("ChargeCount", 0);
            }

            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                animator.SetInteger("ChargeCount", 1);
                //audioSource.loop = true;
                audioSource.clip = charge;
                audioSource.Play();
                StartCoroutine("JumpChargeTimer");
            }

        }*/

        if(isGrounded)
        {
            animator.SetBool("isGrounded", true);
        }
        else 
        {
            animator.SetBool("isGrounded", false);
        }
    }

    private IEnumerator JumpChargeTimer()
    {
        int timer = 0;

        while(timer <= 3)
        {
            timer++;
            if (!Input.GetKey(KeyCode.Space))
            {
                audioSource.pitch = 1;
                animator.SetTrigger("Jump");
                animator.SetInteger("ChargeCount", 0);
                
                break;
            }
            if (timer >= 2)
            {
                
                Debug.Log("Setting Charge Counter to 2!");
                animator.SetInteger("ChargeCount", 2);


            }

            yield return new WaitForSeconds(1);
        }

        yield return null;
    }

    private void FacingTracking()
    {
        if(Input.GetKeyDown(KeyCode.A) && currentFacing != Facing.Left)
        {
            currentFacing = Facing.Left;
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            textBox.transform.localScale = new Vector3(-0.25f, 0.25f, 0.25f);
        }

        if(Input.GetKeyDown(KeyCode.D) && currentFacing != Facing.Right)
        {
            currentFacing = Facing.Right;
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
            textBox.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
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
                audioSource.PlayOneShot(blinkFail);
                Debug.Log("Oh no! Touching the thing!");
            }

            else if (canBlink && blinkCount >= 1)
            {
                audioSource.PlayOneShot(blink);
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
        Vector2 ghostTargetPos = new Vector2((maxBlinkDist + ghost.transform.position.x) * transform.localScale.x, ghost.transform.position.y);
        ghost.transform.position = Vector3.Lerp(ghost.transform.position, ghostTargetPos, ghostAimSpeed);

        while (Input.GetKey(KeyCode.LeftShift))
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

    public int GetHeldHelmetID()
    {
        return heldTotem.GetComponent<PickupFirstHelmet>().GetHelmetID();
    }

    public void SetJumpCounterMax(int newMax)
    {
        jumpCounterMax = newMax;
    }

    public void SetBlink(bool canBlink)
    {
        hasBlink = canBlink;
    }

    public void SetSuperJump(bool superJump)
    {
        hasSuperJump = superJump;
    }
}