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
        ApplyBuffsAndDebuffs(attackerData, defenderData);

        // 3. Aplica o resultado ao defensor
        defenderData.defender.ApplyDamage(damage);

        // 4. Aplica efeitos de contra-ataque, se houver
        if (defenderData.defense == DefenseStrategy.CounterAttack && IsCounterAttackSuccessful(defenderData.defender.Dexterity, attackerData.attacker.Dexterity))
        {
            attackerData.defender.ApplyDamage(defenderData.calculatedCounterDamage);
        }
    }

    private int ProcessAttack(ActionData attackerData, ActionData defenderData)
    {
        switch (attackerData.attack)
        {
            case AttackStrategy.Basic:
                return Math.Max(0, attackerData.calculatedAttack - defenderData.calculatedDefense);
            case AttackStrategy.CardAttack:
                return CalculateSpecialAttackDamage(attackerData);
            case AttackStrategy.Fake:
                return 0; // Fake attack does no damage
        }
        return 0;
    }

    private void ApplyBuffsAndDebuffs(ActionData attackerData, ActionData defenderData)
    {
        // Processa lógica de buffs/debuffs
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

    private int CalculateSpecialAttackDamage(ActionData attackerData)
    {
        // Cálculo personalizado para ataques especiais
        return attackerData.cardDamage;
    }
}
