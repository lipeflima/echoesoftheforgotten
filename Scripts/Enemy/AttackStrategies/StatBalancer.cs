using System.Collections.Generic;
using System.Linq;

public class StatBalancer : IStrategy
{
    public Card Execute(EnemyContext context)
    {
        var attacker = context.attackerStats;
        var defender = context.defenderStats;

        // Compara as estatísticas relevantes
        var statComparison = new List<(string statName, float attackerValue, float defenderValue)>
        {
            ("Dexterity", attacker.Dexterity, defender.Dexterity),
            ("Attack", attacker.Attack, defender.Attack),
            ("Defense", attacker.Defense, defender.Defense),
            ("Mana", attacker.Mana, defender.Mana),
            ("Health", attacker.Health, defender.Health)
        };

        // Ordena pela maior desvantagem do atacante
        var statToBalance = statComparison
            .OrderByDescending(stat => stat.defenderValue - stat.attackerValue)
            .FirstOrDefault(stat => stat.defenderValue > stat.attackerValue); // Só considera se o defensor tiver vantagem

        if (!string.IsNullOrEmpty(statToBalance.statName))
        {
            // Procura cartas que afetam a estatística em desvantagem
            Card card = context.cardsInHand
                .Where(card => card.effects.Any(effect =>
                    (effect.statName == statToBalance.statName && effect.effectType == Card.CardType.Debuff) || // Reduz o defensor
                    (effect.statName == statToBalance.statName && effect.effectType == Card.CardType.Buff)))    // Melhora o atacante
                .OrderByDescending(card => card.effects.Max(effect => effect.value)) // Prioriza maior valor de efeito
                .FirstOrDefault();

            context.selectedAttackStrategies.Add(ActionManager.AttackStrategy.StatBalancer);

            return card;
        }

        return null; // Estratégia não se aplica
    }
}