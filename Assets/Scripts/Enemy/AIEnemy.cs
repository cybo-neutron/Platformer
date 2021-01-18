using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemy : Enemy
{
    public override void Start()
    {
        base.Start();
        type = types.AI;
    }
    public override void idle()
    {

        velocity = Vector2.zero;
        Move(ref velocity);
        StartCoroutine(stayIdle());

    }

    public override void patrol()
    {

        edgeCheck();
        if (movingRight)
        {
            velocity = Vector2.right * enemyProps.speed;
        }
        else
        {
            velocity = Vector2.left * enemyProps.speed;
        }

        Move(ref velocity);




    }
    public override void chase()
    {
        edgeCheck();

        if(enemyProps.collisionInfo.below)
        {
            if (transform.position.x < whoisPlayer.transform.position.x)
            {
                if (!movingRight)
                    changeDirection();
                velocity = Vector2.right * enemyProps.speed;

            }
            else
            {
                if (movingRight)
                    changeDirection();
                velocity = Vector2.left * enemyProps.speed;
            }
        }
        else
        {
            print("Stop here");
            velocity = Vector2.zero;
        }

        Move(ref velocity);



    }


    public override void attack()
    {



        velocity = Vector2.zero;
        Move(ref velocity);
    }

    IEnumerator stayIdle()
    {
        yield return new WaitForSeconds(1f);
        if (state == states.idle)
        {
            changeDirection();
            state = states.patrol;

        }

    }




}
