using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
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

}