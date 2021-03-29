using UnityEngine;


namespace SzymonPeszek.Misc
{
    /// <summary>
    /// Class for storing coroutines WaitForSecond enumerators for less cache garbage creation
    /// </summary>
    public static class CoroutineYielder
    {
        public static WaitForFixedUpdate fixedUpdateWaiter { get; } = new WaitForFixedUpdate();
        public static WaitForEndOfFrame endOffFrameWaiter { get; } = new WaitForEndOfFrame();

        public static WaitForSeconds waitFor01Second { get; } = new WaitForSeconds(0.1f);
        public static WaitForSeconds waitFor02Second { get; } = new WaitForSeconds(0.2f);
        public static WaitForSeconds waitFor05Second { get; } = new WaitForSeconds(0.5f);
        public static WaitForSeconds waitFor1Second { get; } = new WaitForSeconds(1f);
        public static WaitForSeconds waitFor1HalfSecond { get; } = new WaitForSeconds(1.5f);
        public static WaitForSeconds waitFor2Seconds { get; } = new WaitForSeconds(2f);
        public static WaitForSeconds waitFor2HalfSeconds { get; } = new WaitForSeconds(2.5f);
        public static WaitForSeconds waitFor3Seconds { get; } = new WaitForSeconds(3f);
        public static WaitForSeconds waitFor3HalfSeconds { get; } = new WaitForSeconds(3.5f);
        public static WaitForSeconds waitFor4Second { get; } = new WaitForSeconds(4f);
        public static WaitForSeconds waitFor4HalfSecond { get; } = new WaitForSeconds(4.5f);
        public static WaitForSeconds waitFor5Second { get; } = new WaitForSeconds(5f);
        public static WaitForSeconds waitFor20Second { get; } = new WaitForSeconds(20f);
        public static WaitForSeconds waitFor40Second { get; } = new WaitForSeconds(40f);
        public static WaitForSeconds waitFor60Second { get; } = new WaitForSeconds(60f);
    }
}