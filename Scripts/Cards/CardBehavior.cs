using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public abstract class CardBehavior
{
    protected CardFeedbackManager cardFeedbackManager; // Pode ser acessado pelas subclasses
    public Card cardData { get; private set; }

    public CardBehavior(Card cardData)
    {
        this.cardData = cardData;
    }

    public abstract void ExecuteAction(Card card, Battler target);

    [System.Serializable]
    public class StatEffect
    {
        public string statName; // Nome da estatística, como "Health", "Attack", etc.
        public int value;       // Valor da alteração (positivo ou negativo).
        public CardType effectType;
        public int duration;
    }
}
