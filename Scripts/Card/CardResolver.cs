using UnityEngine;
using System;

public class CardResolver
{
    public static CardBehavior Resolve(Card cardData)
    {
        // Usa reflexão para instanciar o comportamento pelo nome da classe
        Type behaviorType = Type.GetType(cardData.behaviorClassName);
        if (behaviorType == null)
        {
            Debug.LogError($"Comportamento para {cardData.cardName} não encontrado: {cardData.behaviorClassName}");
            return null;
        }

        return (CardBehavior)Activator.CreateInstance(behaviorType, cardData);
    }
}
