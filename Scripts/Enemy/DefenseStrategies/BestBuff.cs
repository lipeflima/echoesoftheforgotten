using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ActionManager;

public class BestBuff : IStrategy
{
    public Card Execute(EnemyContext context)
    {
        if (context.cardsInHand.Count > 0)
        {
            return context.cardsInHand
                .Where(card => card.EnergyCost <= context.availableEnergy)
                .Select(card => new 
                {
                    Card = card,
                    BestEffectValue = card.effects
                        .Where(effect => effect.effectType == Card.CardType.Buff || effect.effectType == Card.CardType.Debuff)
                        .Any() // Verifica se há elementos
                        ? card.effects
                            .Where(effect => effect.effectType == Card.CardType.Buff || effect.effectType == Card.CardType.Debuff)
                            .Max(effect => effect.value) // Obtém o valor máximo se houver elementos
                        : 0 // Define como 0 se não houver elementos
                })
                .Where(entry => entry.BestEffectValue > 0) // Garante que há um efeito válido
                .OrderByDescending(entry => entry.BestEffectValue) // Ordena pelas cartas com o maior efeito
                .Select(entry => entry.Card)
                .FirstOrDefault();
        }
        return null;
    }
}