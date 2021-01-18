using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum types
{
    stationary,moving,AI
}

public enum attackSkills
{
    melle,shooting
}

public enum states
{
    idle,patrol,chase,attack
}



public abstract class Enemy : MonoBehaviour
{
    public types type;
    public attackSkills attackSkill;
    public states state;

    #region State Related
    [Header("State Related")]
    public GameObject whoisPlayer;
    public LayerMask collisionLayer;

    float distanceFromPlayer;
    public float chasingDistance;
    public float attackingDistance;
    public float rayCastLength;


    #endregion

    #region Properties
    [Header("Enemy Properties")]
    public enemyProps enemyProps;
    [HideInInspector]public Rigidbody2D rb2d;
    BoxCollider2D boxCollider;
    public LayerMask groundLayer;
    [HideInInspector]public bool movingRight;
    public bool isIdle;
    float horizontalRaySpacing;
    public Vector2 velocity;
    #endregion


    public Transform[] Waypoints;

    public virtual void Start()
    {
        if(whoisPlayer==null)
        {
            whoisPlayer = GameObject.FindGameObjectWithTag("Player");
    
        }
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        movingRight = true;
        calculateRaySpacing();

        enemyProps.speed = Mathf.Clamp(enemyProps.speed, 1, int.MaxValue);
        enemyProps.health = Mathf.Clamp(enemyProps.health, 1, int.MaxValue);
        enemyProps.points = Mathf.Clamp(enemyProps.points, 1, int.MaxValue);
        enemyProps.fireRate = Mathf.Clamp(enemyProps.fireRate, 0.1f, int.MaxValue);
        enemyProps.bulletSpeed = Mathf.Clamp(enemyProps.bulletSpeed, 0.5f, int.MaxValue);
        enemyProps.skinWidth = Mathf.Clamp(enemyProps.skinWidth, 0.01f, int.MaxValue);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            state = states.idle;

        if(type == types.stationary)
        {
            //horizontal rayCast
            Debug.DrawRay(transform.position, transform.right*rayCastLength, Color.blue);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, rayCastLength, collisionLayer);

            if(hit && hit.collider.CompareTag("Player"))
            {
                
                if(hit.distance >attackingDistance)
                {
                    state = states.idle;
                }
                else
                {
                    state = states.attack;
                }
            }
            else 
            {
                state = states.idle;
            }


        }
        else if(type == types.AI)
        {
            //rayCast to the player

            Vector2 distance = whoisPlayer.transform.position - transform.position;

            Debug.DrawRay(transform.position, distance.normalized*rayCastLength, Color.green);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, distance.normalized, rayCastLength,collisionLayer);
            //edgeCheck();

            if (hit && hit.collider.CompareTag("Player"))
            {
                print("player Detected");

                if (hit.distance <= attackingDistance)
                {
                    state = states.attack;
                }
                else if (hit.distance > attackingDistance && hit.distance <= chasingDistance)
                {
                    state = states.chase;
                }
                else
                {
                    //cycle b/w idle and patrol
                    edgeCheck();

                }

            }
            else
            {
                //cycle b/w idle and patrol
                edgeCheck();
            }


        }
        else
        {

            //moving Enemy switch b/w idle and patrol

            edgeCheck();

            
        }


        switch(state)
        {
            case states.idle:
                idle();
                break;
            case states.patrol:
                patrol();
                break;
            case states.chase:
                chase();
                break;
            case states.attack:
                attack();
                break;
            default:
                patrol();
                break;
        }
        
    }


    public void updateRayCastOrigins()
    {
        
        Bounds bound = boxCollider.bounds;
        bound.Expand(-enemyProps.skinWidth * 2);

        enemyProps.rayCastOrigins.bottomLeft = new Vector2(bound.min.x, bound.min.y);
        enemyProps.rayCastOrigins.bottomRight = new Vector2(bound.max.x, bound.min.y);
        enemyProps.rayCastOrigins.topLeft = new Vector2(bound.min.x, bound.max.y);
        enemyProps.rayCastOrigins.topRight = new Vector2(bound.max.x, bound.max.y);

    }

    void calculateRaySpacing()
    {
        Bounds bound = boxCollider.bounds;
        bound.Expand(-enemyProps.skinWidth * 2);

        enemyProps.horizontalRayCount = Mathf.Clamp(enemyProps.horizontalRayCount, 4, int.MaxValue);
        horizontalRaySpacing = bound.size.y / (enemyProps.horizontalRayCount - 1);

    }

    public void edgeCheck()
    {
        enemyProps.collisionInfo.Reset();
        updateRayCastOrigins();
        HorizontalCollisionRayCast();
        VerticalCollisionRayCast();
        if (!enemyProps.collisionInfo.below && state == states.patrol)
        {
            state = states.idle;
        }
        

        if ((enemyProps.collisionInfo.left || enemyProps.collisionInfo.right)  && state==states.patrol)
        {
            state = states.idle;
        }
    }

    public void VerticalCollisionRayCast()
    {
        Vector2 rayOrigin = movingRight ? enemyProps.rayCastOrigins.bottomRight : enemyProps.rayCastOrigins.bottomLeft;

        Debug.DrawRay(rayOrigin, Vector2.down * enemyProps.skinWidth * 3, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, enemyProps.skinWidth * 3, groundLayer);

        if(hit)
        {
            enemyProps.collisionInfo.below = true;
        }

    }

    public void HorizontalCollisionRayCast()
    {
        Vector2 rayOrigin = movingRight ? enemyProps.rayCastOrigins.bottomRight : enemyProps.rayCastOrigins.bottomLeft;
        

        int direction = rb2d.velocity.x!=0? (int)Mathf.Sign(rb2d.velocity.x):0;

        
        for(int i=0;i<enemyProps.horizontalRayCount;i++)
        {
            Vector2 rayPoint = rayOrigin + Vector2.up * (i * horizontalRaySpacing);
            Debug.DrawRay(rayPoint, Vector2.right * direction * enemyProps.skinWidth * 3, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(rayPoint, Vector2.right * direction, enemyProps.skinWidth * 3, groundLayer);

            if(hit)
            {
                state = states.idle;
                enemyProps.collisionInfo.left = direction == -1;
                enemyProps.collisionInfo.right = direction == 1;

            }
        }

    }



    public void changeDirection()
    {
        
        if(movingRight)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            //transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            //transform.rotation = Quaternion.Euler(0, 0, 0);

        }

        movingRight = !movingRight;
    }

    public void Move(ref Vector2 velocity)
    {
        edgeCheck();

        rb2d.velocity = velocity;
    }


    public abstract void patrol();
    public abstract void idle();
    public abstract void chase();
    public abstract void attack();

}
