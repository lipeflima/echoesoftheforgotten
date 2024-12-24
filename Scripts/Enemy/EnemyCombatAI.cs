using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatIA : MonoBehaviour
{
    // Decide qual carta o inimigo usará
    public Card DecideCard(List<Card> hand)
    {
        // Implementar a lógica para escolher uma carta
        // Exemplo: Retornar a primeira carta disponível
        if (hand.Count > 0)
        {
            return hand[0];
        }

        return null;
    }

    // Decide qual alvo será atacado
    public GameObject DecideTarget(Card selectedCard)
    {
        // Implementar a lógica para escolher o alvo
        // Exemplo: Retornar um alvo fictício
        GameObject dummyTarget = GameObject.Find("Player"); // Exemplo de busca
        return dummyTarget;
    }
}
