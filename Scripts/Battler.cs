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
    public GameObject battlerGameobject { get; set; }

    public Battler(string name, int initiative, bool isPlayer, int health, int mana)
    {
        Name = name;
        Initiative = initiative;
        IsPlayer = isPlayer;
        Health = health;
        Mana = mana;

    }

    public abstract void TakeAction(ActionData actionData);
    public abstract void Defend(ActionData actionData);
    public abstract void ApplyDamage(int damage);
}

