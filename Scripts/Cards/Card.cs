using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Card;

[CreateAssetMenu(fileName = "NewCard", menuName = "CardGame/Card")]
public class Card : ScriptableObject
{
    public enum CardType { Attack, Defense, Buff, Debuff }
    public string cardName;
    public string description;
    public int EnergyCost;
    public string behaviorClassName; // Nome da classe que implementa o comportamento
    public Sprite artwork;
    public List<CardEffectData> effects;
    public CardType cardType;
}

[System.Serializable]
public class CardEffectData
{
    public string statName; // Nome da estatística (e.g., "Health", "Attack")
    public int value;       // Valor a alterar
    public CardType effectType;
    public int duration;
}
