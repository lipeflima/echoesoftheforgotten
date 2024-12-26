using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : Battler
{
    public PlayerActionManager actionManager;
    public PlayerCombat(string name, int initiative, int health, int mana)
        : base(name, initiative, true, health, mana) { }

    public override void TakeAction()
    {
        Debug.Log("Player está iniciando as ações!");

        actionManager = battlerGameobject.GetComponent<PlayerActionManager>();
        
        if (actionManager == null)
        {
            Debug.LogError("PlayerActionManager não encontrado!");
            return;
        }

        actionManager.StartAction();
    }

    public override void Defend(Battler attacker)
    {
        Debug.Log($"{Name} está se defendendo contra {attacker.Name}.");
    }

    public override void ApplyDamage(int damage)
    {
        Health -= damage;
    }
}
