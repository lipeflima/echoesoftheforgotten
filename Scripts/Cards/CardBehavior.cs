using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class CardBehavior : MonoBehaviour
{
    public Card cardData;
    public void ExecuteAction(Battler target)
    {
        foreach (var effect in cardData.effects)
        {
            switch (effect.statName)
            {
                case "Health":
                    // target.ModifyHealth(effect.value);
                    break;
                case "Attack":
                    // target.ModifyAttack(effect.value);
                    break;
                // Adicione mais casos para outras estatísticas
            }
        }
    }

    [System.Serializable]
    public class StatEffect
    {
        public string statName; // Nome da estatística, como "Health", "Attack", etc.
        public int value;       // Valor da alteração (positivo ou negativo).
        public CardType effectType;
        public int duration;
    }
}
