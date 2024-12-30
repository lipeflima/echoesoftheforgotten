using UnityEngine;

public class FireFuryBehavior : CardBehavior
{
    public FireFuryBehavior() : base(null) { }
    public FireFuryBehavior(Card cardData) : base(cardData){}

    public override void ExecuteAction(Card cardData, Battler target)
    {
        Debug.Log($"Executando ação! {cardData.cardName} - {target}");
        cardFeedbackManager = CardFeedbackManager.instance;
        cardFeedbackManager.SetCardBeforeInvoke(cardData.cardName);
        cardFeedbackManager.OnCardActivation?.Invoke();

        foreach (var effect in cardData.effects)
        {
            target.ApplyEffect(effect);
        }
    }
}