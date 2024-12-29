using System.Collections.Generic;
using System.Linq;

public class FocusedAttack : IStrategy
{
    // Thresholds específicos para cada estatística
    private readonly Dictionary<string, float> thresholds = new Dictionary<string, float>
    {
        { "Dexterity", 70.0f },
        { "Attack", 80.0f },
        { "Defense", 60.0f },
        { "Mana", 50.0f },
        { "Health", 40.0f }
    };

    public Card Execute(EnemyContext context)
    {
        var target = context.defenderStats;

        // Identifica a estatística mais alta para explorar com base nos thresholds
        var stats = new Dictionary<string, float>
        {
            { "Dexterity", target.Dexterity },
            { "Attack", target.Attack },
            { "Defense", target.Defense },
            { "Mana", target.Mana },
            { "Health", target.Health }
        };

        var statToExploit = stats
            .Where(stat => thresholds.ContainsKey(stat.Key) && stat.Value >= thresholds[stat.Key]) // Apenas estatísticas que superam o threshold
            .OrderByDescending(stat => stat.Value) // Ordena pela estatística mais alta
            .FirstOrDefault();

        if (!string.IsNullOrEmpty(statToExploit.Key))
        {
            // Procura uma carta que afete a estatística identificada
            Card card = context.cardsInHand
                .Where(card => card.effects.Any(effect => effect.statName == statToExploit.Key && effect.effectType == Card.CardType.Debuff))
                .OrderByDescending(card => card.effects.Max(effect => effect.value)) // Prioriza maior valor de debuff
                .FirstOrDefault();

            context.selectedAttackStrategies.Add(ActionManager.AttackStrategy.FocusedAttack);

            return card;
        }

        return null; // Estratégia não se aplica
    }
}
