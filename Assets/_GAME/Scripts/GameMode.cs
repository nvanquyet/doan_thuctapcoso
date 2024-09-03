using Unity.Netcode;
using UnityEngine;
namespace ShootingGame
{
    

    public enum GamePlayMode
    {
        Survival,
        Co_op,
        Pvp
    }
    public class GameMode : MonoBehaviour
    {
        public enum GamePlayMode { Offline, Online }
        private GamePlayMode currentMode;

        private void Start()
        {
            // Default to offline mode
            SetGameMode(GamePlayMode.Offline);
        }

        // Method to set the game mode
        public void SetGameMode(GamePlayMode mode)
        {
            currentMode = mode;

            switch (currentMode)
            {
                case GamePlayMode.Online:
                    StartOnlineMode();
                    break;
                case GamePlayMode.Offline:
                    StartOfflineMode();
                    break;
            }
        }

        private void StartOnlineMode()
        {
            // Initialize NetworkManager for online mode
            if (!NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.StartClient(); // or StartHost() / StartServer()
                Debug.Log("Started Online Mode");
            }
        }

        private void StartOfflineMode()
        {
            // Stop the network if it's running
            if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.Shutdown();
                Debug.Log("Stopped Network. Entering Offline Mode");
            }

            // Initialize game in offline mode
            InitializeOfflineGame();
        }

        private void InitializeOfflineGame()
        {
            // Add your offline game initialization logic here
            Debug.Log("Initialized Offline Mode");
        }

        // Example to switch between modes at runtime
        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.O)) // Press 'O' to switch to Online Mode
        //     {
        //         SetGameMode(GameMode.Online);
        //     }
        //     if (Input.GetKeyDown(KeyCode.F)) // Press 'F' to switch to Offline Mode
        //     {
        //         SetGameMode(GameMode.Offline);
        //     }
        // }
    }
}

