using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stationaryEnemy : Enemy
{

    public override void Start()
    {
        base.Start();
        type = types.stationary;
        attackSkill = attackSkills.shooting;
        attackingDistance = chasingDistance;
    }


    

    public override void idle()
    {
        print("idle");
    }
    public override void patrol()
    {
        idle();
    }

    public override void chase()
    {
        attack();
    }

    public override void attack()
    {
        print("attack");
    }

    

}
