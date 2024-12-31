using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class CardInteraction : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Card cardData;
    [SerializeField] private bool isClickable = false;

    public void Initialize(Card card)
    {
        cardData = card;
        SetClickable(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isClickable) return;
        if (CardUI.instance != null)
        {
            CardUI.instance.ToggleCardSelection(cardData);
        }
    }

    public void SetClickable(bool clickable)
    {
        isClickable = clickable;
    }
}
