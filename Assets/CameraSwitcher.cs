using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cinemachine Virtual Camera")]
    public Cinemachine.CinemachineVirtualCamera vCam;

    [Header("Players")]
    public Transform player1;
    public Transform player2;

    private void Start()
    {
        if (vCam == null)
            Debug.LogError("Virtual Camera is not assigned!");

        if (player1 == null || player2 == null)
            Debug.LogError("Players are not assigned!");
    }

    private void Update()
    {
        // Switch to Player 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchCameraToPlayer(player1);
        }

        // Switch to Player 2
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchCameraToPlayer(player2);
        }
    }

    private void SwitchCameraToPlayer(Transform player)
    {
        vCam.Follow = player;
        vCam.LookAt = player;
    }
}
