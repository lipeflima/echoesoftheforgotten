using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ActionManager;

/// <summary>
/// Classe que gerencia a resolução do turno entre atacante e defensor.
/// </summary>
public class TurnResolver : MonoBehaviour
{
    private static System.Random random = new System.Random();
    public void ResolveTurn(ActionData actionData)
    {
        Debug.Log("Resolvendo o Turno");   
        // 1. Aplica buffs e debuffs
        ApplyBuffsAndDebuffs(actionData.CombatAction.AttackerAction.CardEffects, actionData.Defender);
        
        // 2. Processa o ataque
        int damage = ProcessAttack(actionData);
        int avoidedDamage = ProcessDefense(actionData);

        Debug.Log($"Turn damage: {damage}");
        Debug.Log($"Turn avoided Damage: {avoidedDamage}");

        bool hasCounterAttack = IsCounterAttackSuccessful(actionData.Defender.Dexterity, actionData.Attacker.Dexterity) && actionData.CombatAction.DefenderAction.DefenseType == DefenseType.CounterAttack;

        // 3. Aplica o resultado ao defensor
        // 4. Aplica efeitos de contra-ataque, se houver
        if (!hasCounterAttack) {
            Debug.Log($"Defender Health Before: {actionData.Defender.Health}");
            actionData.Defender.ApplyDamage(damage - avoidedDamage);
            Debug.Log($"Defender Health After: {actionData.Defender.Health}");
        } else {
            Debug.Log($"Attacker Health Before: {actionData.Attacker.Health}");
            actionData.Attacker.ApplyDamage(avoidedDamage);
            Debug.Log($"Attacker Health After: {actionData.Attacker.Health}");
        }
    }

    private int ProcessAttack(ActionData actionData)
    {
        switch (actionData.CombatAction.AttackerAction.AttackType)
        {
            case AttackType.Basic:
                return Math.Max(0, (int)(actionData.Attacker.Attack * actionData.Attacker.Accuracy));
            case AttackType.CardAttack:
                return CalculateSpecialAttackDamage(actionData.CombatAction.AttackerAction.CardEffects.FindAll(effect => effect.statName == "Health" && effect.effectType == Card.CardType.Attack));
            case AttackType.FakeAttack:
                return 0; // Fake attack does no damage
            default:
                break;
        }
        return 0;
    }

    private int ProcessDefense(ActionData actionData)
    {
        switch (actionData.CombatAction.DefenderAction.DefenseType)
        {
            case DefenseType.Basic:
                return Math.Max(0, (int)(actionData.Defender.Defense * actionData.Defender.ArmourPenetration));
            case DefenseType.CardDefense:
                return CalculateSpecialDefense(actionData.CombatAction.DefenderAction.CardEffects.FindAll(effect => effect.statName == "Health" && effect.effectType == Card.CardType.Attack));
            case DefenseType.CounterAttack:
                return CalculatedCounterAttackDamage(actionData.Defender.Dexterity, actionData.Attacker.Dexterity, actionData.Attacker.Attack);
            case DefenseType.Evade:
                return 0; // Fake attack does no damage
            default:
                break;
        }
        return 0;
    }

    private void ApplyBuffsAndDebuffs(List<CardEffectData> effects, Battler target)
    {
        foreach(var effect in effects)
        {
            switch (effect.effectType)
            {
                case Card.CardType.Buff:
                    ApplyBuff(effect, target);
                    break;
                case Card.CardType.Debuff:
                    ApplyBuff(effect, target);
                    break;
            }
        }
    }

    public void ApplyBuff(CardEffectData effect, Battler target)
    {
        Debug.Log($"StatName {effect.statName}");
        Debug.Log($"value {effect.value}");
        Debug.Log($"Battler {target.Name}");
        // Aplica o buff imediatamente
        target.ModifyStat(effect.statName, effect.value);

        // Adiciona o buff à lista de buffs ativos
        target.ActiveBuffs.Add(new ActiveBuff
        {
            StatName = effect.statName,
            Value = effect.value,
            RemainingTurns = effect.duration // Duração em turnos
        });
    }

    public static bool IsCounterAttackSuccessful(int defenderDexterity, int attackerDexterity)
    {
        // Base difficulty threshold
        int baseThreshold = 10;

        // Calculate the threshold adjusted by the difference in dexterity
        int adjustedThreshold = baseThreshold + (attackerDexterity - defenderDexterity);

        // Roll a d20 and add defender's dexterity
        int roll = random.Next(1, 21);
        int defenderResult = roll + defenderDexterity;

        // Check if the defender's result meets or exceeds the adjusted threshold
        return defenderResult >= adjustedThreshold;
    }

    public static int CalculatedCounterAttackDamage(int defenderDexterity, int attackerDexterity, int avoidedDamage)
    {
        // Base ratio for counter-attack damage
        float baseRatio = 0.2f; // 20% do dano evitado como dano base

        // Adjust ratio based on dexterity difference
        float dexterityFactor = Mathf.Clamp((defenderDexterity - attackerDexterity) * 0.05f, -0.2f, 0.2f);
        float finalRatio = baseRatio + dexterityFactor;

        // Ensure the ratio is within reasonable bounds
        finalRatio = Mathf.Clamp(finalRatio, 0.1f, 0.5f); // Entre 10% e 50% do dano evitado

        // Calculate the counter-attack damage
        int counterAttackDamage = Mathf.RoundToInt(avoidedDamage * finalRatio);

        // Ensure a minimum damage of 1
        return Mathf.Max(counterAttackDamage, 1);
    }

    private int CalculateSpecialAttackDamage(List<CardEffectData> effects)
    {
        var damageSome = 0;
        foreach(var effect in effects)
        {
            damageSome+=effect.value;
        }
        return damageSome;
    }

    private int CalculateSpecialDefense(List<CardEffectData> effects)
    {
        var defenseSome = 0;
        foreach(var effect in effects)
        {
            defenseSome+=effect.value;
        }
        return defenseSome;
    }
}
