using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ActionManager;

public class ActionData : MonoBehaviour
{
    
    public Battler Defender { get; set; }
    public Battler Attacker { get; set; }
    public int EnergyCost { get; set; }
    public CombatAction CombatAction { get; set; }
    // public Dictionary<string, object> CustomData { get; set; } = new Dictionary<string, object>();
    public CardData CardData { get; set; }
}

public class CardData
{
    public List<Card> AttackerSelectedCards { get; set; }
    public List<Card> DefenderSelectedCards { get; set; }
    public List<Card> DeckCards { get; set; }
}

public class CombatAction
{
    public AttackerAction AttackerAction;
    public DefenderAction DefenderAction;
    public int CalculatedAttack { get; set; }
    public int CalculatedDefense { get; set; }
    public int CalculatedCounterDamage { get; set; }
    public Dictionary<string, object> CustomCombatData { get; set; } = new Dictionary<string, object>();
}

public class AttackerAction
{
    public AttackStrategy AttackStrategy { get; set; }
    public Battler Target { get; set; }
    public List<CardEffectData> CardEffects;
    public int Attack { get; set; }
    public int Evasion { get; set; }
    public int Dexterity { get; set; }
    public int ManaCost { get; set; }

}

public class DefenderAction
{
    public DefenseStrategy Defense { get; set; }
    public List<CardEffectData> CardEffects;
    public int DefenderDefense { get; set; }
    public int Dexterity { get; set; }
    public int ManaCost {get; set; }

}

