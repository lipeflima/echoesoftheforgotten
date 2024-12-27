using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Battler
{
    public string Name { get; protected set; }
    public int Initiative { get; protected set; }
    public bool IsPlayer { get; protected set; }
    public int Health { get; protected set; }
    public int Mana { get; protected set; }
    public int Attack { get; protected set; }
    public int Defense { get; protected set; }
    public int Dexterity { get; protected set; }
    public int Resistance { get; protected set; }
    public int Mentality { get; protected set; }
    public int Luck { get; protected set; }
    public float CriticalDamage { get; protected set; }
    public float CriticalChance { get; protected set; }
    public float ArmourPenetration { get; protected set; }
    public float Accuracy { get; protected set; }
    public float Recovery { get; protected set; }
    public float Absorsion { get; protected set; }
    public bool HasActiveBuff { get; protected set; }
    public bool HasActiveDebuff { get; protected set; }
    public GameObject battlerGameobject { get; set; }
    public List<ActiveBuff> ActiveBuffs { get; private set; } = new List<ActiveBuff>();

    public Battler(string name, int initiative, bool isPlayer, int health, int mana, int attack, int defense, int dexterity, 
                    int resistance, int mentality, int luck, float criticalDamage, float criticalChance, float armourPenetration, float recovery, float absorsion, float accuracy)
    {
        Name = name;
        Initiative = initiative;
        IsPlayer = isPlayer;
        Health = health;
        Mana = mana;
        Attack = attack;
        Defense = defense;
        Dexterity = dexterity;
        Resistance = resistance;
        Mentality = mentality;
        Luck = luck;
        CriticalDamage = criticalDamage;
        CriticalChance = criticalChance;
        ArmourPenetration = armourPenetration;
        Recovery = recovery;
        Absorsion = absorsion;
        Accuracy = accuracy;
    }

    public abstract void TakeAction(ActionData actionData);
    public abstract void Defend(ActionData actionData);
    
    public void ApplyDamage(int damage)
    {
        Health -= damage;
    }

    // Método para aplicar alterações diretas
    public abstract void ModifyStat(string statName, float value);
}

