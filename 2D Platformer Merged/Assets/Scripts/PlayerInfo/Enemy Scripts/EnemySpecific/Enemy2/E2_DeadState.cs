﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_DeadState : DeadState
{

   private Enemy2 enemy;
    public E2_DeadState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData, Enemy2 enemy) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
         int count = Random.Range(3, 10); //100% Chance
  int random = Random.Range(1,20);
        
        for (int i = 0; i < count; ++i)
        {
            if (random >10)
            {
                if (random%2 == 0)
                 GameObject.Instantiate(stateData.Green_coin, entity.aliveGO.transform.position, Quaternion.identity);
                if (random%2 !=0)
                GameObject.Instantiate(stateData.Blue_coin, entity.aliveGO.transform.position, Quaternion.identity);
                if (random%5 == 0)
                GameObject.Instantiate(stateData.Yellow_coin, entity.aliveGO.transform.position, Quaternion.identity);
                if (random%7 == 0)
                GameObject.Instantiate(stateData.Yellow_coin, entity.aliveGO.transform.position, Quaternion.identity);
            }
            if (random <= 10)
            {
                GameObject.Instantiate(stateData.Hearts, entity.aliveGO.transform.position, Quaternion.identity);
                break;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
