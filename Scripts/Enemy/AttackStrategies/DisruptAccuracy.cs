using System.Linq;

public class DisruptAccuracy : IStrategy
{
    public Card Execute(EnemyContext context)
    {
        Card card = context.cardsInHand
            .Where(card => card.effects.Any(effect => effect.effectType == Card.CardType.Debuff && effect.statName == "Accuracy"))
            .OrderByDescending(card => card.effects.Max(effect => effect.value))
            .FirstOrDefault(); // Carta que aplica debuff de precisão

        context.selectedAttackStrategies.Add(ActionManager.AttackStrategy.DisruptAccuracy);

        return card;
    }
}