using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SharedCardPool", menuName = "CardGame/SharedCardPool")]
public class SharedCardPool : ScriptableObject
{
    public List<Card> allCards;

    public Card GetRandomCard()
    {
        if (allCards == null || allCards.Count == 0) 
        {
            Debug.LogError("SharedCardPool: allCards está vazio ou não inicializado.");
            return null;
        }
        return allCards[Random.Range(0, allCards.Count)];
    }

    public Card GetRandomAttackCard()
    {
        if (allCards == null || allCards.Count == 0)
        {
            Debug.LogError("SharedCardPool: allCards está vazio ou não inicializado.");
            return null;
        }

        // Filtra as cartas com o cardType especificado
        var filteredCards = allCards.Where(card => (card.cardType == Card.CardType.Attack) || (card.cardType == Card.CardType.Buff) || (card.cardType == Card.CardType.Debuff)).ToList();

        if (filteredCards == null || filteredCards.Count == 0)
        {
            Debug.LogWarning($"Nenhuma carta dos tipos encontrada.");
            return null;
        }

        // Retorna uma carta aleatória do tipo filtrado
        return filteredCards[Random.Range(0, filteredCards.Count)];
    }

    public Card GetRandomDefenseCard()
    {
        if (allCards == null || allCards.Count == 0)
        {
            Debug.LogError("SharedCardPool: allCards está vazio ou não inicializado.");
            return null;
        }

        // Filtra as cartas com o cardType especificado
        var filteredCards = allCards.Where(card => (card.cardType == Card.CardType.Defense) || (card.cardType == Card.CardType.Buff) || (card.cardType == Card.CardType.Debuff)).ToList();

        if (filteredCards == null || filteredCards.Count == 0)
        {
            Debug.LogWarning($"Nenhuma carta dos tipos encontrada.");
            return null;
        }

        // Retorna uma carta aleatória do tipo filtrado
        return filteredCards[Random.Range(0, filteredCards.Count)];
    }
}
