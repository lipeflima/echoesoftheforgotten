using UnityEngine;

public class RootWallBehavior : CardBehavior
{
    public RootWallBehavior() : base(null) {}

    public override void ExecuteAction(Card cardData, Battler target)
    {
        Debug.Log($"Executando ação! {cardData.cardName}");
        cardFeedbackManager = CardFeedbackManager.instance;
        cardFeedbackManager.SetCardBeforeInvoke(cardData.cardName);
        cardFeedbackManager.OnCardActivation?.Invoke();

        foreach (var effect in cardData.effects)
        {
            target.ApplyEffect(effect);
        }
    }
}