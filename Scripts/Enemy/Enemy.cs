using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class Enemy : Battler
{
    public EnemyDefenseAction defenseAction;
    public EnemyAttackAction attackAction;
    public Enemy(string name, int initiative, int health, int mana)
        : base(name, initiative, false, health, mana) { }

    public override void TakeAction(ActionData actionData)
    {
        attackAction = battlerGameobject.GetComponent<EnemyAttackAction>();
        Debug.Log($"{Name} está executando uma ação de IA...");
        attackAction.ExecuteAttack(actionData);
    }

    public override void Defend(ActionData actionData)
    {
        defenseAction = battlerGameobject.GetComponent<EnemyDefenseAction>();
        Debug.Log($"{Name} está se defendendo contra {actionData.Attacker.Name}");
        defenseAction.ExecuteDefense(actionData);
    }

    public override void ApplyDamage(int damage)
    {
        throw new System.NotImplementedException();
    }
}
