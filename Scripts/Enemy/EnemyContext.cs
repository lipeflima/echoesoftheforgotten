using System.Collections.Generic;
using static ActionManager;

public class EnemyContext
{
    public CharacterStats attackerStats; // Estatísticas do atacante
    public CharacterStats defenderStats; // Estatísticas do defensor
    public List<Card> cardsInHand; // Cartas disponíveis para defesa
    public int availableEnergy; // Energia disponível para usar cartas
    public AttackerAction attackerData; // Última ação do atacante
    public List<Card> selectedCards;
    public List<AttackStrategy> selectedAttackStrategies;
    public List<DefenseStrategy> selecteddefenseStrategies;
    public DefenseType defenseType;
    public DefenseStrategy defenseStrategy;
    public AttackType attackType;
    public AttackStrategy attackStrategy;
    public bool HasStrategyApplied = false;
}