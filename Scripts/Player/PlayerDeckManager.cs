using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckManager : MonoBehaviour
{
    public SharedCardPool cardPool;
    public int maxHandSize = 4;

    private Deck playerDeck;
    [SerializeField] private List<Card> hand = new List<Card>();
    
    [SerializeField] private List<Card> discardedCards = new List<Card>();

    private void Awake()
    {
        // Inicializa o deck com 20 cartas aleatórias
        List<Card> initialCards = new();
        for (int i = 0; i < 20; i++)
        {
            initialCards.Add(cardPool.GetRandomCard());
        }
        
        playerDeck = new Deck(initialCards);

        DrawHand();
    }

    public void DrawHand()
    {
        for (int i = 0; i < maxHandSize; i++)
        {
            hand.Add(playerDeck.DrawCard());
        }
    }

    public Card DrawCards(int amount)
    {
        Card newCard = playerDeck.DrawCard();
        for (int i = 0; i < amount; i++)
        {
            hand.Add(newCard);
        }

        return newCard;        
    }

    public void Discard(List<Card> cards)
    {
        var itemsToRemove = new List<Card>(cards);
        foreach (var card in itemsToRemove)
        {
            hand.Remove(card);
            discardedCards.Add(card);
        }
    }

    public void Shuffle()
    {
        playerDeck.Shuffle();
    }

    public void PlayCard(Card selectedCard, CharacterStats user, CharacterStats target)
    {
        playerDeck.UseCard(selectedCard, user, target);
    }

    public List<Card> GetPlayerHand()
    {
        return hand;
    }

    public List<Card> GetPlayerDeckCards()
    {
        return playerDeck.GetDeckCards();
    }

    public int GetMaxHandSize()
    {
        return maxHandSize;
    }
}

