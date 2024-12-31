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
}
