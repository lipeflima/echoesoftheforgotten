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
    public void ResolveTurn(ActionData attackerData, ActionData defenderData)
    {
        // 1. Processa o ataque
        int damage = ProcessAttack(attackerData, defenderData);

        // 2. Aplica buffs e debuffs
        ApplyBuffsAndDebuffs(attackerData.CombatAction.AttackerAction.CardEffects, attackerData.CombatAction.AttackerAction.Target);

        // 3. Aplica o resultado ao defensor
        defenderData.Defender.ApplyDamage(damage);

        // 4. Aplica efeitos de contra-ataque, se houver
        if (defenderData.CombatAction.DefenderAction.DefenseStrategy == DefenseStrategy.CounterAttack && IsCounterAttackSuccessful(defenderData.Defender.Dexterity, attackerData.Attacker.Dexterity))
        {
            attackerData.Defender.ApplyDamage(defenderData.CombatAction.CalculatedCounterDamage);
        }
    }

    private int ProcessAttack(ActionData attackerData, ActionData defenderData)
    {
        switch (attackerData.CombatAction.AttackerAction.AttackStrategy)
        {
            case AttackStrategy.Basic:
                return Math.Max(0, attackerData.CombatAction.CalculatedAttack - defenderData.CombatAction.CalculatedDefense);
            case AttackStrategy.CardAttack:
                return CalculateSpecialAttackDamage(attackerData.CombatAction.AttackerAction.CardEffects.FindAll(effect => effect.statName == "Health"));
            case AttackStrategy.Fake:
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
                    // ApplyBuff(effect, target);
                    break;
                case Card.CardType.Debuff:
                    // ApplyDebuff(effect, target);
                    break;
            }
        }
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

    private int CalculateSpecialAttackDamage(List<CardEffectData> effects)
    {
        var damageSome = 0;
        foreach(var effect in effects)
        {
            damageSome+=effect.value;
        }
        return damageSome;
    }
}
