using System.Collections;
using System.Collections.Generic;
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
}
