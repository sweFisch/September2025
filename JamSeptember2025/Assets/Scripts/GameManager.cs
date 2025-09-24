using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //public class PlayerStatus
    //{

    //}


    public static GameManager Instance { get; private set; } // Singelton

    [SerializeField] private ControlCam controlCam; // Camera controller, keeping players in view

    List<GameObject> _playerList;

    public Transform[] spawnPoints;
    private int _playerCount;
    private int _lastSpawnIndex = 0;



    void Start()
    {
        // Singelton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Awake()
    {
        if (controlCam == null)
        {
            controlCam = GameObject.FindFirstObjectByType<ControlCam>();
        }
    }

    public void AddPlayerToList(GameObject newPlayer)
    {
        if (!_playerList.Contains(newPlayer))
        {
            _playerList.Add(newPlayer);
        }
    }


    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.transform.position = spawnPoints[_playerCount].transform.position;
        if (_playerCount == 0)
        {
            // First player behavior
        }

        // Set player Sprite Look - TODO lookup after the sprite choosen
        playerInput.GetComponent<Stats>().SetSprite(_playerCount);

        _playerCount++;

        // Add player to camera
        controlCam.AddTrackingGameObject(playerInput.gameObject);
    }

    public void PlayerDied(GameObject deadPlayer)
    {
        // Try to not get the same spawn index
        int spawnIndex = 0;
        for (int i = 0;  i < 10; i++)
        {
            spawnIndex= Random.Range(0, spawnPoints.Length);
            if (spawnIndex != _lastSpawnIndex) 
            { 
                _lastSpawnIndex = spawnIndex;
                break; 
            }
        }

        deadPlayer.transform.position = spawnPoints[spawnIndex].position;
        deadPlayer.GetComponent<Health>().Heal(1000);
    }


}
