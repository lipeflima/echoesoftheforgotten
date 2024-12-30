using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static ActionManager;

/// <summary>
/// Classe que gerencia a resolução do turno entre atacante e defensor.
/// </summary>
public class TurnResolver : MonoBehaviour
{
    private CardFeedbackManager cardFeedbackManager;
    [SerializeField] private GeneralUI generalUI;
    private static System.Random random = new System.Random();

    public void Start()
    {
        cardFeedbackManager = CardFeedbackManager.instance;
    }

    public void ResolveTurn(ActionData actionData)
    {  
        // 1. Aplica mana
        ApplyManaEnergy(actionData);

        // 2. Verifica ações evasivas e de contrataque
        bool hasCounterAttack = IsCounterAttackSuccessful(actionData.Defender.Dexterity, actionData.Attacker.Dexterity) && actionData.CombatAction.DefenderAction.DefenseType == DefenseType.CounterAttack;
        bool hasEvaded = IsEvadeSuccessful(actionData.Defender.Dexterity, actionData.Defender.Accuracy, actionData.Attacker.Dexterity) && actionData.CombatAction.DefenderAction.DefenseType == DefenseType.Evade;
        bool hasFaked = actionData.CombatAction.AttackerAction.AttackType == AttackType.FakeAttack;
        bool hasCardAttacked = actionData.CombatAction.AttackerAction.AttackType == AttackType.CardAttack;
        bool hasCardDefended = actionData.CombatAction.DefenderAction.DefenseType == DefenseType.CardDefense;

        if (hasFaked) return;

        // 3. Aplica os efeitos das cartas
        if (hasCardAttacked) ApplyAttackCardEffects(actionData);
        if (hasCardDefended) ApplyDefenseCardEffects(actionData);

        if (hasEvaded)
        {
            // realizar algum feedback de evasão
            Debug.Log("O defensor evadiu o ataque!");
            return;
        }
        
        // 4. Processa o ataque e defesa
        int damage = ProcessAttack(actionData);

        Debug.Log($"Turn damage: {damage}");

        // 4. Aplica o resultado ao defensor ou Aplica efeitos de contra-ataque no atacante, se houver.
        if (hasCounterAttack) {
            Debug.Log("O defensor contra-atacou!");
            actionData.Attacker.ApplyDamage(CalculatedCounterAttackDamage(actionData.Defender.Dexterity, actionData.Attacker.Dexterity, actionData.Attacker.Attack));
        } else {
            actionData.Defender.ApplyDamage(damage);
        }

        actionData.CardData.AttackerSelectedCards.Clear();
        actionData.CardData.DefenderSelectedCards.Clear();
    }

    private void ApplyAttackCardEffects(ActionData actionData)
    {
        if (actionData.CardData.AttackerSelectedCards.Count  > 0)
        {
            foreach(Card attackCard in actionData.CardData.AttackerSelectedCards)
            {
                CardBehavior behavior = CardResolver.Resolve(attackCard);
                switch (attackCard.cardType)
                {
                    case Card.CardType.Buff:
                        behavior.ExecuteAction(attackCard, actionData.Attacker);
                        break;
                    case Card.CardType.Debuff:
                        behavior.ExecuteAction(attackCard, actionData.Defender);
                        break;
                    case Card.CardType.Attack:
                        behavior.ExecuteAction(attackCard, actionData.Defender);
                        break;
                    default:
                        Debug.LogError("Tipo da carta não existe e não pode executar efeito!");
                        break;
                }
            }
        }
    }
    
    private void ApplyDefenseCardEffects(ActionData actionData)
    {
        if (actionData.CardData.DefenderSelectedCards.Count > 0)
        {
            foreach(Card defenseCard in actionData.CardData.DefenderSelectedCards)
            {
                CardBehavior behavior = CardResolver.Resolve(defenseCard);
                switch (defenseCard.cardType)
                {
                    case Card.CardType.Buff:
                        behavior.ExecuteAction(defenseCard, actionData.Defender);
                        break;
                    case Card.CardType.Debuff:
                        behavior.ExecuteAction(defenseCard, actionData.Attacker);
                        break;
                    case Card.CardType.Defense:
                        behavior.ExecuteAction(defenseCard, actionData.Defender);
                        break;
                    default:
                        Debug.LogError("Tipo da carta não existe e não pode executar efeito!");
                        break;
                }
            }
        }
    }

    private void ApplyManaEnergy(ActionData actionData)
    {
        actionData.Attacker.Mana = actionData.Attacker.IsPlayer ? actionData.Attacker.Mana - actionData.PlayerTurnSpentEnergy : actionData.Attacker.Mana - actionData.EnemyTurnSpentEnergy;
        actionData.Defender.Mana = actionData.Defender.IsPlayer ? actionData.Defender.Mana - actionData.PlayerTurnSpentEnergy : actionData.Defender.Mana - actionData.EnemyTurnSpentEnergy;
        generalUI.SetPlayerCurrentAvailableEnergyUI(actionData.PlayerStats.Mana);
    }

    private int ProcessAttack(ActionData actionData)
    {
        switch (actionData.CombatAction.AttackerAction.AttackType)
        {
            case AttackType.Basic:
                return Math.Max(0, (int)((actionData.Attacker.Attack * actionData.Attacker.Accuracy) + actionData.Attacker.Dexterity*0.5));
            case AttackType.CardAttack:
                return 0;
            case AttackType.FakeAttack:
                return 0;
            default:
                return 0;
        }
    }

    public bool IsCounterAttackSuccessful(int defenderDexterity, int attackerDexterity)
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

    public bool IsEvadeSuccessful(int defenderDexterity, float defenderAccuracy, int attackerDexterity)
    {
        int baseThreshold = 10;
        // Ajustar o limiar com base na diferença de estatísticas, afetado pela Accuracy do defensor
        float adjustedThreshold = baseThreshold + ((attackerDexterity * (1 - defenderAccuracy)) - defenderDexterity);

        // Calcular o bônus do defensor
        int defenderBonus = defenderDexterity;

        // Gerar um lançamento de dado de 1 a 20
        int roll = random.Next(1, 21);

        // Calcular o resultado final do defensor
        float defenderResult = roll + defenderBonus;

        // Retornar verdadeiro se o resultado for maior ou igual ao limiar ajustado
        return defenderResult >= adjustedThreshold;
    }

}
