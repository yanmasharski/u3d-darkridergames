namespace DRG.Speech.Recognition.SRGS
{
    using System.Xml;
    using System.Xml.Serialization;
    using System.Collections.Generic;

    [XmlRoot("grammar")]
    public class Grammar
    {
        [XmlAttribute("root")]
        public string Root;

        [XmlAttribute("xml:lang")]
        public string XMLLang;

        [XmlElement("rule")]
        public List<Rule> Rules { get; set; }

        public Grammar()
        {
            Rules = new List<Rule>();
        }

        public void Fix()
        {
            if (Rules != null)
            {
                for (int i = 0; i < Rules.Count; i++)
                {
                    Rules[i].Fix();
                }

                if (Rules.Count == 0)
                {
                    Rules = null;
                }
            }
        }
    }
}
