using System.Linq;

public class OpportunityStrike : IStrategy
{
    public Card Execute(EnemyContext context)
    {
        if (context.defenderStats.Mana < 2) // Baixa energia
        {
            Card card = context.cardsInHand
                .Where(card => card.EnergyCost <= context.attackerStats.Mana * 0.3 && card.cardType == Card.CardType.Attack) // Custo de mana baixo
                .OrderByDescending(card => card.effects.Max(effect => effect.value))
                .FirstOrDefault();

            context.selectedAttackStrategies.Add(ActionManager.AttackStrategy.OportunityStrike);

            return card;
        }
        return null; // Estratégia não se aplica
    }
}