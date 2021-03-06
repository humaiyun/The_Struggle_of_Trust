﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;
    public D_Entity entityData;
    public int facingDirection { get; private set;}
    public Rigidbody2D rb {get; private set;}
    public Animator anim { get; private set; }
    public GameObject aliveGO { get; private set; }
    public AniimationToStateMachine atsm {get; private set;}
    public int lastDamageDirection {get; private set;}

    [SerializeField]
    private Transform wallCheck;
     [SerializeField]
    private Transform ledgeCheck;
     [SerializeField]
    private Transform ledgeCheckUp;
      [SerializeField]
    private Transform playerCheck;
      [SerializeField]
    private Transform groundCheck;
    
    private float currentStunResistance;
    private float lastDamageTime; 

    private float currentHealth;

    private Vector2 velocityWorkspace;
    protected bool isStunned;
    public bool PlayerDamaged;
    protected bool isDead;
    public Vector2 positionOfObject;
    
	float radius, moveSpeed;
    


    public virtual void Start()
    {
        facingDirection = 1;

        currentHealth = entityData.maxHealth;
        currentStunResistance = entityData.stunResistance;
        radius = 5f;
		moveSpeed = 5f;
        aliveGO = transform.Find("Slime").gameObject;
        
        rb = aliveGO.GetComponent<Rigidbody2D>();
        anim = aliveGO.GetComponent<Animator>();
        stateMachine = new FiniteStateMachine();
        atsm = aliveGO.GetComponent<AniimationToStateMachine>();
    }
    public virtual void Update()
    {

        stateMachine.currentState.LogicUpdate();
        positionOfObject = aliveGO.transform.localPosition;
        anim.SetFloat("yVelocity", rb.velocity.y);
        if (Time.time >= lastDamageTime + entityData.sunRecoveryTime)
        {
            ResetStunResistance();
        }
      
 //       ResetHurt();
    }
    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityWorkspace; //set the velocity of the enemy
    }
      public virtual void SetVelocityUp(float velocity)
    {
       // Debug.Log("Going thru here");
        velocityWorkspace.Set(rb.velocity.x, velocity * facingDirection);
        rb.velocity = velocityWorkspace; //set the velocity of the enemy
    }
     public virtual void SetVelocityDown(float velocity)
    {
        velocityWorkspace.Set( velocity, facingDirection * -rb.velocity.y);
        rb.velocity = velocityWorkspace; //set the velocity of the enemy
    }
 

    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x*velocity*direction,angle.y*velocity);
        rb.velocity = velocityWorkspace;
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position,aliveGO.transform.right,entityData.wallCheckDistance,entityData.whatIsGround);
    }

    public virtual bool CheckLedge()
    {
   return Physics2D.Raycast(ledgeCheck.position,Vector2.down,entityData.ledgeCheckDistance,entityData.whatIsGround);
    }
     public virtual bool CheckUp()
    {
   return Physics2D.Raycast(ledgeCheckUp.position,Vector2.up,entityData.ledgeCheckDistance,entityData.whatIsGround);
    }
    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position,entityData.groundCheckRadius,entityData.whatIsGround);
    }

    public virtual bool CheckPlayerInMinAgroRange() //Shorter Distance Check between Player and enemy
    {
        return Physics2D.Raycast(playerCheck.position,aliveGO.transform.right,entityData.minAgroDistance,entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerinMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position,aliveGO.transform.right,entityData.maxAgroDistance,entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position,aliveGO.transform.right,entityData.closeRangeActionDistance,entityData.whatIsPlayer);
    }

    public virtual void DamageHop(float velocity)
    {
        velocityWorkspace.Set(rb.velocity.x,velocity);
        rb.velocity = velocityWorkspace;
    }

    public virtual void ResetStunResistance()
    {
        isStunned = false;
        PlayerDamaged = false;
        currentStunResistance = entityData.stunResistance;
    }
    public virtual void ResetHurt()
    {
        
    PlayerDamaged = false;
        
       
    }
    public virtual void Damage(AttackDetails attackDetails)
    {
        lastDamageTime = Time.time;
        PlayerDamaged = true;
        currentStunResistance -=attackDetails.stunDamageAmount;
        currentHealth -= attackDetails.damageAmount;
        DamageHop(entityData.damageHopSpeed);
        Instantiate(entityData.hitParticle,aliveGO.transform.position,Quaternion.Euler(0f,0f,Random.Range(0f,360f)));

        if (attackDetails.position.x > aliveGO.transform.position.x)
        {
            lastDamageDirection = -1;
        }
        else 
        {
            lastDamageDirection = 1;
        }
        if (currentStunResistance <=0)
        {
            isStunned = true;
        }
        if (currentHealth <=0)
        {
            isDead = true;
        }
    }

    public virtual void Flip()
    {
        facingDirection *=-1;
        aliveGO.transform.Rotate(0f,180f,0f);
    }

      public virtual void Flip2()
    {
        facingDirection *=-1;
       // aliveGO.transform.Rotate(180f,0f,0f);
    }

      public virtual void Flip3()
    {
        facingDirection *=-1;
        aliveGO.transform.Rotate(180f,0f,0f);
    }

    public virtual void PlayHitSound()
    {
        if (entityData.enemy_id == 1)
        {
            FindObjectOfType<AudioManager>().Play("SlimeHit"); // 06 June 2020
        }
        else if (entityData.enemy_id == 2)
        {
            FindObjectOfType<AudioManager>().Play("GolemHit"); // 06 June 2020
        }
        else if (entityData.enemy_id == 3)
        {
             FindObjectOfType<AudioManager>().Play("ArcherHurt"); // 06 June 2020
        }
        else if (entityData.enemy_id == 4)
        {
            FindObjectOfType<AudioManager>().Play("BatHurt"); // 06 June 2020
        }
        else if (entityData.enemy_id == 5)
        {
             FindObjectOfType<AudioManager>().Play("Yeti_hitB"); // 06 June 2020
        }
        else if (entityData.enemy_id == 7)
        {
            FindObjectOfType<AudioManager>().Play("wolf_hurt"); // 06 June 2020
        }
        else if (entityData.enemy_id == 10)
        {
            FindObjectOfType<AudioManager>().Play("ogre_hurt"); // 06 June 2020
        }
        else if (entityData.enemy_id == 11)
        {
            FindObjectOfType<AudioManager>().Play("skeleton_hurt"); // 06 June 2020
        }
         else if (entityData.enemy_id == 12)
        {
       
            FindObjectOfType<AudioManager>().Play("shade_hurt"); // 06 June 2020
        }
        else if (entityData.enemy_id == 13)
        {
       
            FindObjectOfType<AudioManager>().Play("eyeball_hurt"); // 06 June 2020
        }
      
       

    }


    public virtual void PlayMoveSound()
    {
        
    }

    public virtual void StopMoveSound()
    {

         FindObjectOfType<AudioManager>().Pause("thomb_move"); // 06 June 2020
    }
   

    public virtual void PlayDeadSound()
    {
        if (entityData.enemy_id == 1)
        {
             FindObjectOfType<AudioManager>().Play("SlimeDead"); // 06 June 2020
        }
        else  if (entityData.enemy_id == 2)
        {
            FindObjectOfType<AudioManager>().Play("GolemDead"); // 06 June 2020
        }
        else if (entityData.enemy_id == 3)
        {
             FindObjectOfType<AudioManager>().Play("ArcherDead"); // 06 June 2020
        }
        else if (entityData.enemy_id == 4)
        {
            FindObjectOfType<AudioManager>().Play("BatDead"); // 06 June 2020
        }
        else if (entityData.enemy_id == 5)
        {
             FindObjectOfType<AudioManager>().Play("Yeti_Dead"); // 06 June 2020
        }
         else if (entityData.enemy_id == 7)
        {
            FindObjectOfType<AudioManager>().Play("wolf_dead"); // 06 June 2020
        }
          else if (entityData.enemy_id == 8)
        {
             FindObjectOfType<AudioManager>().Play("rat_death"); // 06 June 2020
        }
         else if (entityData.enemy_id == 10)
        {
            FindObjectOfType<AudioManager>().Play("ogre_dead"); // 06 June 2020
        }
        else if (entityData.enemy_id == 11)
        {
            FindObjectOfType<AudioManager>().Play("skeleton_dead"); // 06 June 2020
        }
          else if (entityData.enemy_id == 12)
        {
            FindObjectOfType<AudioManager>().Play("shade_dead"); // 06 June 2020
        }
        else if (entityData.enemy_id == 13)
        {
            FindObjectOfType<AudioManager>().Play("eyeball_dead"); // 06 June 2020
        }
      
      
    }

    public void PlayAttackSound()
    {
        if (entityData.enemy_id == 5)
        {
            
            FindObjectOfType<AudioManager>().Play("Yeti_Attack"); // 06 June 2020
        }
        else if (entityData.enemy_id == 3)
        {
            FindObjectOfType<AudioManager>().Play("sumari_Attack"); // 06 June 2020
        }
        else if (entityData.enemy_id == 2)
        {
             FindObjectOfType<AudioManager>().Play("Golem_Attack"); // 06 June 2020
        }
        else if (entityData.enemy_id == 7)
        {
            FindObjectOfType<AudioManager>().Play("wolf_attack"); // 06 June 2020
        }
        else if (entityData.enemy_id == 9)
        {
            FindObjectOfType<AudioManager>().Play("swing_scyth"); // 06 June 2020
        }
         else if (entityData.enemy_id == 10)
        {
            FindObjectOfType<AudioManager>().Play("ogre_attack"); // 06 June 2020
        }
        else if (entityData.enemy_id == 11)
        {
            FindObjectOfType<AudioManager>().Play("skeleton_attack"); // 06 June 2020
        }
      

    }

    public void PlayRangeAttackSound()
    {
        if (entityData.enemy_id == 3)
        {
            FindObjectOfType<AudioManager>().Play("Archer_Shoot"); // 06 June 2020
        }
        else if (entityData.enemy_id == 9)
        {
            FindObjectOfType<AudioManager>().Play("range_scyth"); // 06 June 2020
        }
          else if (entityData.enemy_id == 12)
        {
            FindObjectOfType<AudioManager>().Play("shade_attack"); // 06 June 2020
        }
      
    }




   public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheck.position,ledgeCheck.position + (Vector3)(Vector2.down *entityData.ledgeCheckDistance));
        Gizmos.DrawLine(ledgeCheckUp.position,ledgeCheckUp.position + (Vector3)(Vector2.up *entityData.ledgeCheckDistance));
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right*entityData.closeRangeActionDistance),0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right*entityData.minAgroDistance),0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right*entityData.maxAgroDistance),0.2f);
       
        
    }

}
