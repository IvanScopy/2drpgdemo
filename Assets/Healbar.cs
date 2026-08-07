using System;
using UnityEngine;
using UnityEngine.UI;

public class Healbar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Entity entity;
    [SerializeField] private Vector3 offset= new Vector3(0, 1.2f, 0);
    
    private Transform target;

    private void Awake()
    {
        target = entity.transform;
        entity.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDestroy()
    {
        entity.OnHealthChanged -= UpdateHealthBar;
    }

    private void LateUpdate()
    {
        transform.position = target.position + offset;
        transform.rotation = Quaternion.identity;
    }

    private void UpdateHealthBar(int curHealth, int maxHealth)
    {
        float percent = (float)curHealth / maxHealth;
        fillImage.fillAmount = percent;
    }
}
