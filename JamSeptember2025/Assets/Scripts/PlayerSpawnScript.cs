using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnScript : MonoBehaviour
{
    public Transform[] spawnPoints;
    private int _playerCount;

    [SerializeField] private ControlCam controlCam; // Camera controller, keeping players in view

    private void Awake()
    {
        if (controlCam == null )
        {
            controlCam = GameObject.FindFirstObjectByType<ControlCam>();
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.transform.position = spawnPoints[_playerCount].transform.position;
        if(_playerCount == 0)
        {
            // First player behavior
        }


        playerInput.GetComponent<Stats>().SetSprite(_playerCount);

        _playerCount++;

        // Add player to camera
        controlCam.AddTrackingGameObject(playerInput.gameObject);
    }
}
