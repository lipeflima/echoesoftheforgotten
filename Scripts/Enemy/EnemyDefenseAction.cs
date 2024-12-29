using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ActionManager;

public class EnemyDefenseAction : MonoBehaviour
{
    public void Update()
    {
        
    }
    public void Start()
    {
        /* var playerDeckManager = FindObjectOfType<PlayerDeckManager>();
        var attackerBattler = new Player("Aron", 3, 5, 5);
        var defenderBattler = new Enemy("Bolha", 1, 2, 2);
        var actionData = new ActionData
            {
                Attacker = attackerBattler,
                Defender = defenderBattler,
                CombatAction = new CombatAction
                {
                    AttackerAction = new(),
                    DefenderAction = new(),
                },
                CardData = new CardData(),
            };

        actionData.CardData.AttackerSelectedCards.Add(playerDeckManager.DrawCards(2));
        actionData.CombatAction.AttackerAction.Dexterity = 2;
        actionData.CombatAction.DefenderAction.Dexterity = 1;
        actionData.CombatAction.AttackerAction.Attack = 3;
        actionData.CombatAction.DefenderAction.Defense = 1;
        actionData.CombatAction.AttackerAction.ManaCost = 4;
        
        ExecuteDefense(
            actionData
        ); */
    }

    [SerializeField] private TurnManager turnManager;
    public void ExecuteDefense(ActionData data)
    {
        EnemyContext context = BuildEnemyContext(data);

        // Lista de estratégias organizadas por prioridade
        List<IStrategy> strategies = new List<IStrategy>
        {
            new BlockStrongAttacks(),
            new RegenerateHealth(),
            new CounterLowCostAttacks(),
            new PreserveEnergy(),
            new BestBuff()
        };

        int iterator = 0;
        bool strategyApplied = false;

        // Tentar encontrar no máximo duas cartas
        while (iterator < 2)
        {

            foreach (var strategy in strategies)
            {
                Card bestCard = strategy.Execute(context);
                if (bestCard != null)
                {
                    SelectCard(bestCard, context);
                    strategyApplied = true; 
                    break;
                }
            }

            if (context.selectedCards.Count >= 2 || !strategyApplied)
                break;

            iterator++;
        }

        context.HasStrategyApplied = strategyApplied;
        data.CombatAction.DefenderAction.DefenseType = DetermineDefenseStrategy(context);
    }

    private void SelectCard(Card card, EnemyContext context)
    {
        context.cardsInHand.Remove(card);
        context.selectedCards.Add(card);
    }

    // Constrói e retorna o contexto com base nos dados fornecidos
    private EnemyContext BuildEnemyContext(ActionData data)
    {
        return new EnemyContext
        {
            attackerStats = data.Attacker,
            defenderStats = data.Defender,
            cardsInHand = data.CardData.DefenderSelectedCards,
            availableEnergy = data.Defender.Mana, // Exemplo
            attackerData = data.CombatAction.AttackerAction,
            selectedCards = new(),
        };
    }

    private DefenseType DetermineDefenseStrategy(EnemyContext context)
    {
        if (!context.HasStrategyApplied)
        {
            Debug.Log("Nenhuma estrategia aplicada");
            return DefenseType.Basic;
        }
        if (HasDefenseCard(context)) {
            Debug.Log("Aplicando estrategia de BlockStrongAttack");
            return DefenseType.CardDefense;
        }
        if (HasHealthCard(context))
        {
            Debug.Log("Aplicando estrategia de RegenerateHealth");
            return DefenseType.Basic;
        }
        if (HasDexterityCard(context))
        {
            Debug.Log("Aplicando estrategia de CounterLowCOstAttack");
            return DefenseType.CounterAttack;
        }
        if (HasBuffOrDebuffCard(context))
        {
            Debug.Log("Aplicando estrategia de BestBuff");
            return DefenseType.Basic;
        }
        Debug.Log("Aplicando estrategia de evade");
        return DefenseType.Evade;
    }

    private bool HasDefenseCard(EnemyContext context) =>
        context.selectedCards.Any(card => card.effects.Any(effect => effect.effectType == Card.CardType.Defense));

    private bool HasHealthCard(EnemyContext context) =>
        context.selectedCards.Any(card => card.effects.Any(effect => effect.statName == "Health"));

    private bool HasDexterityCard(EnemyContext context) =>
        context.selectedCards.Any(card => card.effects.Any(effect => effect.statName == "Dexterity"));

    private bool HasBuffOrDebuffCard(EnemyContext context) =>
        context.selectedCards.Any(card => card.effects.Any(effect =>
            effect.effectType == Card.CardType.Buff || effect.effectType == Card.CardType.Debuff));
}
