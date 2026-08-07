using System;
using UnityEngine;

public class Entity_AnimationEvent : MonoBehaviour
{
    private Entity _entity;

    private void Awake()
    {
        _entity = GetComponentInParent<Entity>();
    }

    private void DisableJumpAndMove()
    {
        _entity.EnableMovement(false);
    }

    private void EnableJumpAndMove()
    {
        _entity.EnableMovement(true);
    }

    public void DamageTarget() => _entity.DamageTagets();
    
}
