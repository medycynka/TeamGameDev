using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SzymonPeszek.PlayerScripts;
using SzymonPeszek.PlayerScripts.Inventory;

namespace SzymonPeszek.SaveScripts
{
    /// <summary>
    /// Static class used for saving DataManager storage class to binary file
    /// </summary>
    public static class SaveManager
    {
        /// <summary>
        /// Saves data from main menu
        /// </summary>
        public static void SaveMainMenu()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            string path = Application.dataPath + "/Saves/GameSave.lps";
            FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            DataManager dataManager = new DataManager();

            binaryFormatter.Serialize(stream, dataManager);
            stream.Close();
        }

        /// <summary>
        /// Saves data during game
        /// </summary>
        /// <param name="playerManager">Player manager</param>
        /// <param name="playerStats">Player stats</param>
        /// <param name="playerInventory">Player inventory</param>
        public static void SaveGame(PlayerManager playerManager, PlayerStats playerStats, PlayerInventory playerInventory)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            string path = Application.dataPath + "/Saves/GameSave.lps";
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            DataManager dataManager = new DataManager(playerManager, playerStats, playerInventory);

            binaryFormatter.Serialize(stream, dataManager);
            stream.Close();
        }

        /// <summary>
        /// Loads data from save file
        /// </summary>
        /// <returns>DataManager storage class that stores data needs for loading game</returns>
        public static DataManager LoadGame()
        {
            string path = Application.dataPath + "/Saves/GameSave.lps";
            
            if (File.Exists(path))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                DataManager dataManager = binaryFormatter.Deserialize(stream) as DataManager;

                stream.Close();
                
                return dataManager;
            }
            
            
            return null;
        }
    }
}