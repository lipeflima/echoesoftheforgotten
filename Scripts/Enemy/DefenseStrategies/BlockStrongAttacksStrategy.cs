using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ActionManager;

public class BlockStrongAttacks : IStrategy
{
    [SerializeField] private int energyThreshold = 5;
    [SerializeField] private int strongAttackThreshold = 10;
    public Card Execute(EnemyContext context)
    {
        // Avaliar se o ataque é forte
        if (context.attackerStats.Attack > strongAttackThreshold && context.availableEnergy >= energyThreshold)
        {
            // Use uma carta de bloqueio forte
            return context.cardsInHand
            .Where(card => card.EnergyCost <= context.availableEnergy)
            .Where(card => card.effects.Any(effect =>
                effect.effectType == Card.CardType.Defense ||
                effect.effectType == Card.CardType.Buff ||
                (effect.effectType == Card.CardType.Debuff && effect.statName == "Attack")
            ))
            .OrderByDescending(card => card.effects.Sum(effect =>
                effect.effectType == Card.CardType.Defense ? effect.value : 
                effect.effectType == Card.CardType.Buff ? effect.value :
                effect.effectType == Card.CardType.Debuff && effect.statName == "Attack" ? -effect.value : 0
            ))
            .FirstOrDefault();
        }

        return null;
    }
}