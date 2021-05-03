
namespace SzymonPeszek.Npc.DialogueSystem.Runtime
{
    [System.Serializable]
    public class ExposedProperty
    {
        public static ExposedProperty CreateInstance()
        {
            return new ExposedProperty();
        }

        public string propertyName = "New String";
        public string propertyValue = "New Value";
    }
}