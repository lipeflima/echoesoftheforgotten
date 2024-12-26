using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Battler
{
    public PlayerActionManager actionManager;
    public Player(string name, int initiative, int health, int mana)
        : base(name, initiative, true, health, mana) { }

    public override void TakeAction(ActionData actionData)
    {
        Debug.Log("Player está iniciando as ações!");

        actionManager = battlerGameobject.GetComponent<PlayerActionManager>();
        
        if (actionManager == null)
        {
            Debug.LogError("PlayerActionManager não encontrado!");
            return;
        }

        actionManager.StartAction(actionData);
    }

    public override void Defend(ActionData actionData)
    {
        Debug.Log($"{Name} está se defendendo contra {actionData.Attacker.Name}.");

        actionManager = battlerGameobject.GetComponent<PlayerActionManager>();
        
        if (actionManager == null)
        {
            Debug.LogError("PlayerActionManager não encontrado!");
            return;
        }

        actionManager.StartAction(actionData);

    }

    public override void ApplyDamage(int damage)
    {
        Health -= damage;
    }
}
