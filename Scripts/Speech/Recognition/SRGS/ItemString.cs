namespace DRG.Speech.Recognition.SRGS
{
    using System.Xml;
    using System.Xml.Serialization;


    public class ItemString : Item
    {
        [XmlText]
        public string String;
    }
}