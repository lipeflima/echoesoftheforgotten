using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private List<Card> cards = new List<Card>();

    public Deck(List<Card> initialCards)
    {
        cards = new List<Card>(initialCards);
        Shuffle();
    }

    public void Shuffle()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int randomIndex = Random.Range(0, cards.Count);
            (cards[i], cards[randomIndex]) = (cards[randomIndex], cards[i]);
        }
    }

    public Card DrawCard()
    {
        if (cards.Count == 0) return null;
        Card topCard = cards[0];
        cards.RemoveAt(0);
        return topCard;
    }

    public void AddCard(Card card)
    {
        cards.Add(card);
    }

    public void Remove(Card card)
    {
        cards.Remove(card);
    }

    public List<Card> GetDeckCards()
    {
        return cards;
    }

    public void UseCard(Card selectedCard, CharacterStats user, CharacterStats target)
    {
        // Resolve o comportamento da carta
        CardBehavior behavior = CardResolver.Resolve(selectedCard);

        if (behavior != null)
        {
            // Executa a ação da carta
            behavior.ExecuteAction(user, target);
        }
        else
        {
            Debug.LogError("Não foi possível executar a ação da carta.");
        }
    }
}
