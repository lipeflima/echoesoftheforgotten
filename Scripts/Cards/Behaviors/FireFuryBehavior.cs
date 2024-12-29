using UnityEngine;

public class FireFuryBehavior : CardBehavior
{
    public FireFuryBehavior(Card cardData) : base(cardData){}

    public void Start()
    {
        cardFeedbackManager = CardFeedbackManager.instance;
    }

    public override void ExecuteAction(Card cardData, Battler attacker, Battler defender)
    {
        Debug.Log($"Executando ação! {cardData.cardName}");
        cardFeedbackManager.SetCardBeforeInvoke(cardData.cardName);
        cardFeedbackManager.OnCardActivation?.Invoke();

        foreach (var effect in cardData.effects)
        {
            defender.ApplyEffect(effect);
        }
    }
}