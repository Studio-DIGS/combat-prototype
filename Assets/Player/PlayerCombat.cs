using System;
using System.Collections;
using System.Collections.Generic;
using Obscura;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class PlayerCombat : MonoBehaviour
{
    [HideInInspector] public bool isAttacking;
    public bool holdingSword;
    [SerializeField] private List<AttackSO> unamredCombo;
    [SerializeField] private List<AttackSO> armedCombo;
    private List<AttackSO> combo;
    private float lastClickedTime;
    private int comboCounter;

    [SerializeField] private GameObject hitEffect;
    [SerializeField] private PositionFollowCameraController camera;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackTime = 0.1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask swordLayer;
    [SerializeField] private float pickupRange;
    [Range(0, 1)] [SerializeField] private float animationTimeMax = 0.9f;
    [SerializeField] Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        holdingSword = false;
        combo = unamredCombo;
    }

    // Update is called once per frame
    void Update()
    {
        AttackTimeout();
        
        if (Input.GetButtonDown("Fire1"))
        {
            combo = (holdingSword) ? armedCombo : unamredCombo; // choose which combo depending on weapon equipped
            Attack();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            PickupSword();
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

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            if (hitEnemies.Length > 0)
            {
                StartCoroutine(HandleHit(hitEnemies));
            }
            
            lastClickedTime = Time.time;
            comboCounter++;
        }
    }

    IEnumerator HandleHit(Collider2D[] hitEnemies)
    {
        // Handle damage and knockback here
        Vector2 knockback = new Vector2(combo[comboCounter].knockback.x * transform.localScale.x,
            combo[comboCounter].knockback.y);

        if (comboCounter == combo.Count - 1)
        {
            var waitTime = anim.GetCurrentAnimatorStateInfo(0).length * combo[comboCounter].freezeFrame;
            yield return new WaitForSeconds(waitTime);
            camera.FreezeScreen();
        }
        camera.ShakeScreen();

        foreach (Collider2D enemy in hitEnemies)
        {
            Instantiate(hitEffect, attackPoint);
            enemy.GetComponent<EnemyController>().Hit(knockback);
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
        Gizmos.color = Color.grey;
        Gizmos.DrawWireCube(transform.position, new Vector3(pickupRange, pickupRange));
    }

    public void PickupSword()
    {
        Collider2D sword = Physics2D.OverlapCircle(transform.position, pickupRange, swordLayer);
        if (sword)
        {
            Debug.Log("pickup");
            Destroy(sword.gameObject);
            holdingSword = true;
        }
    }
}
