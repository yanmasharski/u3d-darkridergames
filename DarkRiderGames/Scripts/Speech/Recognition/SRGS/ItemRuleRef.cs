namespace DRG.Speech.Recognition.SRGS
{
    using System.Xml;
    using System.Xml.Serialization;

    public class ItemRuleRef : Item
    {
        [XmlElement("ruleref")]
        public RuleRef RuleRef;
    }
}