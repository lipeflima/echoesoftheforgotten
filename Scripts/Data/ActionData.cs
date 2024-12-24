using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ActionManager;

public class ActionData : MonoBehaviour
{
    public List<Card> selectedCards = new List<Card>();
    public List<Card> hand = new List<Card>();
    public Battler defender { get; set; }
    public Battler attacker { get; set; }
    public AttackStrategy attack { get; set; }
    public DefenseStrategy defense { get; set; }
    public List<Card> deckCards = new();
    public int manaCost = 0;
    public int calculatedAttack { get; set; }
    public int calculatedDefense { get; set; }
    public int cardDamage { get; set; }
    public int attackerDogde { get; set; }
    public int calculatedCounterDamage { get; set; }
    // public AttackerAction attackerAction { get; set; }
}

