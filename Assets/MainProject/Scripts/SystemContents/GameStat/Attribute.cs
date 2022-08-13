
namespace BlackTree
{
    [System.Serializable]
    public class Attribute
    {
        public Attributes type;
        public Modifiableint value;
        public void SetParent(PlayerStats _parent)
        {
            value = new Modifiableint(AttributeModified);
        }
        public void AttributeModified()
        {

        }
    }
}
