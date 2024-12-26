using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ActionManager;

public class ActionData
{
    public Battler Defender { get; set; }
    public Battler Attacker { get; set; }
    public int EnergyPool { get; set; }
    public CurrentTurnAction CurrentTurnAction { get; set; }
    public CombatAction CombatAction { get; set; }
    // public Dictionary<string, object> CustomData = new Dictionary<string, object>();
    public CardData CardData { get; set; }

    public ActionData()
    {
        CardData = new CardData(); 
        CombatAction = new CombatAction();
    }
}

public class CardData
{
    public List<Card> AttackerSelectedCards { get; set; } = new List<Card>(); 
    public List<Card> DefenderSelectedCards { get; set; } = new List<Card>();
    public List<Card> DeckCards { get; set; } = new List<Card>();
}

public class CombatAction
{
    public AttackerAction AttackerAction { get; set; }
    public DefenderAction DefenderAction { get; set; }
    public int CalculatedAttack { get; set; }
    public int CalculatedDefense { get; set; }
    public int CalculatedCounterDamage { get; set; }
    public Dictionary<string, object> CustomCombatData { get; set; } = new Dictionary<string, object>();

    public CombatAction()
    {
        AttackerAction = new AttackerAction();
        DefenderAction = new DefenderAction();
    }
}

public class AttackerAction
{
    public AttackStrategy AttackStrategy { get; set; } = new AttackStrategy();
    public Battler Target { get; set; }
    public List<CardEffectData> CardEffects { get; set; } = new List<CardEffectData>();
    public int Attack { get; set; }
    public int Evasion { get; set; }
    public int Dexterity { get; set; }
    public int ManaCost { get; set; }
}

public class DefenderAction
{
    public DefenseStrategy DefenseStrategy { get; set; } = new DefenseStrategy();
    public List<CardEffectData> CardEffects { get; set; } = new List<CardEffectData>();
    public int Defense { get; set; }
    public int Dexterity { get; set; }
    public int ManaCost { get; set; }
}

