using System.Linq;

public class ArmourBreak : IStrategy
{
    public Card Execute(EnemyContext context)
    {
        var target = context.defenderStats;
        if (target.Defense > context.attackerStats.ArmourPenetration) // Defesa alta
        {
            Card card = context.cardsInHand
                .Where(card => card.effects.Any(effect => effect.effectType == Card.CardType.Attack))
                .OrderByDescending(card => card.effects.Max(effect => effect.value) * context.attackerStats.ArmourPenetration)
                .FirstOrDefault(); // Carta com melhor sinergia para penetrar armadura

            context.selectedAttackStrategies.Add(ActionManager.AttackStrategy.ArmourBreak);
            return card;
        }
        return null; // Estratégia não se aplica
    }
}