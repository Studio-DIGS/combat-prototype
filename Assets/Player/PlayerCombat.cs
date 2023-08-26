using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public List<AttackSO> combo;
    private float lastClickedTime;
    private float lastComboEnd;
    private int comboCounter;

    [SerializeField] private float comboCooldown = 0.3f;
    [SerializeField] private float attackTime = 0.1f;
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
        if (comboCounter <= combo.Count && Time.time - lastClickedTime > attackTime)
        {
            // play animation with currently set attack animation
            anim.runtimeAnimatorController = combo[comboCounter].animatorOV;
            anim.Play("Attack", 0, 0);

            // Handle damage and knockback here

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

    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
    }
}
