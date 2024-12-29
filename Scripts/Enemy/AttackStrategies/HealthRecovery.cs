using System.Linq;

public class HealthRecovery : IStrategy
{
    private readonly float healthThreshold = 30.0f; // Threshold para saúde baixa

    public Card Execute(EnemyContext context)
    {
        var attacker = context.attackerStats;

        if (attacker.Health < healthThreshold)
        {
            // Busca cartas que aumentam a saúde
            Card card = context.cardsInHand
                .Where(card => card.effects.Any(effect => effect.statName == "Health" && effect.effectType == Card.CardType.Buff))
                .OrderByDescending(card => card.effects.Max(effect => effect.value)) // Prioriza o maior aumento de saúde
                .FirstOrDefault();

            context.selectedAttackStrategies.Add(ActionManager.AttackStrategy.HealthRecovery);

            return card;
        }

        return null; // Estratégia não aplicável
    }
}