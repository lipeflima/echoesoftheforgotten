using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Battler
{
    public EnemyDefenseAction defenseAction;
    public Enemy(string name, int initiative, int health, int mana)
        : base(name, initiative, false, health, mana) { }

    public override void TakeAction()
    {
        Debug.Log($"{Name} está executando uma ação de IA...");
    }

    public override void Defend(Battler attacker)
    {
        Debug.Log($"{Name} está se defendendo contra {attacker.Name}.");
        // defenseAction.ExecuteDefense();
    }

    public override void ApplyDamage(int damage)
    {
        throw new System.NotImplementedException();
    }
}
