namespace MainBankOfRussia.XmlModels
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class ValCursValute
    {
        private ushort numCodeField;

        private string charCodeField;

        private uint nominalField;

        private string nameField;

        private string valueField;

        private string idField;

        /// <remarks/>
        public ushort NumCode
        {
            get
            {
                return this.numCodeField;
            }
            set
            {
                this.numCodeField = value;
            }
        }

        /// <remarks/>
        public string CharCode
        {
            get
            {
                return this.charCodeField;
            }
            set
            {
                this.charCodeField = value;
            }
        }

        /// <remarks/>
        public uint Nominal
        {
            get
            {
                return this.nominalField;
            }
            set
            {
                this.nominalField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }
}
