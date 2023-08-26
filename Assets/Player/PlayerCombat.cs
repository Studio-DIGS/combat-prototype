using System;
using System.Collections;
using System.Collections.Generic;
using Obscura;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public bool isAttacking;
    public List<AttackSO> combo;
    private float lastClickedTime;
    private int comboCounter;

    [SerializeField] private PositionFollowCameraController camera;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackTime = 0.1f;
    [SerializeField] private LayerMask enemyLayer;
    [Range(0, 1)] [SerializeField] private float animationTimeMax = 0.9f;
    [SerializeField] Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AttackTimeout();
        
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (comboCounter < combo.Count && Time.time - lastClickedTime > attackTime)
        {
            isAttacking = true;
            // play animation with currently set attack animation
            anim.runtimeAnimatorController = combo[comboCounter].animatorOV;
            anim.Play("Attack", 0, 0);

            // Handle damage and knockback here
            Vector2 knockback = new Vector2(combo[comboCounter].knockback.x * transform.localScale.x,
                combo[comboCounter].knockback.y);
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyController>().Hit(knockback);
            }
            
            if (hitEnemies.Length > 0)
            {
                // Screen shake and freeze
                if (comboCounter == combo.Count - 1)
                {
                    var halfTime = anim.GetCurrentAnimatorStateInfo(0).length / 2;
                    camera.Invoke("ShakeScreen", halfTime);
                    camera.Invoke("FreezeScreen", halfTime);
                }
                else
                {
                    camera.ShakeScreen();
                }
            }

            lastClickedTime = Time.time;
            comboCounter++;
        }
    }

    void AttackTimeout()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > animationTimeMax &&
            anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            EndCombo();
        }
    }

    public void EndCombo()
    {
        isAttacking = false;
        comboCounter = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
