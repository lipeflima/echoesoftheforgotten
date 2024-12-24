using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCard : MonoBehaviour
{
    
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float targetRotation = 360f;
    private float totalRotation = 0f;

    public void Rotate(){
        StartCoroutine(RotateIndicator());
    }

    public IEnumerator RotateIndicator()
    {
        

        while (totalRotation < targetRotation)
        {
            float rotationThisFrame = rotationSpeed * Time.deltaTime;

            if (totalRotation + rotationThisFrame > targetRotation)
            {
                rotationThisFrame = targetRotation - totalRotation;
            }

            transform.Rotate(0f, rotationThisFrame, 0f);
            totalRotation += rotationThisFrame;

            yield return null;
        }

        Vector3 finalRotation = transform.rotation.eulerAngles;
        finalRotation.y = Mathf.Round(finalRotation.y);
        transform.rotation = Quaternion.Euler(finalRotation);
    }
}
