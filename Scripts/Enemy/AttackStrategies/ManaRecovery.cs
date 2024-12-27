using System.Linq;

public class ManaRecovery : IStrategy
{
    private readonly float manaThreshold = 20.0f; // Threshold para mana baixa

    public Card Execute(EnemyContext context)
    {
        var attacker = context.attackerStats;

        if (attacker.Mana < manaThreshold)
        {
            // Busca cartas que aumentam a mana
            Card card = context.cardsInHand
                .Where(card => card.effects.Any(effect => effect.statName == "Mana" && effect.effectType == Card.CardType.Buff))
                .OrderByDescending(card => card.effects.Max(effect => effect.value)) // Prioriza o maior aumento de mana
                .FirstOrDefault();

            context.selectedAttackStrategies.Add(ActionManager.AttackStrategy.ManaRecovery);

            return card;
        }

        return null; // Estratégia não aplicável
    }
}