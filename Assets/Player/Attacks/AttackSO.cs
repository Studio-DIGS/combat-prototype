using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Attacks/Normal Attack")]
public class AttackSO : ScriptableObject
{
    public Vector2 hitboxPosition;
    public Vector2 hitboxSize;
    public AnimatorOverrideController animatorOV;
    public float damage;
    public Vector2 knockback;
    [Range(0, 1)] public float freezeFrame;
}