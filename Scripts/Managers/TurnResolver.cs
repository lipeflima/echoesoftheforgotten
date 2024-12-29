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
    [SerializeField] private GeneralUI generalUI;
    private static System.Random random = new System.Random();
    public void ResolveTurn(ActionData actionData)
    {  
        // 1. Aplica mana
        ApplyManaEnergy(actionData);
        // 2. Aplica buffs e debuffs
        ApplyBuffsAndDebuffs(actionData);
        
        // 3. Processa o ataque
        int damage = ProcessAttack(actionData);
        int avoidedDamage = ProcessDefense(actionData);

        Debug.Log($"Turn damage: {damage}");
        Debug.Log($"Turn avoided Damage: {avoidedDamage}");

        if (actionData.CardData.AttackerSelectedCards.Count  > 0)
        {
            foreach(Card attackCard in actionData.CardData.AttackerSelectedCards)
            {
                ApplyCardEffects(attackCard, actionData.Attacker, actionData.Defender);
            }
        }
        if (actionData.CardData.DefenderSelectedCards.Count > 0)
        {
            foreach(Card defenseCard in actionData.CardData.DefenderSelectedCards)
            {
                ApplyCardEffects(defenseCard, actionData.Attacker, actionData.Defender);
            }
        }

        bool hasCounterAttack = IsCounterAttackSuccessful(actionData.Defender.Dexterity, actionData.Attacker.Dexterity) && actionData.CombatAction.DefenderAction.DefenseType == DefenseType.CounterAttack;

        // 4. Aplica o resultado ao defensor ou Aplica efeitos de contra-ataque no atacante, se houver.
        if (!hasCounterAttack) {
            actionData.Defender.ApplyDamage(damage - avoidedDamage);
        } else {
            actionData.Attacker.ApplyDamage(-avoidedDamage);
        }
    }

    private void ApplyCardEffects(Card card, Battler attacker, Battler defender)
    {
        // CardResolver.Resolve(card).ExecuteAction(attacker, defender);
    }


    private void ApplyManaEnergy(ActionData actionData)
    {
        Debug.Log($"Resolvendo energia de Mana");
        actionData.Attacker.Mana = actionData.Attacker.IsPlayer ? actionData.Attacker.Mana - actionData.PlayerTurnSpentEnergy : actionData.Attacker.Mana - actionData.EnemyTurnSpentEnergy;
        actionData.Defender.Mana = actionData.Defender.IsPlayer ? actionData.Defender.Mana - actionData.PlayerTurnSpentEnergy : actionData.Defender.Mana - actionData.EnemyTurnSpentEnergy;
        generalUI.SetPlayerCurrentAvailableEnergyUI(actionData.PlayerStats.Mana);
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
                return 0; 
            default:
                break;
        }
        return 0;
    }

    private void ApplyBuffsAndDebuffs(ActionData actionData)
    {
        List<CardEffectData> effects = actionData.CombatAction.AttackerAction.CardEffects;
        Battler attacker = actionData.Attacker;
        Battler defender = actionData.Defender;
        CurrentTurnAction turnAction = actionData.CurrentTurnAction;

        foreach(var effect in effects)
        {
            switch (effect.effectType)
            {
                // Buff e Debuff é automaticamente aplicado no battler que usou a carta
                case Card.CardType.Buff:
                    ApplyBuff(effect, turnAction == CurrentTurnAction.Attack ? attacker : defender);
                    break;
                case Card.CardType.Debuff:
                    ApplyBuff(effect, turnAction == CurrentTurnAction.Attack ? defender : attacker);
                    break;
            }
        }
    }

    public void ApplyBuff(CardEffectData effect, Battler target)
    {
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

    public class DebugLogger
{
    public static void LogProperties(object obj, int level = 0, int maxDepth = 3)
    {
        if (obj == null)
        {
            Debug.Log($"{new string(' ', level * 2)}Object is null");
            return;
        }

        // Obter o tipo do objeto
        var type = obj.GetType();
        Debug.Log($"{new string(' ', level * 2)}Logging properties of object: {type.Name}");

        // Prevenir loops infinitos
        if (level >= maxDepth)
        {
            Debug.Log($"{new string(' ', (level + 1) * 2)}Max depth reached.");
            return;
        }

        // Obter as propriedades públicas
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            try
            {
                // Obter o nome e valor da propriedade
                var name = property.Name;
                var value = property.GetValue(obj, null);

                // Logar a propriedade
                Debug.Log($"{new string(' ', (level + 1) * 2)}{name}: {value}");

                // Se a propriedade é um objeto (e não um tipo primitivo ou string), logar recursivamente
                if (value != null && !(value is string) && !(value is ValueType) && !(value is IEnumerable))
                {
                    LogProperties(value, level + 1, maxDepth);
                }
            }
            catch
            {
                Debug.LogWarning($"{new string(' ', (level + 1) * 2)}Failed to log property: {property.Name}");
            }
        }
    }
}
}
