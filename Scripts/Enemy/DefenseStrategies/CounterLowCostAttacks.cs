using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ActionManager;

public class CounterLowCostAttacks : IStrategy
{
    [SerializeField] private int attackerLowManaCostBase = 2;
    [SerializeField] private int moderateAvailableEnergyBase = 3;
    private static System.Random random = new System.Random();
    public Card Execute(EnemyContext context)
    {
        Debug.Log($"Analisando CounterLowCost Strategy");
        if (context.attackerData.ManaCost <= attackerLowManaCostBase && context.availableEnergy >= moderateAvailableEnergyBase)
        {
            return context.cardsInHand
                .Where(card => card.manaCost <= context.availableEnergy)
                .Where(card => card.effects.Any(effect =>
                    (effect.effectType == Card.CardType.Defense && effect.statName == "Dexterity") ||
                    (effect.effectType == Card.CardType.Buff && effect.statName == "Dexterity") ||
                    (effect.effectType == Card.CardType.Debuff && effect.statName == "Dexterity")
                ))
                .OrderByDescending(card => {
                    int totalEffect = 0;
                    foreach (var effect in card.effects)
                    {
                        if ((effect.effectType == Card.CardType.Defense || effect.effectType == Card.CardType.Buff) 
                            && effect.statName == "Dexterity")
                        {
                            totalEffect += effect.value; // Incrementa a dexterity do defensor.
                        }
                        else if (effect.effectType == Card.CardType.Debuff && effect.statName == "Dexterity")
                        {
                            totalEffect -= effect.value; // Reduz a dexterity do atacante.
                        }
                    }

                    int roll = random.Next(1, 21);

                    // Ajusta a probabilidade para favorecer o contra-ataque
                    int adjustedThreshold = 10 + (context.attackerDexterity - (context.defenderDexterity + totalEffect));
                    int defenderRoll = roll + context.defenderDexterity + totalEffect;
                    return defenderRoll - adjustedThreshold; // Diferencial positivo favorece o defensor.
                })
                .FirstOrDefault();
        }
        return null;
    }
}