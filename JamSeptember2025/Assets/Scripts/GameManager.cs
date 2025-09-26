using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIPlayer[] uiPlayerArray;



    public class PlayerStatus
    {
        public GameObject playerGO;
        public Stats playerStats;
        public Movement playerMovement;
        public Health playerHealth;
        public ItemHandler playerItemHandler;

        public int PlayerIndex {  get; private set; }

        public int PlayerLives { get; set; }
        int playerSprite = 0;

        public PlayerStatus(GameObject newPlayerGO, int lives, int newPlayerIndex ,int playerSpriteIndex = 0)
        {
            playerGO = newPlayerGO;
            PlayerLives = lives;
            playerSprite = playerSpriteIndex;
            playerStats = playerGO.GetComponent<Stats>();
            playerMovement = playerGO.GetComponent<Movement>();
            playerHealth = playerGO.GetComponent<Health>();
            playerItemHandler = playerGO.GetComponent<ItemHandler>();

            PlayerIndex = newPlayerIndex;

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
            // Resets all stats on player - aka set all timers to 0 so no mods effect player 
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

    public int _maxLives = 3;


    public static GameManager Instance { get; private set; } // Singelton

    [SerializeField] public ControlCam _controlCam; // Camera controller, keeping players in view

    List<PlayerStatus> _playerList;

    //public Transform[] _spawnPoints;
    public List<Transform> _spawnPoints;
    private int _playerCount = 0;
    private int _lastSpawnIndex = 0;



    void Start()
    {
        // Singelton
        if (Instance == null)
        {
            Instance = this;

            UIPlayerCount(_playerCount); // Update UI overlay

            SearchForLevelSpawnPoints();
        }
        else
        {
            Destroy(this);
        }
    }

    private void SearchForLevelSpawnPoints()
    {
        // Search for and update level spawn points
        SpawnPointsPlayer levelSpawnPoints = FindAnyObjectByType<SpawnPointsPlayer>();
        if (levelSpawnPoints == null) 
        { 
            Debug.LogWarning("Missing script SpawnPointsPlayer on object in scene! - Used for getting children of script as spawn points."); 
            return;
        }
        _spawnPoints = levelSpawnPoints.gameObject.transform.GetComponentsInChildren<Transform>().ToList<Transform>();
        _spawnPoints.RemoveAt(0); // remove first element in list (parent to spawn points)
        //foreach (Transform t in _spawnPoints) { print(t); }
    }

    private void Awake()
    {
        if (_controlCam == null)
        {
            _controlCam = GetComponent<ControlCam>();
        }

        _playerList = new List<PlayerStatus>();
    }

    private void Update()
    {
        if (Keyboard.current[Key.O].wasPressedThisFrame)
        {
            RestartCurrentLevel();
        }


        if (Keyboard.current[Key.P].wasPressedThisFrame)
        {
            _controlCam.CameraShake(0.4f, 0.5f);
        }


        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void RestartCurrentLevel()
    {
        //Debug.Log("Restarting Current Level");
        foreach (PlayerStatus playerStatus in _playerList)
        {
            playerStatus.PlayerResting();
            playerStatus.ResetStats();
            playerStatus.Spawn(_spawnPoints[GetSpawnIndex()]);
            playerStatus.PlayerLives = _maxLives;

            _controlCam.AddTrackingGameObject(playerStatus.playerGO);

            UIUpdatePlayerLives(playerStatus.PlayerIndex);
        }
    }

    public void AddPlayerToList(GameObject newPlayer)
    {
        // Create a new playerStatus and add to the _playerList
        PlayerStatus newPlayerStatus = new PlayerStatus(newPlayer, _maxLives, _playerCount, _playerCount);

        _playerList.Add(newPlayerStatus);

        UIPlayerCount(_playerCount);
        
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

        // check if already in game? Disable inputs in another way ? - Handeled by keeping the game object in scene
        //Debug.Log(playerInput);
        //Debug.Log(playerInput.gameObject);

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
                playerStatus.Spawn(_spawnPoints[GetSpawnIndex()]);

                UIUpdatePlayerLives(playerStatus.PlayerIndex);
                UIUpdatePlayerSprites();
            }
        }

        _playerCount++;

        // Add player to camera
        _controlCam.AddTrackingGameObject(playerInput.gameObject);
    }

    public int GetSpawnIndex()
    {
        // Try to not get the same spawn index
        int spawnIndex = 0;
        for (int i = 0; i < 10; i++)
        {
            spawnIndex = Random.Range(0, _spawnPoints.Count);
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

                UIUpdatePlayerLives(playerStatus.PlayerIndex);

                //Debug.Log("player life left : " + playerStatus.PlayerLives);
                if (playerStatus.PlayerLives > 0)
                {
                    playerStatus.PlayerResting(); // Drops item as well
                    playerStatus.ResetStats();
                    playerStatus.Spawn(_spawnPoints[spawnIndex]); // Spawn also calls player Active
                    
                    _controlCam.AddTrackingGameObject(playerStatus.playerGO);
                    UIUpdatePlayerLives(playerStatus.PlayerIndex);
                }
                else
                {
                    //Debug.Log($"--- Player {playerStatus.playerGO.name} Died! ---");
                    playerStatus.PlayerResting(); // disable player
                    
                    _controlCam.RemoveTrackingGameObject(playerStatus.playerGO);
                    UIplayerDead(playerStatus.PlayerIndex);
                }
            }
        }

    }

    // UI Stuff
    private void UIPlayerCount(int nrOfPlayers)
    {
        //Debug.Log("Updating Player Count UI");
        for (int i = 0; i < 4; i++) 
        {
            //Debug.Log($"Updating Player Count UI {i}");
            if (i <= nrOfPlayers)
            {
                //Debug.Log($"{i} : true");
                uiPlayerArray[i].gameObject.SetActive(true);
            }
            else
            {
                //Debug.Log($"{i} : false");
                uiPlayerArray[i].gameObject.SetActive(false);
            }
        }
    }

    private void UIUpdatePlayerLives(int index)
    {
        //print(index + "  is updating Life  to " + _playerList[index].PlayerLives);
        uiPlayerArray[index].SetAlive();
        uiPlayerArray[index].SetMaxLife(_maxLives);
        uiPlayerArray[index].SetCurrentLife(_playerList[index].PlayerLives);

    }
    private void UIplayerDead(int index)
    {
        uiPlayerArray[index].SetMaxLife(_maxLives);
        uiPlayerArray[index].SetCurrentLife(_playerList[index].PlayerLives);
        uiPlayerArray[index].SetDeath();
    }

    private void UIUpdatePlayerSprites()
    {
        for (int i = 0; i < _playerList.Count; i++)
        {
            Sprite playerSprite = _playerList[i].playerStats.GetCurrentSprite();
            uiPlayerArray[i].SetSprite(playerSprite);
        }
    }
}
