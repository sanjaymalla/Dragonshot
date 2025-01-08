using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float jump;
    private Animator anim;
    private bool grounded;

    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2 (horizontalInput*speed, rb.linearVelocity.y); //left right movement

        //flipping player left and right while moving
        if (horizontalInput > 0.1f) 
        { 
            transform.localScale = Vector3.one;
        }

        else if (horizontalInput < -0.1f)
        {
            transform.localScale = new Vector3(-1,1,1);
        }

        //Jump movement
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }


        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded",grounded);
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }
}
