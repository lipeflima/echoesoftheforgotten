using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class CardFeedbackManager : FeedbackManager
{
    public UnityEvent OnCardActivation;
    public static CardFeedbackManager instance;
    public string currentCardName;

    public void Awake()
    {
        instance = this;
    }

    public void SetCardBeforeInvoke(string name)
    {
        currentCardName = name;
        BattleEffectsTest.instance.SetCardName(currentCardName);
    }
}
