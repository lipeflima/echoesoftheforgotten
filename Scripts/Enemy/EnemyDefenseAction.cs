using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDefenseAction : MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;
    public void ExecuteDefense(ActionData data)
    {
        EnemyContext context = BuildEnemyContext(data);

        // Lista de estratégias organizadas por prioridade
        List<IStrategy> strategies = new List<IStrategy>
        {
            new BlockStrongAttacks(),
            new CounterLowCostAttacks(),
            new PreserveEnergy(),
            new RandomDefense()
        };

        while(data.CardData.DefenderSelectedCards.Count < 2)
        {
            // Iterar pelas estratégias e executar a primeira válida
            foreach (var strategy in strategies)
            {
                if (strategy.Execute(context))
                    break; // Uma estratégia válida foi aplicada
            }
        } 
    }

    // Constrói e retorna o contexto com base nos dados fornecidos
    private EnemyContext BuildEnemyContext(ActionData data)
    {
        return new EnemyContext
        {
            attackerStats = data.Attacker,
            defenderStats = data.Defender,
            cardsInHand = data.CardData.AttackerSelectedCards,
            availableEnergy = GetAvailableEnergy(), // Exemplo
            attackerData = data.CombatAction.AttackerAction,
            selectedCards = new(),
            attackerDexterity = data.CombatAction.AttackerAction.Dexterity,
            defenderDexterity = data.CombatAction.DefenderAction.Dexterity,
        };
    }

    private int GetAvailableEnergy()
    {
        return turnManager.GetAvailableEnergy();
    }
}

public class EnemyContext
{
    public Battler attackerStats; // Estatísticas do atacante
    public Battler defenderStats; // Estatísticas do defensor
    public List<Card> cardsInHand; // Cartas disponíveis para defesa
    public int availableEnergy; // Energia disponível para usar cartas
    public AttackerAction attackerData; // Última ação do atacante
    public List<Card> selectedCards;
    public int attackerDexterity;
    public int defenderDexterity;
}

public interface IStrategy
{
    bool Execute(EnemyContext context);
}

public class BlockStrongAttacks : IStrategy
{
    [SerializeField] private int moderateEnergyBase = 5;
    [SerializeField] private int strongAttackBase = 10;
    public bool Execute(EnemyContext context)
    {
        // Avaliar se o ataque é forte
        if (context.attackerData.Attack > strongAttackBase && context.availableEnergy >= moderateEnergyBase)
        {
            // Use uma carta de bloqueio forte
            Card bestBlockCard = context.cardsInHand
            .Where(card => card.effects.Any(effect =>
                effect.effectType == Card.CardType.Defense ||
                effect.effectType == Card.CardType.Buff ||
                (effect.effectType == Card.CardType.Debuff && effect.statName == "Attack")
            ))
            .OrderByDescending(card => card.effects.Sum(effect =>
                effect.effectType == Card.CardType.Defense ? effect.value : 
                effect.effectType == Card.CardType.Buff ? effect.value :
                effect.effectType == Card.CardType.Debuff && effect.statName == "Attack" ? -effect.value : 0
            ))
            .FirstOrDefault();

            if (bestBlockCard != null)
            {
                SelectCard(bestBlockCard, context);
                return true; // Estratégia aplicada
            }
        }
        return false; // Estratégia não aplicável
    }

    private void SelectCard(Card card, EnemyContext context)
    {
        Debug.Log($"Inimigo jogou a carta: {card.cardName}");
        context.selectedCards.Add(card);
    }
}

public class CounterLowCostAttacks : IStrategy
{
    [SerializeField] private int attackerLowManaCostBase = 2;
    [SerializeField] private int moderateAvailableEnergyBase = 3;
    private static System.Random random = new System.Random();
    public bool Execute(EnemyContext context)
    {
        if (context.attackerData.ManaCost <= attackerLowManaCostBase && context.availableEnergy >= moderateAvailableEnergyBase)
        {
            Card counterCard = context.cardsInHand
                .Where(card => card.effects.Any(effect =>
                    (effect.effectType == Card.CardType.Defense && effect.statName == "Dexterity") ||
                    (effect.effectType == Card.CardType.Buff && effect.statName == "Dexterity") ||
                    (effect.effectType == Card.CardType.Debuff && effect.statName == "Dexterity")
                ))
                .OrderByDescending(card => {
                    int totalEffect = 0;
                    foreach (var effect in card.effects)
                    {
                        if ((effect.effectType == Card.CardType.Defense || effect.effectType == Card.CardType.Buff) 
                            && effect.statName == "Dexterity")
                        {
                            totalEffect += effect.value; // Incrementa a dexterity do defensor.
                        }
                        else if (effect.effectType == Card.CardType.Debuff && effect.statName == "Dexterity")
                        {
                            totalEffect -= effect.value; // Reduz a dexterity do atacante.
                        }
                    }

                    int roll = random.Next(1, 21);

                    // Ajusta a probabilidade para favorecer o contra-ataque
                    int adjustedThreshold = 10 + (context.attackerDexterity - (context.defenderDexterity + totalEffect));
                    int defenderRoll = roll + context.defenderDexterity + totalEffect;
                    return defenderRoll - adjustedThreshold; // Diferencial positivo favorece o defensor.
                })
                .FirstOrDefault();

            if (counterCard != null)
            {
                SelectCard(counterCard, context);
                return true;
            }
        }
        return false;
    }

    private void SelectCard(Card card, EnemyContext context)
    {
        Debug.Log($"Inimigo usou contra-ataque: {card.cardName}");
        context.selectedCards.Add(card);
    }
}

public class PreserveEnergy : IStrategy
{
    [SerializeField] private int minHealthToAcceptDamage = 25; // mudar para porcentagem do total
    public bool Execute(EnemyContext context)
    {
        if (context.availableEnergy <= 3 && context.defenderStats.Health > minHealthToAcceptDamage)
        {
            Debug.Log("Inimigo decidiu preservar energia.");
            return true; // Simula um turno defensivo
        }
        return false;
    }
}

public class RandomDefense : IStrategy
{
    public bool Execute(EnemyContext context)
    {
        if (context.cardsInHand.Count > 0)
        {
            Card randomCard = context.cardsInHand[Random.Range(0, context.cardsInHand.Count)];
            SelectCard(randomCard);
            return true;
        }
        return false;
    }

    private void SelectCard(Card card)
    {
        Debug.Log($"Inimigo jogou uma carta aleatória: {card.cardName}");
    }
}
