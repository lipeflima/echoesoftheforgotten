using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  PlayerCombat : Battler
{
    public PlayerActionManager actionManager;
    private CardFeedbackManager cardFeedbackManager;
    private CharacterBar characterBar;
    private StatsUI statsUI;
    public PlayerCombat(string Name, int Initiative, bool IsPlayer, int Health, int Mana, int Attack, int Defense, int Dexterity, 
                    int Resistance, int Mentality, int Luck, float CriticalDamage, float CriticalChance, float ArmourPenetration, float Recovery, float Absorsion, float Accuracy)
        : base(Name, Initiative, true, Health, Mana, Attack, Defense, Dexterity, Resistance, Mentality, Luck, CriticalDamage, 
            CriticalChance, ArmourPenetration, Recovery, Absorsion, Accuracy) { }

    public void Start()
    {
        
    }

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
        Debug.Log($"Aplicando efeito na Stat: >>>>>>>>>>>>>>>>>>>>>>>>>> {statName} - {value}");
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
        Debug.Log($"[Player] levando dano! {damage}");
        Health -= damage;
        // FEEDBACK
        //cardFeedbackManager = CardFeedbackManager.instance;
        //cardFeedbackManager.SetCardBeforeInvoke("TakeDamage");
        //cardFeedbackManager.OnCardActivation?.Invoke();
        // CharacterBarUI
        characterBar = battlerGameobject.GetComponent<CharacterBar>();
        characterBar.UpdateUI(Health);
        // StatsUI
        statsUI = TurnManager.instance.statsUI;
        statsUI.CreateStatsUI(this);
    }

    public override int AvoidedDamage()
    {
        int avoidedDamage = (int)(Defense*ArmourPenetration);
        Debug.Log($"[Player] Dano evitado: {avoidedDamage}");
        return avoidedDamage;
    }

    public override void SetMana(int amount)
    {
        Mana = amount;
    }

    public override void ApplyEffect(CardEffectData effect)
    {
        // Aplica o buff imediatamente
        ModifyStat(effect.statName, effect.value);
        // StatsUI
        statsUI = TurnManager.instance.statsUI;
        statsUI.CreateStatsUI(this);
        // Adiciona o buff à lista de buffs ativos
        if (effect.effectType == Card.CardType.Buff || effect.effectType == Card.CardType.Debuff)
        {
            ActiveBuffs.Add(new ActiveBuff
            {
                buffName = effect.effectName,
                StatName = effect.statName,
                Value = effect.value,
                RemainingTurns = effect.duration // Duração em turnos
            });
        } 
        
    }
}
