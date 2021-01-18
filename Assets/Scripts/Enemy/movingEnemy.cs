using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingEnemy : Enemy
{

    public override void Start()
    {
        base.Start();
        type = types.moving;
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
        if(movingRight)
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
        patrol();
    }


    public override void attack()
    {
        idle();
    }

    IEnumerator stayIdle()
    {
        yield return new WaitForSeconds(1f);
        if(state == states.idle)
        {
            changeDirection();
            state = states.patrol;

        }
        
    }

    
   
}
