using System.Collections;
using System.Collections.Generic;
using UnityEngine;


struct corners
{
    public Vector2 bottomLeft, bottomRight, topLeft, topRight;
}


[RequireComponent(typeof(Rigidbody2D))]
public class controller : MonoBehaviour
{

    Rigidbody2D rb;

    public float movespeed;
    public float jumpHeight;
    public float timeToApex;
    float gravity;
    float jumpVelocity;
    public int extraJumps;
    int jumpsLeft;


    corners rayCastOrigins;
    BoxCollider2D col;
    public float skinWidth;
    public float rayLength;
    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        calculateInitial();
        col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        calculateInitial();
    }

    void calculateInitial()
    {
        gravity = 2 * jumpHeight / (timeToApex * timeToApex);
        jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
        rb.gravityScale = gravity;


    }
    public void move(Vector2 velocity)
    {
        verticalCollisions();
        velocity.x *= movespeed;
        velocity.y = rb.velocity.y;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump(ref velocity);
            //velocity.y = jumpVelocity;
        }

        rb.velocity = velocity;

    }


    void updateCorners()
    {
        Bounds bound = col.bounds;
        bound.Expand(-skinWidth);

        rayCastOrigins.bottomLeft = new Vector2(bound.min.x, bound.min.y);
        rayCastOrigins.bottomRight = new Vector2(bound.max.x, bound.min.y);
        rayCastOrigins.topLeft = new Vector2(bound.min.x, bound.max.y);
        rayCastOrigins.topRight = new Vector2(bound.max.x, bound.max.y);

    }

    void verticalCollisions()
    {
        updateCorners();


        Debug.DrawRay(rayCastOrigins.bottomLeft, Vector2.up * -1 * rayLength, Color.red);
        Debug.DrawRay(rayCastOrigins.bottomRight, Vector2.up * -1 * rayLength, Color.red);

        RaycastHit2D hit1 = Physics2D.Raycast(rayCastOrigins.bottomLeft, Vector2.up * -1, rayLength, groundLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(rayCastOrigins.bottomRight, Vector2.up * -1, rayLength, groundLayer);

        if (hit1.collider != null || hit2.collider != null)
        {
            jumpsLeft = extraJumps;
        }


    }


    void jump(ref Vector2 velocity)
    {
        if (jumpsLeft > 0)
        {
            velocity.y = jumpVelocity;
            jumpsLeft--;
        }
    }

}
