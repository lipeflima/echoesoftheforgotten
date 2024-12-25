using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ActionManager;
public class RegenerateHealth : IStrategy
{
    public Card Execute(EnemyContext context)
    {
        Debug.Log($"Analisando RegenerateHealth Strategy");
        if (context.defenderStats.Health >= 20)
        {
            return context.cardsInHand
                .Where(card => card.manaCost <= context.availableEnergy) // Filtra cartas com custo válido
                .Where(card => card.effects.Any(effect => effect.effectType == Card.CardType.Buff && effect.statName == "Health")) // Verifica se a carta aumenta Health
                .OrderBy(card => card.manaCost / card.effects
                    .Where(effect => effect.effectType == Card.CardType.Buff && effect.statName == "Health")
                    .Sum(effect => effect.value)) // Ordena pelo menor custo-benefício
                .FirstOrDefault();
        }
        return null;
    }
}