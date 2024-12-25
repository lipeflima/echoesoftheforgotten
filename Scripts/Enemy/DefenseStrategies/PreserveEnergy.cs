using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ActionManager;

public class PreserveEnergy : IStrategy
{
    [SerializeField] private int healthThreshold = 25; 
    [SerializeField] private int energyThreshold = 5;
    public Card Execute(EnemyContext context)
    {
        Debug.Log($"Analisando PreserveEnergy Strategy");
        if (context.availableEnergy <= energyThreshold)
        {
            if (context.defenderStats.Health > healthThreshold)
            {
                // Buscar cartas que aumentam energia
                return context.cardsInHand
                    .Where(card => card.manaCost <= context.availableEnergy)
                    .Where(card => card.effects.Any(effect =>
                        effect.effectType == Card.CardType.Buff && effect.statName == "Energy"))
                    .OrderByDescending(card => card.effects
                        .Where(effect => effect.statName == "Energy")
                        .Sum(effect => effect.value)) // Escolher a carta que aumenta mais a energia)
                    .FirstOrDefault();
            }
            else
            {
                // Buscar cartas com menor custo-benefício entre energia e melhorar defesa ou diminuir ataque do inimigo
                return context.cardsInHand
                    .Where(card => card.manaCost <= context.availableEnergy) 
                    .Where(card => card.effects.Any(effect =>
                        (effect.effectType == Card.CardType.Defense && effect.statName == "Defense") ||
                        (effect.effectType == Card.CardType.Buff && effect.statName == "Defense") ||
                        (effect.effectType == Card.CardType.Debuff && effect.statName == "Attack")))
                    .OrderBy(card =>
                    {
                        // Calcula o custo-benefício
                        int energyCost = card.manaCost;
                        int defenseGain = card.effects
                            .Where(effect => (effect.effectType == Card.CardType.Defense || effect.effectType == Card.CardType.Buff) &&
                                            effect.statName == "Defense")
                            .Sum(effect => effect.value);
                        int attackReduction = card.effects
                            .Where(effect => effect.effectType == Card.CardType.Debuff && effect.statName == "Attack")
                            .Sum(effect => effect.value);

                        // Cálculo do custo-benefício
                        return energyCost / (defenseGain + attackReduction + 1.0); // +1 para evitar divisão por zero
                    })
                    .FirstOrDefault();
            }
        }

        return null;
    }
}