using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public TMP_Text cardNameText;
    public TMP_Text cardDescriptionText;
    public TMP_Text cardCostText;
    public Image cardArtworkImage;
    public Image highlight;

    private Card card; // A carta lógica associada a este display

    // Configura os elementos visuais com base na carta lógica
    public void Setup(Card newCard)
    {
        card = newCard;
        cardNameText.text = card.cardName;
        cardDescriptionText.text = card.description;
        cardCostText.text = card.EnergyCost.ToString();
        cardArtworkImage.sprite = card.artwork;
        highlight.gameObject.SetActive(false);
    }

    // Permite obter a carta lógica associada (útil para seleções)
    public Card GetCard()
    {
        return card;
    }

    public void highlightCard(bool activate)
    {
        highlight.gameObject.SetActive(activate);
    }
}
