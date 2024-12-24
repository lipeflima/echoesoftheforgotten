using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionManager : MonoBehaviour
{
    [SerializeField] private EnemyCombatIA enemyCombatIA;
    public List<Card> hand = new List<Card>(); // Mão atual do inimigo
    public int maxHandSize = 4; 
    private bool isActionComplete = false;

    private void Start()
    {
        enemyCombatIA = this.GetComponent<EnemyCombatIA>();
        StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        while (true)
        {
            Debug.Log("Início do turno do inimigo");

            // Etapa 1: Pegar cartas até completar a mão
            FillHand();

            // Etapa 2: Escolher uma carta
            Card selectedCard = ChooseCard();
            if (selectedCard == null)
            {
                Debug.Log("O inimigo não tem cartas para jogar.");
                yield break; // Encerrar o turno
            }

            // Etapa 3: Escolher um alvo
            GameObject target = ChooseTarget(selectedCard);

            // Etapa 4: Resolver a ação
            yield return StartCoroutine(ResolveAction(selectedCard, target));

            Debug.Log("Fim do turno do inimigo");

            yield break; // Encerrar a corrotina para o turno do inimigo
        }
    }

    private void FillHand()
    {
       
    }

    private Card ChooseCard()
    {
        // Consultar a IA para decidir qual carta usar
        Card selectedCard = enemyCombatIA.DecideCard(hand);

        if (selectedCard != null)
        {
            Debug.Log($"Inimigo escolheu a carta {selectedCard.cardName}.");
        }

        return selectedCard;
    }

    private GameObject ChooseTarget(Card selectedCard)
    {
        // Consultar a IA para decidir qual alvo escolher
        GameObject target = enemyCombatIA.DecideTarget(selectedCard);

        if (target != null)
        {
            Debug.Log($"Inimigo escolheu o alvo {target.name}.");
        }

        return target;
    }

    private IEnumerator ResolveAction(Card card, GameObject target)
    {
        Debug.Log($"Inimigo está usando a carta {card.cardName} no alvo {target.name}.");
        isActionComplete = false;

        // Aqui deve haver lógica para aplicar o efeito da carta
        yield return new WaitForSeconds(1.5f); // Simulação de tempo para resolver a ação

        isActionComplete = true;
        Debug.Log("Ação do inimigo concluída.");
    }
}
