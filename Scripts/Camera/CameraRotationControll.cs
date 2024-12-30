using UnityEngine;
using Cinemachine;

public class CameraRotationControl : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachinePOV povComponent;
    private bool LeftMouseInput;
    void Start()
    {
        // Obt�m a refer�ncia � Cinemachine Virtual Camera e ao componente Aim POV
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            povComponent = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        }

        if (povComponent == null)
        {
            Debug.LogError("CinemachinePOV n�o encontrado na Virtual Camera.");
        }
    }

    void Update()
    {
        LeftMouseInput = Input.GetMouseButton(0);

        if (povComponent == null)
            return;

        // Verifica se o bot�o direito do mouse est� pressionado
        if (LeftMouseInput)
        {
            // Define a velocidade do eixo horizontal como 2
            povComponent.m_HorizontalAxis.m_MaxSpeed = 300f;
            povComponent.m_VerticalAxis.m_MaxSpeed = 300f;

            // Atualiza apenas o eixo horizontal do POV com base no movimento do mouse
            //float horizontalInput = Input.GetAxis("Mouse X");
            //povComponent.m_HorizontalAxis.Value += horizontalInput * povComponent.m_HorizontalAxis.m_MaxSpeed * Time.deltaTime;
        }
        else
        {
            // Define a velocidade do eixo horizontal como 0
            povComponent.m_HorizontalAxis.m_MaxSpeed = 0f;
            povComponent.m_VerticalAxis.m_MaxSpeed = 0f;
        }
    }
}
