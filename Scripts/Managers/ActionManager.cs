using System.Collections;
using UnityEngine;

public abstract class ActionManager : MonoBehaviour
{
    protected Battler currentBattler;
    protected Coroutine actionCoroutine;
    
    public enum AttackStrategy { Basic, Fake, CardAttack }
    
    public enum DefenseStrategy { Evade, CounterAttack, CardDefense, Basic }

    public void StartAction(Battler battler)
    {
        currentBattler = battler;
        actionCoroutine = StartCoroutine(ActionFlow());
    }

    protected abstract IEnumerator ActionFlow();
}
