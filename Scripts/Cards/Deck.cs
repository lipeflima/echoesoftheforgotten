using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck
{
    private List<Card> cards;
    private List<Card> discardPile = new List<Card>();

    public Deck(List<Card> initialCards)
    {
        cards = new List<Card>(initialCards);
        Shuffle();
    }

    // Embaralhar o deck
    public void Shuffle()
    {
        System.Random rng = new System.Random();
        cards = cards.OrderBy(card => rng.Next()).ToList();
    }

    // Comprar uma carta
    public Card DrawOneCard()
    {
        if (cards.Count == 0) RecycleDiscardPile();
        if (cards.Count == 0) return null; // Deck vazio

        Card drawnCard = cards[0];
        cards.RemoveAt(0);
        return drawnCard;
    }

    // Comprar múltiplas cartas
    public List<Card> DrawCards(int count)
    {
        List<Card> drawnCards = new List<Card>();
        for (int i = 0; i < count; i++)
        {
            Card card = DrawOneCard();
            if (card != null) drawnCards.Add(card);
        }
        return drawnCards;
    }

    // Descartar cartas
    public void Discard(List<Card> cardsToDiscard)
    {
        discardPile.AddRange(cardsToDiscard);
    }

    public void Discard(Card card)
    {
        discardPile.Add(card);
    }

    // Reciclar a pilha de descarte
    private void RecycleDiscardPile()
    {
        cards = new List<Card>(discardPile);
        discardPile.Clear();
        Shuffle();
    }

    // Retornar o tamanho do deck
    public int Count => cards.Count;

    // Verificar se o deck está vazio
    public bool IsEmpty => cards.Count == 0;
}
