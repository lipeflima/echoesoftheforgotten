using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class PlayerDeckManager : MonoBehaviour
{
    private Deck attackDeck;
    private Deck defenseDeck;

    [SerializeField] private List<Card> attackHand = new List<Card>();
    [SerializeField] private List<Card> defenseHand = new List<Card>();
    private int maxHandSize = 4;
    private int minimumDeckSize = 20;
    [SerializeField] private List<Card> discardPile = new();
    [SerializeField] private SharedCardPool cardPool;

    public void Awake()
    {
        InitializeDecks();
    }

    // Inicializar uma mão com maxHandSize cartas
    private void DrawHand(Deck deck, List<Card> hand)
    {
        hand.AddRange(deck.DrawCards(maxHandSize));
    }

    // Comprar cartas para a mão de ataque
    public void DrawToAttackHand(int count)
    {
        attackHand.AddRange(attackDeck.DrawCards(count));
    }

    // Comprar cartas para a mão de defesa
    public void DrawToDefenseHand(int count)
    {
        defenseHand.AddRange(defenseDeck.DrawCards(count));
    }

    // Comprar uma carta para a mão de ataque
    public Card DrawOneToAttackHand()
    {
        Card card = attackDeck.DrawOneCard();
        if (card != null) attackHand.Add(card);
        return card;
    }

    // Comprar uma carta para a mão de defesa
    public Card DrawOneToDefenseHand()
    {
        Card card = defenseDeck.DrawOneCard();
        if (card != null) defenseHand.Add(card);
        return card;
    }

    // Descartar cartas da mão de ataque
    public void DiscardFromAttackHand(List<Card> cardsToDiscard)
    {
        foreach (Card card in cardsToDiscard)
        {
            attackHand.Remove(card);
        }
        attackDeck.Discard(cardsToDiscard);
    }

    // Descartar cartas da mão de defesa
    public void DiscardFromDefenseHand(List<Card> cardsToDiscard)
    {
        foreach (Card card in cardsToDiscard)
        {
            defenseHand.Remove(card);
        }
        defenseDeck.Discard(cardsToDiscard);
    }

    // Exemplo para embaralhar o deck de ataque
    public void ShuffleAttackDeck()
    {
        attackDeck.Shuffle();
    }

    // Exemplo para embaralhar o deck de defesa
    public void ShuffleDefenseDeck()
    {
        defenseDeck.Shuffle();
    }

    // Acessar o estado atual das mãos
    public List<Card> GetAttackHand() => new List<Card>(attackHand);
    public List<Card> GetDefenseHand() => new List<Card>(defenseHand);

    /* public void PlayCard(Card selectedCard, Battler target)
    {
        playerDeck.UseCard(selectedCard, target);
    } */

    public int GetMaxHandSize()
    {
        return maxHandSize;
    }

    public void InitializeDecks()
    {
        Dictionary<string, (Card card, int count)> Cards = new Dictionary<string, (Card card, int count)>();
        // Gerar cartas para o deck
        int attackCardCount = 0;
        int defenseCardCount = 0;

        while (attackCardCount < minimumDeckSize / 2 || defenseCardCount < minimumDeckSize / 2)
        {
            if (attackCardCount < minimumDeckSize / 2)
            {
                var card = cardPool.GetRandomAttackCard();
                if (Cards.ContainsKey(card.cardName))
                {
                    var existingCard = Cards[card.cardName];
                    Cards[card.cardName] = (existingCard.card, existingCard.count + 1);
                }
                else
                {
                    Cards.Add(card.cardName, (card, 1));
                }
                attackCardCount++;
            }

            if (defenseCardCount < minimumDeckSize / 2)
            {
                var card = cardPool.GetRandomDefenseCard();
                if (Cards.ContainsKey(card.cardName))
                {
                    var existingCard = Cards[card.cardName];
                    Cards[card.cardName] = (existingCard.card, existingCard.count + 1);
                }
                else
                {
                    Cards.Add(card.cardName, (card, 1));
                }
                defenseCardCount++;
            }
        }

        // Criar os decks a partir do dicionário
        List<Card> attackCards = Cards.Values
            .Where(entry => entry.card.cardType == Card.CardType.Attack || entry.card.cardType == Card.CardType.Buff || entry.card.cardType == Card.CardType.Debuff)
            .SelectMany(entry => Enumerable.Repeat(entry.card, entry.count))
            .ToList();

        List<Card> defenseCards = Cards.Values
            .Where(entry => entry.card.cardType == Card.CardType.Defense || entry.card.cardType == Card.CardType.Buff || entry.card.cardType == Card.CardType.Debuff)
            .SelectMany(entry => Enumerable.Repeat(entry.card, entry.count))
            .ToList();


        // Inicializar os decks
        attackDeck = new Deck(attackCards);
        defenseDeck = new Deck(defenseCards);

        // Inicializar as mãos
        DrawToAttackHand(4);
        DrawToDefenseHand(4);
    }
}

