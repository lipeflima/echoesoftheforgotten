using System.Linq;

public class CriticalStrike : IStrategy
{
    public Card Execute(EnemyContext context)
    {
        if (context.attackerStats.CriticalChance > 0.3) // Chance de crítico considerável
        {
            Card card = context.cardsInHand
                .Where(card => card.effects.Any(effect => effect.effectType == Card.CardType.Attack && effect.value > 0))
                .OrderByDescending(card => context.attackerStats.CriticalDamage * card.effects.Max(effect => effect.value))
                .FirstOrDefault(); // Carta com maior sinergia de crítico

            context.selectedAttackStrategies.Add(ActionManager.AttackStrategy.CriticalStrike);

            return card;
        }
        return null; // Estratégia não se aplica
    }
}