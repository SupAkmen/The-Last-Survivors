using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="EnemyScriptableObject",menuName ="NS/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    [SerializeField]
    float moveSpeed;
    public float MoveSpeed { get => moveSpeed;private  set => moveSpeed = value; }
    [SerializeField]
    float maxHeath;
    public float MaxHeath { get => maxHeath; private set => maxHeath = value; }
    [SerializeField]
    float damage;
    public float Damage { get => damage; private set => damage = value; }
}
