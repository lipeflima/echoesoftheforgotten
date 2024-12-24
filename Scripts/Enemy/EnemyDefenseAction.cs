using System.Collections;
using System.Collections.Generic;
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

        // Iterar pelas estratégias e executar a primeira válida
        foreach (var strategy in strategies)
        {
            if (strategy.Execute(context))
                break; // Uma estratégia válida foi aplicada
        }
    }

    // Constrói e retorna o contexto com base nos dados fornecidos
    private EnemyContext BuildEnemyContext(ActionData data)
    {
        return new EnemyContext
        {
            attackerStats = data.attacker,
            defenderStats = data.defender,
            cardsInHand = data.hand,
            availableEnergy = GetAvailableEnergy(), // Exemplo
            lastAction = data
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
    public ActionData lastAction; // Última ação do atacante
}

public interface IStrategy
{
    bool Execute(EnemyContext context);
}

public class BlockStrongAttacks : IStrategy
{
    public bool Execute(EnemyContext context)
    {
        // Avaliar se o ataque é forte
        if (context.lastAction.defender.Attack > 10 && context.availableEnergy >= 5)
        {
            // Use uma carta de bloqueio forte
            Card bestBlockCard = context.cardsInHand.Find(card => card.cardName == "Strong Block");
            if (bestBlockCard != null)
            {
                PlayCard(bestBlockCard);
                return true; // Estratégia aplicada
            }
        }
        return false; // Estratégia não aplicável
    }

    private void PlayCard(Card card)
    {
        Debug.Log($"Inimigo jogou a carta: {card.cardName}");
    }
}

public class CounterLowCostAttacks : IStrategy
{
    public bool Execute(EnemyContext context)
    {
        if (context.lastAction.manaCost <= 2 && context.availableEnergy >= 3)
        {
            Card counterCard = context.cardsInHand.Find(card => card.cardName == "Quick Counter");
            if (counterCard != null)
            {
                PlayCard(counterCard);
                return true;
            }
        }
        return false;
    }

    private void PlayCard(Card card)
    {
        Debug.Log($"Inimigo usou contra-ataque: {card.cardName}");
    }
}

public class PreserveEnergy : IStrategy
{
    public bool Execute(EnemyContext context)
    {
        if (context.availableEnergy <= 3)
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
            PlayCard(randomCard);
            return true;
        }
        return false;
    }

    private void PlayCard(Card card)
    {
        Debug.Log($"Inimigo jogou uma carta aleatória: {card.cardName}");
    }
}
