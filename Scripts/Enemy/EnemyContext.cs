using System.Collections.Generic;
using static ActionManager;

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
    public DefenseStrategy defenseStrategy;
    public bool HasStrategyApplied = false;
}