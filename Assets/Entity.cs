using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Entity : MonoBehaviour
{
    protected Animator _anim;
    protected Rigidbody2D _rb;
    protected Collider2D _col;
    protected SpriteRenderer _sr;
    
    [Header("Health")]
    [SerializeField] private int _maxHealth = 1;
    [SerializeField] private int _currentHealth;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private float damageFeedbackDuration= .1f;
    private Material _originalMaterial; 
    private Coroutine damageFeedbackCoroutine;
    public event Action<int, int> OnHealthChanged;

    [Header("Attack details")]
    [SerializeField] protected float attackRadius;
    [SerializeField] protected Transform attackPoint;
    [FormerlySerializedAs("whatisEnemy")] [SerializeField] protected LayerMask WhatIsTaget;
    
    [Header("Collision details")] 
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    
    //facing direction detail
    protected bool facingRight = true;
    protected int facingDirection=1;
    protected bool isGrounded=true;
    protected bool canMove = true;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        _col = GetComponent<Collider2D>();
        _sr = GetComponentInChildren<SpriteRenderer>();
        
        _currentHealth= _maxHealth;
        _originalMaterial = _sr.material; 
    }
    
    // Update is called once per frame
    protected virtual void Update()
    {
        HandelMovement();
        HandelAnimations();
        HandleFlip();
        HandleCollision();
    }

    public void DamageTagets()
    {
       Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, WhatIsTaget);
       foreach (Collider2D enemy in enemyColliders)
       {
           Entity entityTaget = enemy.GetComponent<Entity>();
           if (entityTaget != null)
           {
            entityTaget.TakeDamage();
           }
       }
    }

    private void TakeDamage()
    {
        _currentHealth -= 1;
        
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        
        PlayDamageFeedback();

        if (_currentHealth <= 0)
            Die();
    }
    protected virtual void Die()
    {
        _anim.enabled = false;
        _col.enabled = false;
        
        _rb.gravityScale = 12;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 15);
        
        Destroy(gameObject, 3);
    }

    private void PlayDamageFeedback()
    {
        if (damageFeedbackCoroutine != null)
            StopCoroutine(damageFeedbackCoroutine);    
        
        damageFeedbackCoroutine = StartCoroutine(DamageFeedbackCO());
    }
    
    private IEnumerator DamageFeedbackCO()
    {
        _sr.material = damageMaterial;
        
        yield return new WaitForSeconds(damageFeedbackDuration);
        
        _sr.material = _originalMaterial;   // ← SỬA dòng này, dùng biến cấp class thay vì local
    }

    // private IEnumerator DamageFeedbackCO()
    // {
    //     Material originalMaterial = _sr.material;
    //     _sr.material = damageMaterial;
    //     
    //     yield return new WaitForSeconds(damageFeedbackDuration);
    //     
    //     _sr.material = originalMaterial;
    // }
    
    public virtual void EnableMovement(bool enable)
    {
        canMove = enable;
    }

    protected void HandelAnimations()
    {
        _anim.SetFloat("xVelocity", _rb.linearVelocity.x); 
        _anim.SetBool("isGrounded", isGrounded); 
        _anim.SetFloat("yVelocity", _rb.linearVelocity.y);
    }

    protected virtual void HandleAttack()
    {
        if (isGrounded)
            _anim.SetTrigger("atk");
    }

    protected virtual void HandelMovement()
    {
    }
    

    protected virtual void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    protected virtual void HandleFlip()
    {
        if (_rb.linearVelocity.x >0 && facingRight==false)
            Flip();
        else if (_rb.linearVelocity.x <0 && facingRight==true)
            Flip();
        
    }
    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDirection = facingDirection * -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position,transform.position + new Vector3(0, - groundCheckDistance));

        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position,attackRadius);
    }
}