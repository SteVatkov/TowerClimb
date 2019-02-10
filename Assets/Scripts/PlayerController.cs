using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    [SerializeField] float runSpeed = 5.0f;
    [SerializeField] [Range(1, 20)] float jumpSpeed = 5f;
    [SerializeField] [Range(1, 40)] float highJumpSpeed = 10f;
    [SerializeField] [Range(1, 10)] float fallMultiplier = 2f;
    [SerializeField] [Range(1, 10)] float lowJumpMultiplier = 2f;
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);
    [SerializeField] Vector2 enemyKick = new Vector2(25f, 25f);
    [SerializeField] GameObject dust;

    private SpriteRenderer mySpriteRenderer;
    private Rigidbody2D myRigidBody;
    private CircleCollider2D myFeet;
    private BoxCollider2D myBodyCollider;
    private Animator animator;
    private bool isFacingRight = true;
    private bool trigger = true;
    private GameObject runDust;

    public bool isAlive = true;

    // Use this for initialization
    void Start () {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myRigidBody = GetComponent<Rigidbody2D>();
        myFeet = GetComponent<CircleCollider2D>();
        myBodyCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!isAlive) { return; }
        Jump();
        //FlipSprite();
        Attack();
        Die();
    }

    private void FixedUpdate()
    {
        if (!isAlive) { return; }
        Run();
        Fall();
    }

    void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool isRunning = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        animator.SetBool("Running", isRunning); // Running is true if any velocity is detected
        animator.SetFloat("AnimMultiplier", Mathf.Abs(controlThrow)); // GetAxis controls the speed of the animation

        if (isRunning && myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            FlipSprite();
            Dust();
        }
        if (!isRunning)
        {
            trigger = true;
        }
    }

    void Jump()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (CrossPlatformInputManager.GetButtonDown("Jump")){
            animator.SetBool("Jumping", true);
            if (CrossPlatformInputManager.GetAxis("Vertical") > 0.5f)
            {
                Vector2 jumpVelocityToAdd = new Vector2(0f, highJumpSpeed);
                myRigidBody.velocity += jumpVelocityToAdd;
                //myRigidBody.velocity = Vector2.up * jumpSpeed;
            }

            else
            {
                Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
                myRigidBody.velocity += jumpVelocityToAdd;
                //myRigidBody.velocity = Vector2.up * jumpSpeed;
            }
        }
    }

    void Fall()
    {
        if (myRigidBody.velocity.y < 0)
        {
            myRigidBody.velocity += Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (myRigidBody.velocity.y > 0 && !CrossPlatformInputManager.GetButton("Jump"))
        {
            myRigidBody.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        else if (myRigidBody.velocity.y == 0 && myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) { animator.SetBool("Jumping", false); }
    }

    private void Attack()
    {
        if (CrossPlatformInputManager.GetButtonDown("Attack"))
        {
            animator.SetTrigger("Attacking");
        }
    }

    private void FlipSprite()
    {
            bool facingRight = Mathf.Sign(myRigidBody.velocity.x) > 0;
            if (facingRight && isFacingRight)
            {
                transform.localRotation = Quaternion.Euler(0, 0f, 0f);
                Dust();

                isFacingRight = !isFacingRight;
            }
            else if (!facingRight && !isFacingRight)
            {
                transform.localRotation = Quaternion.Euler(0, 180f, 0f);

                Dust();

                isFacingRight = !isFacingRight;
            }
        

            /*
            if (playerHasHorizontalSpeed)
            {
                //transform.localRotation = Quaternion.Euler(0,);
                transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
            }

            else if (!playerHasHorizontalSpeed)
            {
                Vector2 pos = new Vector2(transform.position.x, transform.position.y);

                Instantiate(dust, pos, Quaternion.identity);
            }
            */
        }

    private void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            animator.SetTrigger("Dying");
            myRigidBody.velocity = deathKick;
            //FindObjectOfType<GameSession>().ProcessPlayerDeath();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Dust()
    {
        if (!trigger) { return; }
        
            Vector2 pos = new Vector2(transform.position.x - (0.25f * Mathf.Sign(myRigidBody.velocity.x)),transform.position.y);
            runDust = (GameObject)Instantiate(dust, pos, transform.rotation);
            
            Destroy(runDust, 1f);
            trigger = !trigger;
        
    }
}
