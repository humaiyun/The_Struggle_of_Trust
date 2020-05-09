﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


public class PlayerCombatController : MonoBehaviour {

    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private float inputTimer, attack1Radius, attack1Damage;
     [SerializeField]
     private float stunDamageAmount =1f;
    [SerializeField]
    private Transform attack1HitBoxPos;
    [SerializeField]
    private LayerMask whatIsDamageable;

    private bool gotInput;
    private bool isAttacking;
    private bool isFirstAttack;
    private float canGetHit;

    private float lastInputTime = Mathf.NegativeInfinity; // Storing the last time player attempted to attack and will be ready to attack
    private AttackDetails attackDetails;

    private Animator anim;
    private PlayerController PC;

    private void Start() {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);
        PC = GetComponent<PlayerController>();
    }

    private void Update() {
        CheckCombatInput();
        CheckAttacks();
    }

    private void CheckCombatInput() { //Checks for any combat related input from the player
    

        //if (Input.GetKeyDown(KeyCode.V)) { // "V" is for attack
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Z)) { // True if LMB/Mouse 1 is pressed
            if (combatEnabled) {
               // Debug.Log("MB1 was pressed.");
                //Attempt to Combat
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
        else if (Input.GetKeyDown("S"))
        {
             Debug.Log("MB1 was pressed.");
        }
    }

    private void CheckAttacks() { // Makes attack happen when there is an input
        if (gotInput) {
            // Perform Attack 1
            if (!isAttacking) {
                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack;
                anim.SetBool("attack1", true);
                anim.SetBool("firstAttack", isFirstAttack);
                anim.SetBool("isAttacking", isAttacking);

                // play random animation (07 may 2020)
                int index = UnityEngine.Random.Range(1, 4); // random number 
                anim.Play("Attack" + index);
                Invoke("ResetAttack", .15f);
            }
        }

        if (Time.time >= lastInputTime + inputTimer) {
            // Wait for new input
            gotInput = false;
        }
    }

    //07 may 2020 (for rng attack anim)
    private void ResetAttack() {
        isAttacking = false;
    }

    private void CheckAttackHitbox() { // Detect damagable objects in a range
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamageable); // Detect all objects in a circle

        attackDetails.damageAmount = attack1Damage;
        attackDetails.position = transform.position;
        attackDetails.stunDamageAmount = stunDamageAmount;



        foreach (Collider2D collider in detectedObjects) {
            collider.transform.parent.SendMessage("Damage", attackDetails); // Used to call function on scripts on objects without knowing which script it is

        }
    }

    private void FinishAttack1() { // Called at end of attack animation, to let script know it's done
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
    }

    private void Damage(AttackDetails attackDetails)
    {
        int direction;
        //Damage our player
        //PS.DecreaseHealth(attackDetails.damageAmount); // TODO: 
        canGetHit = FindObjectOfType<PlayerController>().DamageOrNot();
        UnityEngine.Debug.Log(canGetHit);
        if (canGetHit <= 10)
        {
            FindObjectOfType<PlayerHealth>().EndGame();
        }
        // 


        if (attackDetails.position.x < transform.position.x)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
        PC.knockBack(direction);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }
}
