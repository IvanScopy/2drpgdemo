using UnityEngine;

public class Player : Entity
{
    private float _xInput;
    [SerializeField] private float jumpForce= 8;
    [Header("Movement details")]
    [SerializeField] protected float moveSpeed= 3.5f;
    private bool canJumb = true;

    protected override void Update()
    {
        base.Update();
        HandleInput();
    }

    private void HandleInput()
    {
        _xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
            TryToJump();
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
            HandleAttack();
    }
    protected override void HandelMovement()
    {
        if (canMove )
            _rb.linearVelocity = new Vector2(_xInput*moveSpeed, _rb.linearVelocity.y);
        else
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
    }
    
    private void TryToJump()
    {
        if (isGrounded && canJumb)
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
    }

    public override void EnableMovement(bool enable)
    {
        base.EnableMovement(enable);
        canJumb = enable;
    }

    protected override void Die()
    {
        base.Die();
        UI.instance.EnableGameOverUI();
    }
}
