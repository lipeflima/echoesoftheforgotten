using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public static CardUI instance;
    public Card Card { get; private set; }
    public event Action<CardUI> OnCardClicked;
    [SerializeField] private GameObject cardUIPrefab;
    [SerializeField] private Transform handContainer;
    private List<GameObject> cardObjects = new List<GameObject>();
    [SerializeField] private List<Card> selectedCards = new List<Card>();
    public bool isSelectCardsActive = false;
    private int maxSelectableCards = 2;

    public void Awake()
    {
        instance = this;
    }

    public void InitializeHand(List<Card> hand)
    {
        foreach (Card card in hand)
        {
            AddCard(card);
        }
    }

    public void ClearHand()
    {
        foreach (GameObject cardObject in cardObjects)
        {
            Destroy(cardObject);
        }
        cardObjects.Clear();
    }

    public void AddCard(Card card)
    {
        Card uniqueCard = Instantiate(card);
        GameObject cardObject = Instantiate(cardUIPrefab, handContainer);
        cardObject.GetComponent<CardDisplay>().Setup(uniqueCard);
        cardObjects.Add(cardObject);
        var interaction = cardObject.GetComponent<CardInteraction>();
        interaction.Initialize(uniqueCard);
        cardObject.GetComponent<RotateCard>().Rotate();
    }

    public void HighlightCard(Card card, bool highlight)
    {
        GameObject cardObject = cardObjects.Find(c => c.GetComponent<CardDisplay>().GetCard() == card);
        if (cardObject != null)
        {
            cardObject.GetComponent<CardDisplay>().highlightCard(highlight);
        }
    }

    public List<GameObject> GetCardObjects()
    {
        return cardObjects;
    }

    public void DisplayCardUI(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public void ToggleCardSelection(Card card)
    {
        if (!isSelectCardsActive) return;

        if (selectedCards.Contains(card))
        {
            selectedCards.Remove(card);
            HighlightCard(card, false);
        }
        else if (selectedCards.Count < maxSelectableCards)
        {
            selectedCards.Add(card);
            HighlightCard(card, true);
        }
    }

    public List<Card> GetSelectedCards()
    {
        return selectedCards;
    }

    public void ClearSelectedCards()
    {
        selectedCards.Clear();
    }

    public void ActivateSelectCard(bool status)
    {
        isSelectCardsActive = status;
    }
}
