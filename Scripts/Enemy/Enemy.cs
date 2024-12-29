using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class Enemy : Battler
{
    public EnemyDefenseAction defenseAction;
    public EnemyAttackAction attackAction;
    private CardFeedbackManager cardFeedbackManager;
    private StatsUI statsUI;
    [SerializeField] private CharacterBar characterBar;
    public Enemy(string Name, int Initiative, bool IsPlayer, int Health, int Mana, int Attack, int Defense, int Dexterity, 
                    int Resistance, int Mentality, int Luck, float CriticalDamage, float CriticalChance, float ArmourPenetration, float Recovery, float Absorsion, float Accuracy)
        : base(Name, Initiative, false, Health, Mana, Attack, Defense, Dexterity, Resistance, Mentality, Luck, CriticalDamage, 
            CriticalChance, ArmourPenetration, Recovery, Absorsion, Accuracy) { }

    public void Start()
    {
        
    }
    
    public override void TakeAction(ActionData actionData)
    {
        attackAction = battlerGameobject.GetComponent<EnemyAttackAction>();
        attackAction.ExecuteAttack(actionData);
    }

    public override void Defend(ActionData actionData)
    {
        defenseAction = battlerGameobject.GetComponent<EnemyDefenseAction>();
        defenseAction.ExecuteDefense(actionData);
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

    public override void ApplyDamage(int damage)
    {
        statsUI = TurnManager.instance.statsUI;
        Health -= damage;
        Debug.Log($"characterBar {characterBar}");
        Debug.Log($"Health {Health}");
        characterBar = battlerGameobject.GetComponent<CharacterBar>();
        characterBar.UpdateUI(Health);
        statsUI.CreateStatsUI(this);
        cardFeedbackManager = CardFeedbackManager.instance;
        cardFeedbackManager.SetCardBeforeInvoke("TakeDamage");
        cardFeedbackManager.OnCardActivation?.Invoke();
    }

    public override void SetMana(int amount)
    {
        Mana+=amount;
    }

    public override void ApplyEffect(CardEffectData effect)
    {
        // Aplica o buff imediatamente
        ModifyStat(effect.statName, effect.value);

        // Adiciona o buff à lista de buffs ativos
        if (effect.effectType == Card.CardType.Buff || effect.effectType == Card.CardType.Debuff)
        {
            ActiveBuffs.Add(new ActiveBuff
            {
                StatName = effect.statName,
                Value = effect.value,
                RemainingTurns = effect.duration // Duração em turnos
            });
        } 
        
    }
}
