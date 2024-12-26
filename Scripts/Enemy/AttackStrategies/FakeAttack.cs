using System.Linq;

public class FakeAttack : IStrategy
{
    private readonly float defenderHighManaThreshold = 20.0f; // Threshold para considerar o defensor com alta mana
    private readonly float attackerLowAccuracyThreshold = 50.0f; // Threshold para baixa Accuracy do atacante
    private readonly float defenderHighDexterityThreshold = 70.0f; // Threshold para alta Dexterity do defensor

    public Card Execute(EnemyContext context)
    {
        var attacker = context.attackerStats;
        var defender = context.defenderStats;

        // Condições para usar o FakeAttack
        bool isDefenderLikelyToReact = 
            defender.Mana > defenderHighManaThreshold ||
            defender.Dexterity > defenderHighDexterityThreshold ||
            defender.HasActiveBuff;

        bool isAttackerAtDisadvantage = 
            attacker.Accuracy < attackerLowAccuracyThreshold ||
            attacker.Mana < 10.0f; // Energia baixa do atacante

        if (isDefenderLikelyToReact || isAttackerAtDisadvantage)
        {
            // Busca cartas baratas e de impacto controlado
            Card card = context.cardsInHand
                .Where(card => card.EnergyCost <= 3 && card.effects.Any(effect =>
                    effect.effectType == Card.CardType.Attack ||
                    effect.effectType == Card.CardType.Debuff))
                .OrderBy(card => card.EnergyCost) // Prioriza menor custo
                .FirstOrDefault();

            context.selectedAttackStrategies.Add(ActionManager.AttackStrategy.FakeAttack);

            return card;
        }

        return null; // Estratégia não aplicável
    }
}