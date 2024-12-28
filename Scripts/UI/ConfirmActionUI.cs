using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmActionUI : MonoBehaviour
{
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private CardUI cardUI;
    private Action onComplete, onCancel;
    public void Initialize(Action onCompleteCallback, Action onCancelCallback)
    {
        gameObject.SetActive(true);
        onComplete = onCompleteCallback;
        onCancel = onCancelCallback;

        confirmButton.interactable = true; 
        confirmButton.onClick.RemoveListener(CompleteStep);
        confirmButton.onClick.AddListener(CompleteStep);

        cancelButton.interactable = true; 
        cancelButton.onClick.RemoveListener(CancelStep);
        cancelButton.onClick.AddListener(CancelStep);
    }

    private void CompleteStep()
    {
        cardUI.UpdateSpentEnergyCounter();
        gameObject.SetActive(false);
        onComplete?.Invoke();
    }

    private void CancelStep()
    {
        gameObject.SetActive(false);
        onCancel?.Invoke();
    }
}
