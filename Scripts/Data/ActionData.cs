using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ActionManager;

public class ActionData
{
    public Battler Defender;
    public Battler Attacker;
    public Battler PlayerStats;
    public List<Battler> EnemiesStats = new();
    public CurrentTurnAction CurrentTurnAction { get; set; }
    public int PlayerTurnSpentEnergy { get; set; } = 0;
    public int EnemyTurnSpentEnergy { get; set; } = 0;
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
    public AttackType AttackType { get; set; }
    public List<CardEffectData> CardEffects { get; set; } = new List<CardEffectData>();
    public int EnergyCost { get; set; }
}

public class DefenderAction
{
    public DefenseType DefenseType { get; set; }
    public List<CardEffectData> CardEffects { get; set; } = new List<CardEffectData>();
    public int EnergyCost { get; set; }
}

