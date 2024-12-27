using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public static CardUI instance;
    private ActionData actionData;
    public Card Card { get; private set; }
    public event Action<CardUI> OnCardClicked;
    [SerializeField] private GameObject cardUIPrefab;
    [SerializeField] private Transform handContainer;
    [SerializeField] private Transform tableContainer;
    private List<GameObject> cardObjects = new List<GameObject>();
    [SerializeField] private List<Card> selectedCards = new List<Card>();
    private GeneralUI generalUI;
    public bool isSelectCardsActive = false;
    private int maxSelectableCards = 2;
    private TurnManager turnManager;
    [SerializeField] private int playerSpentEnergy = 0;

    public void Awake()
    {
        instance = this;
        generalUI = FindObjectOfType<GeneralUI>();
        turnManager = FindObjectOfType<TurnManager>();
    }

    public void InitializeData(ActionData data)
    {
        actionData = data;
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

    public void AddCardToTable(Card card)
    {
        GameObject cardObject = cardObjects.Find(c => c.GetComponent<CardDisplay>().GetCard() == card);
        if (cardObject != null)
        {
            StartCoroutine(MoveToPosition(cardObject, tableContainer));
        }

        // TODO: Spend Mana
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
        gameObject.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(visible);
    }

    public void ToggleCardSelection(Card card)
    {
        if (!isSelectCardsActive) return;

        if (selectedCards.Contains(card))
        {
            selectedCards.Remove(card);
            HighlightCard(card, false);
            playerSpentEnergy += -card.EnergyCost;
            generalUI.SetCurrentSpentEnergyUI(playerSpentEnergy);
        }
        else if (selectedCards.Count < maxSelectableCards && card.EnergyCost <= actionData.PlayerStats.Mana)
        {
            selectedCards.Add(card);
            HighlightCard(card, true);
            playerSpentEnergy += card.EnergyCost;
            generalUI.SetCurrentSpentEnergyUI(playerSpentEnergy);
        }
    }

    public void UpdateSpentEnergyCounter()
    {   
        turnManager.SetCurrentSpentEnergy(playerSpentEnergy);
    }

    public void InitializeGeneralUI()
    {
        generalUI = FindObjectOfType<GeneralUI>();
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

    private IEnumerator MoveToPosition(GameObject obj, Transform target)
    {
        float duration = 0.5f; // Tempo da transição
        Vector3 start = obj.transform.position;
        Vector3 end = target.position;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            obj.transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = end;
        obj.transform.SetParent(target, true); // Torna-se filho após o movimento
    }
}
