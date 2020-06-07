﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E5_moveState : MoveState
{
        private Enemy5 enemy;
     
     
    public E5_moveState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Enemy5 enemy) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

     public override void Enter()
    {
        base.Enter();
     
    }

   

    public override void Exit()
    {
        base.Exit();
    }

 
    public override void LogicUpdate()
    {
         base.LogicUpdate();
          enemy.CheckTouchDamage();
        enemy.PlayMovementSound();
      


    if (isDetectingWall || !isDetectingLedge)
        {
            enemy.idleState.SetFlipAfterIdle(true);
    
            stateMachine.ChangeState(enemy.idleState);
        }

      
        
    }

    


    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
