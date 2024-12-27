using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionPanel : MonoBehaviour {
    [Header("Painel de Ações")]
    public GameObject panel;
    
    [Header("Player Action Manager")]     
    public PlayerActionManager playerActionManager;

    [Header("Prefabs de Botões")]
    public GameObject pickCardUI; 
    public GameObject shuffleCardUI;
    public GameObject discardCardUI;
    public GameObject regenerateHandUI;
    public GameObject selectCardUI;
    public GameObject selectTargetUI;
    public GameObject resolveTurnUI;

    private void Start() 
    {
        
    }
    
    public void OnPickCardAction()
    {
        DisplayActionButton(pickCardUI, () => {
            pickCardUI.SetActive(false);
            // playerActionManager.PickCardAction();
            
        });
    }

    public void OnShuffleAction()
    {
        DisplayActionButton(shuffleCardUI, () => {
            discardCardUI.SetActive(false);
            shuffleCardUI.SetActive(false);
            regenerateHandUI.SetActive(false);
            // playerActionManager.ShuffleAction();
            
        });
    }

    public void OnDiscardAction()
    {
        DisplayActionButton(discardCardUI, () => {
            discardCardUI.SetActive(false);
            shuffleCardUI.SetActive(false);
            regenerateHandUI.SetActive(false);
            // playerActionManager.DiscardAction();
            
        });
    }

    public void OnRegenerateHandAction()
    {
        DisplayActionButton(regenerateHandUI, () => {
            discardCardUI.SetActive(false);
            shuffleCardUI.SetActive(false);
            regenerateHandUI.SetActive(false);
            // playerActionManager.RegenerateHandAction();
            
        });
    }

    public void OnSelectCardAction()
    {
        DisplayActionButton(selectCardUI, () => {
            selectCardUI.SetActive(false);
            // playerActionManager.SelectCardAction();
            
        });
    }

    public void OnSelectTargetAction()
    {
        DisplayActionButton(selectTargetUI, () => {
            selectTargetUI.SetActive(false);
            // playerActionManager.SelectTargetAction();
            
        });
    }

    public void OnResolveTurnAction()
    {
        DisplayActionButton(resolveTurnUI, () => {
            resolveTurnUI.SetActive(false);
            // playerActionManager.ResolveTurnAction();
            
        });
    }

    private void DisplayActionButton(GameObject actionButton, System.Action onClickAction)
    {
        actionButton.SetActive(true);

        var buttonComponent = actionButton.GetComponent<Button>();

        if (buttonComponent != null)
        {
            buttonComponent.onClick.AddListener(() => onClickAction());
        }
    }

    public void Show() {
        panel.SetActive(true);
    }
    public void Hide() {
        panel.SetActive(false);
    }
}
