namespace DRG.Speech.Recognition.SRGS
{
    using System.Xml;
    using System.Xml.Serialization;
    using System.Collections.Generic;

    public class Rule
    {
        [XmlAttribute("id")]
        public string Id;

        [XmlAttribute("scope")]
        public string Scope;

        [XmlArray("one-of")]
        [XmlArrayItem(typeof(ItemString), ElementName = "item")]
        [XmlArrayItem(typeof(ItemRuleRef), ElementName = "item")]
        public List<Item> OneOfList { get; set; }

        [XmlArray("item")]
        [XmlArrayItem(typeof(ItemString), ElementName = "item")]
        [XmlArrayItem(typeof(RuleRef), ElementName = "ruleref")]
        public List<Item> Items { get; set; }

        public void Fix()
        {
            if (OneOfList != null && OneOfList.Count == 0)
            {
                OneOfList = null;
            }

            if (Items != null && Items.Count == 0)
            {
                Items = null;
            }
        }
    }
}