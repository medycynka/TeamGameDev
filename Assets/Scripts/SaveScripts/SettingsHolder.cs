using SzymonPeszek.Environment.Areas;


namespace SzymonPeszek.SaveScripts
{
    /// <summary>
    /// Static class for storing settings
    /// </summary>
    public static class SettingsHolder
    {
        // Settings
        public static int resolutionID = 0;
        public static bool isFullscreen = true;
        public static int qualityID = 2;
        public static float mouseSensibility = 25;
        public static float soundVolume = 1;

        // Main Menu
        public static bool isCharacterCreated = false;
        public static bool firstStart = true;
        public static string playerName = "";
        public static bool isMale = true;
        public static int[] partsID = { -1, -1, 0, -1, -1, -1, -1, -1, -1, -1, 0, -1, -1, 0, 0, -1, -1, 0, 0, 0, 0, 0, -1, -1, 0, 0 };
        public static float[] partsArmor = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };

        // Stats
        public static int currentLevel = 1;
        public static float currentStrength = 0f;
        public static float currentAgility = 0f;
        public static float currentDefence = 0f;
        public static float currentBonusHealth = 0f;
        public static float currentBonusStamina = 0f;
        public static float currentBonusFocus = 0f;
        
        // Bosses
        public static bool[] bossAreaAlive = { true };

        // Bonfires
        public static bool[] bonfiresActivation = { false, false, false, false, false, false, false, false, false };
        
        // Data Manager
        public static DataManager dataManager;
        
        // World Manager
        public static WorldManager worldManager;
    }
}