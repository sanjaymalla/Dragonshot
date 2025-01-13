using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float jump;
    private Animator anim;
    private BoxCollider2D boxCollider;
    [SerializeField]private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private float wallJumpCooldown;
    private float horizontalInput;

    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //flipping player left and right while moving
        if (horizontalInput > 0.1f) 
        { 
            transform.localScale = Vector3.one;
        }

        else if (horizontalInput < -0.1f)
        {
            transform.localScale = new Vector3(-1,1,1);
        }

        
        //wall jump logic
        if(wallJumpCooldown > 0.2f)
        {
            //Jump movement
            
            rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y); //left right movement

            if (onWall() && !isGrounded()) 
            {
                rb.gravityScale = 0;
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                rb.gravityScale = 2;
            }
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }


        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded",isGrounded());

    }

    private void Jump()
    {
        if (isGrounded())
        {
            rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded()) 
        {
            if (horizontalInput==0)
            {
                rb.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            else
            {
                rb.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            }
            wallJumpCooldown = 0;
        }

    }


    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,boxCollider.bounds.size,0,Vector2.down,0.1f,groundLayer);
        return raycastHit.collider!= null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x,0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
