using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  PlayerCombat : Battler
{
    public PlayerActionManager actionManager;
    public PlayerCombat(string Name, int Initiative, bool IsPlayer, int Health, int Mana, int Attack, int Defense, int Dexterity, 
                    int Resistance, int Mentality, int Luck, float CriticalDamage, float CriticalChance, float ArmourPenetration, float Recovery, float Absorsion, float Accuracy)
        : base(Name, Initiative, true, Health, Mana, Attack, Defense, Dexterity, Resistance, Mentality, Luck, CriticalDamage, 
            CriticalChance, ArmourPenetration, Recovery, Absorsion, Accuracy) { }

    public override void TakeAction(ActionData actionData)
    {

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
        actionManager = battlerGameobject.GetComponent<PlayerActionManager>();
        
        if (actionManager == null)
        {
            Debug.LogError("PlayerActionManager não encontrado!");
            return;
        }

        actionManager.StartAction(actionData);

    }

    public override void ModifyStat(string statName, float value)
    {
        switch (statName)
        {
            case "Health": Health += (int)value; break;
            case "Mana": Mana += (int)value; break;
            case "Attack": Attack += (int)value; break;
            case "Defense": Defense += (int)value; break;
            case "Dexterity": Dexterity += (int)value; break;
            case "CriticalDamage": CriticalDamage += value; break;
            case "CriticalChance": CriticalChance += value; break;
            case "ArmourPenetration": ArmourPenetration += value; break;
            case "Accuracy": Accuracy += value; break;
        }
    }

    public override void SetMana(int amount)
    {
        Mana = amount;
    }
}
