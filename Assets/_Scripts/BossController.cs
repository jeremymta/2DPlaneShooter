using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public float attackCooldown = 2f;
    private float lastAttackTime;

    void Start()
    {
        currentHealth = maxHealth;
        lastAttackTime = Time.time - attackCooldown;
    }

    void Update()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }
    }

    void PerformAttack()
    {
        // Implement special attack logic here
        Debug.Log("Boss is attacking!");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Implement death logic here
        Debug.Log("Boss defeated!");
        Destroy(gameObject);
    }
}
