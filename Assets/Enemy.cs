using System;
using UnityEngine;

public class Enemy : Entity
{
    private bool playerDetected;
    private bool wasPlayerDetected;
    
    [Header("Movement details")]
    [SerializeField] protected float moveSpeed= 2f;
    [Space]
    [Header("Combat details")]
    [SerializeField] private float attckCooldown = 1.5f;
    private float lastAttackTime = -999f;
    protected override void Update()
    {
        base.Update();
        HandleAttack();
    }

    protected override void HandleAttack()
    {
        if (playerDetected && Time.time >= lastAttackTime + attckCooldown)
        {
            _anim.SetTrigger("atk");
            lastAttackTime = Time.time;
        }
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();
        playerDetected = Physics2D.OverlapCircle(attackPoint.position, attackRadius, WhatIsTaget);
    }

    protected override void HandelMovement()
    {
        if (canMove && !playerDetected)
            _rb.linearVelocity = new Vector2(facingDirection*moveSpeed, _rb.linearVelocity.y);
        else
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
    }

    protected override void Die()
    {
        base.Die();
        UI.instance.AddKillCount();
    }
}
