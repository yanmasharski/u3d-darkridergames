namespace DRG.Speech.Recognition.SRGS
{
    using System.Xml;
    using System.Xml.Serialization;

    public class RuleRef : Item
    {
        [XmlAttribute("uri")]
        public string Id;
    }
}