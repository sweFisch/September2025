using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public class PlayerStatus
    {
        public GameObject playerGO;
        public Stats playerStats;
        public Movement playerMovement;
        public Health playerHealth;
        public ItemHandler playerItemHandler;

        public int PlayerLives { get; set; }
        int playerSprite = 0;

        public PlayerStatus(GameObject newPlayerGO, int lives, int playerSpriteIndex = 0)
        {
            playerGO = newPlayerGO;
            PlayerLives = lives;
            playerSprite = playerSpriteIndex;
            playerStats = playerGO.GetComponent<Stats>();
            playerMovement = playerGO.GetComponent<Movement>();
            playerHealth = playerGO.GetComponent<Health>();
            playerItemHandler = playerGO.GetComponent<ItemHandler>();

            ChangeSprite(playerSprite);
        }

        public void PlayerResting()
        {
            playerStats.SetBloodSprite();
            playerStats.enabled = false;
            playerMovement.enabled = false;
            playerHealth.enabled = false;
            playerItemHandler.DropHeldItems();
            playerItemHandler.enabled = false;
        }

        public void PlayerActive()
        {
            playerStats.enabled = true;
            playerMovement.enabled = true;
            playerHealth.enabled = true;
            playerItemHandler.enabled = true;
            ChangeSprite(playerSprite);
        }

        public void ChangeSprite(int index)
        {
            playerSprite = index;
            playerStats.SetSprite(playerSprite);
        }

        public void ResetStats() // On death
        {
            // Resets all stats on player
            // also health
            playerGO.GetComponent<Health>().Heal(1000);
        }

        public void Spawn(Transform spawnTransform)
        {
            PlayerActive();
            playerGO.transform.position = spawnTransform.position;
        }

        public bool CompareGameObject(GameObject playerToCompare)
        {
            if (playerToCompare == playerGO)
            {
                return true;
            }
            return false;
        }
    }

    public int maxLives = 3;


    public static GameManager Instance { get; private set; } // Singelton

    [SerializeField] private ControlCam controlCam; // Camera controller, keeping players in view

    List<PlayerStatus> _playerList;

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

        _playerList = new List<PlayerStatus>();
    }

    private void Update()
    {
        if (Keyboard.current[Key.O].wasPressedThisFrame)
        {
            Debug.Log("Restarting game");
            foreach (PlayerStatus playerStatus in _playerList)
            {
                playerStatus.PlayerResting();
                playerStatus.ResetStats();
                playerStatus.Spawn(spawnPoints[GetSpawnIndex()]);
                playerStatus.PlayerLives = maxLives;
            }
        }
    }

    public void AddPlayerToList(GameObject newPlayer)
    {
        // Create a new playerStatus and add to the _playerList
        PlayerStatus newPlayerStatus = new PlayerStatus(newPlayer, maxLives,_playerCount);

        _playerList.Add(newPlayerStatus);
    }


    public void OnPlayerJoined(PlayerInput playerInput)
    {
        foreach (PlayerStatus playerStatus in _playerList)
        {
            if (playerStatus.CompareGameObject(playerInput.gameObject))
            {
                return;
            }
        }

        // check if already in game? Disable inputs in another way ?
        Debug.Log(playerInput);
        Debug.Log(playerInput.gameObject);

        if (_playerCount == 0)
        {
            // First player behavior
        }

        // Create a Player Status object in List and use that to Spawn
        AddPlayerToList(playerInput.gameObject);
        foreach (PlayerStatus playerStatus in _playerList)
        {
            if (playerStatus.CompareGameObject(playerInput.gameObject))
            {
                playerStatus.Spawn(spawnPoints[GetSpawnIndex()]);
            }
        }

        _playerCount++;

        // Add player to camera
        controlCam.AddTrackingGameObject(playerInput.gameObject);
    }

    public int GetSpawnIndex()
    {
        // Try to not get the same spawn index
        int spawnIndex = 0;
        for (int i = 0; i < 10; i++)
        {
            spawnIndex = Random.Range(0, spawnPoints.Length);
            if (spawnIndex != _lastSpawnIndex)
            {
                _lastSpawnIndex = spawnIndex;
                return spawnIndex;
            }
        }
        Debug.LogWarning("Returning The Same Spawn Index");
        return spawnIndex;
    }

    public void PlayerDied(GameObject deadPlayer)
    {
        // Try to not get the same spawn index
        int spawnIndex = GetSpawnIndex();

        // Reset player attributes and stats on respawn
        foreach (PlayerStatus playerStatus in _playerList)
        {
            if (playerStatus.CompareGameObject(deadPlayer)) 
            {
                playerStatus.PlayerLives -= 1;
                Debug.Log("player life left : " + playerStatus.PlayerLives);
                if (playerStatus.PlayerLives > 0)
                {
                    playerStatus.PlayerResting(); // Drops item as well
                    playerStatus.ResetStats();
                    playerStatus.Spawn(spawnPoints[spawnIndex]); // Spawn also calls player Active
                }
                else
                {
                    Debug.Log($"--- Player {playerStatus.playerGO.name} Died! ---");
                    //playerStatus.playerGO.SetActive(false); // disable player
                    playerStatus.PlayerResting(); // disable player
                }
            }
        }

    }


}
