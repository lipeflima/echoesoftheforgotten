using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeckManager : MonoBehaviour
{
    public SharedCardPool cardPool;
    private Deck enemyDeck;
    [SerializeField] private List<Card> hand = new List<Card>();
    public int maxHandSize = 4;

    private void Start()
    {
        // Inicializa o deck com 20 cartas aleatórias
        List<Card> initialCards = new();
        for (int i = 0; i < 20; i++)
        {
            initialCards.Add(cardPool.GetRandomCard());
        }
        
        enemyDeck = new Deck(initialCards);

        DrawHand();
    }

    public void DrawHand()
    {
        for (int i = 0; i < maxHandSize; i++)
        {
            Card newCard = enemyDeck.DrawCard();
            hand.Add(newCard);
        }
    }

    public void PlayCard(Card selectedCard, Battler target)
    {
        enemyDeck.UseCard(selectedCard, target);
    }
}

