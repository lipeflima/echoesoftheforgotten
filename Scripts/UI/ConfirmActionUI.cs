using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmActionUI : MonoBehaviour
{
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    private Action onComplete, onCancel;
    public void Initialize(Action onCompleteCallback, Action onCancelCallback)
    {
        gameObject.SetActive(true);
        onComplete = onCompleteCallback;
        onCancel = onCancelCallback;

        confirmButton.interactable = true; 
        confirmButton.onClick.AddListener(CompleteStep);

        cancelButton.interactable = true; 
        cancelButton.onClick.AddListener(CancelStep);
    }

    private void CompleteStep()
    {
        gameObject.SetActive(false);
        onComplete?.Invoke();
    }

    private void CancelStep()
    {
        gameObject.SetActive(false);
        onCancel?.Invoke();
    }
}
