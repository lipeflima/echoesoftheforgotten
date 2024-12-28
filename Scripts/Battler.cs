using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Battler
{
    public string Name { get; set; }
    public int Initiative { get; set; }
    public bool IsPlayer { get; set; }
    public int Health { get; set; }
    public int Mana { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Dexterity { get; set; }
    public int Resistance { get; set; }
    public int Mentality { get; set; }
    public int Luck { get; set; }
    public float CriticalDamage { get; set; }
    public float CriticalChance { get; set; }
    public float ArmourPenetration { get; set; }
    public float Accuracy { get; set; }
    public float Recovery { get; set; }
    public float Absorsion { get; set; }
    public bool HasActiveBuff { get; set; }
    public bool HasActiveDebuff { get; set; }
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
        battlerGameobject.GetComponent<CharacterBar>().UpdateUI(Health);
    }

    // Método para aplicar alterações diretas
    public abstract void ModifyStat(string statName, float value);
    public abstract void SetMana(int amount);
}

