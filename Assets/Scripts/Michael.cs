using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float movement;
    [SerializeField] Rigidbody2D rigid;
    
    private bool isFacingRight = true;
    private bool jumpPressed = false;
    private bool isGrounded = false;
    private bool fireReady = true;
    private Scoring scorer;

    [SerializeField] float JUMP_FORCE = 350.0f;
    [SerializeField] int SPEED = 7;

    [SerializeField] private Transform mouth;
    [SerializeField] private GameObject facts;

    [SerializeField] public Animator animator;

    void Start()
    {
        if (rigid == null)
            rigid = GetComponent<Rigidbody2D>();

        scorer = GameObject.FindWithTag("Scorekeeper").GetComponent<Scoring>();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        movement = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;
        
    }

    void FixedUpdate()
    { 
        // Controls jumping.
        rigid.velocity = new Vector2(movement * SPEED, rigid.velocity.y);
        if (movement < 0 && isFacingRight || movement > 0 && !isFacingRight)
            Flip();
        if (jumpPressed && isGrounded)
            Jump();
        else jumpPressed = false;

        // Allows only one "fact" per lmb press.
        if (Input.GetButton("Fire1"))
            SpitFacts();

        // If player goes off screen, swap positions so it seems they're in a loop.
        if (rigid.position.x > 9.5f) rigid.position = new Vector2(-9.0f, rigid.position.y);
        else if (rigid.position.x < -9.5f) rigid.position = new Vector2(9.0f, rigid.position.y);
    }

    void Flip()
    {
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
    }

    void Jump()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, 0);
        rigid.AddForce(new Vector2(0, JUMP_FORCE));
        isGrounded = false;
        jumpPressed = false;
    }

    void SpitFacts()
    {
        
        if (!fireReady) return;

        fireReady = false;
        GameObject factSpat = Instantiate(facts, mouth.position, mouth.rotation);

        animator.SetBool("spat", true);

        if (isFacingRight)
            factSpat.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(5,10), 0);
        else
            factSpat.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-10,-5), 0);

        //Gives enough time for animation to complete and revert stages.
        Invoke("End",0.3f);
        Invoke("Ready",Random.Range(1,2));
    }

    private void End ()
    {
        animator.SetBool("spat", false);
    }

    private void Ready ()
    {
        fireReady = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

}
