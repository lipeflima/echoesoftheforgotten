using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ActionManager;

public class EnemyAttackAction : MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;
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

    public void ExecuteAttack(ActionData data)
    {
        EnemyContext context = BuildEnemyContext(data);

        // Lista de estratégias organizadas por prioridade
        List<IStrategy> strategies = new List<IStrategy>
        {
            new ArmourBreak(),
            new CriticalStrike(),
            new DisruptAccuracy(),
            new ExploitWeakDefense(),
            new FocusedAttack(),
            new HealthRecovery(),
            new ManaRecovery(),
            new OpportunityStrike(),
            new StatBalancer(),
            new FakeAttack(),
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
        
        context.attackStrategy = DetermineAttackStrategy(context.selectedAttackStrategies);
        context.HasStrategyApplied = strategyApplied;
        data.CombatAction.AttackerAction.AttackType = DetermineAttackType(context);
        Debug.Log($"Enemy Attack Strategy: {data.CombatAction.AttackerAction.AttackType}");
        Debug.Log($"Enemy Attack Type: {context.attackStrategy}");
    }

    private void SelectCard(Card card, EnemyContext context)
    {
        Debug.Log($"Inimigo selecionou uma carta: {card.cardName}");
        context.cardsInHand.Remove(card);
        context.selectedCards.Add(card);
    }

    // Constrói e retorna o contexto com base nos dados fornecidos
    private EnemyContext BuildEnemyContext(ActionData data)
    {
        return new EnemyContext
        {
            attackerStats = data.EnemyStats,
            defenderStats = data.PlayerStats,
            cardsInHand = data.CardData.AttackerSelectedCards,
            availableEnergy = data.EnemyStats.Mana,
            selectedCards = new(),
            selectedAttackStrategies = new(),
        };
    }

    private AttackStrategy DetermineAttackStrategy(List<AttackStrategy> attackStrategies)
    {
        // TODO: definir de forma melhor no futuro
        return attackStrategies[0];
    }

    private AttackType DetermineAttackType(EnemyContext context)
    {
        if (HasAppliedArmourBreakStategy(context)) return AttackType.CardAttack;
        if (HasAppliedCriticalStrike(context)) return AttackType.CardAttack;
        if (HasAppliedDisruptAccuracy(context)) return AttackType.Basic;
        if (HasAppliedExploitWeakDefense(context)) return AttackType.CardAttack;
        if (HasAppliedFocusedAttack(context)) return AttackType.Basic;
        if (HasAppliedOportunityStrike(context)) return AttackType.CardAttack;
        if (HasAppliedStatBalancer(context)) return AttackType.Basic;
        if (HasAppliedFakeAttack(context)) return AttackType.FakeAttack;
        if (HasAppliedHealthRecovery(context)) return AttackType.Basic;
        if (HasAppliedManaRecovery(context)) return AttackType.Basic;
        return AttackType.Basic;
    }

    private bool HasAppliedArmourBreakStategy(EnemyContext context) =>
        context.attackStrategy == ActionManager.AttackStrategy.ArmourBreak;

    private bool HasAppliedCriticalStrike(EnemyContext context) =>
        context.attackStrategy == ActionManager.AttackStrategy.CriticalStrike;

    private bool HasAppliedDisruptAccuracy(EnemyContext context) =>
        context.attackStrategy == ActionManager.AttackStrategy.DisruptAccuracy;

    private bool HasAppliedExploitWeakDefense(EnemyContext context) =>
        context.attackStrategy == ActionManager.AttackStrategy.ExploitWeakDefense;

    private bool HasAppliedFocusedAttack(EnemyContext context) =>
        context.attackStrategy == ActionManager.AttackStrategy.FocusedAttack;

    private bool HasAppliedOportunityStrike(EnemyContext context) =>
        context.attackStrategy == ActionManager.AttackStrategy.OportunityStrike;

    private bool HasAppliedStatBalancer(EnemyContext context) =>
        context.attackStrategy == ActionManager.AttackStrategy.StatBalancer;

    private bool HasAppliedFakeAttack(EnemyContext context) =>
        context.attackStrategy == ActionManager.AttackStrategy.FakeAttack;
    
    private bool HasAppliedHealthRecovery(EnemyContext context) =>
        context.attackStrategy == ActionManager.AttackStrategy.HealthRecovery;
    
    private bool HasAppliedManaRecovery(EnemyContext context) =>
        context.attackStrategy == ActionManager.AttackStrategy.ManaRecovery;
}
