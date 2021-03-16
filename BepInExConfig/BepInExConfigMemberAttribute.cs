using System;
using BepInEx.Configuration;

namespace BepInExConfig
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public partial class BepInExConfigMemberAttribute : Attribute 
    {
        public Type FieldType { get; private set; }
        public ConfigDefinition ConfigDefinition { get; private set; }
        public object DefaultValue { get; private set; }
        public ConfigDescription ConfigDescription { get; private set; } = null;
        public int Order { get; private set; } = 0;

        private void InitProperties(string section, string key, Type defaultValueType, object defaultValue, string description, int order = 0)
        {
            ConfigDefinition = new ConfigDefinition(section, key);
            FieldType = defaultValueType;
            DefaultValue = defaultValue;
            ConfigDescription = string.IsNullOrEmpty(description) ? null : new ConfigDescription(description);
            Order = order;
        }

        public BepInExConfigMemberAttribute(string section, string key, string defaultValue, string description = "", int order = 0)
        {
            InitProperties(section, key, defaultValue.GetType(), defaultValue, description, order);
        }

        public BepInExConfigMemberAttribute(string section, string key, bool defaultValue, string description = "", int order = 0)
        {
            InitProperties(section, key, defaultValue.GetType(), defaultValue, description, order);
        }

        public BepInExConfigMemberAttribute(string section, string key, byte defaultValue, string description = "", int order = 0)
        {
            InitProperties(section, key, defaultValue.GetType(), defaultValue, description, order);
        }

        public BepInExConfigMemberAttribute(string section, string key, short defaultValue, string description = "", int order = 0)
        {
            InitProperties(section, key, defaultValue.GetType(), defaultValue, description, order);
        }

        public BepInExConfigMemberAttribute(string section, string key, ushort defaultValue, string description = "", int order = 0)
        {
            InitProperties(section, key, defaultValue.GetType(), defaultValue, description, order);
        }

        public BepInExConfigMemberAttribute(string section, string key, int defaultValue, string description = "", int order = 0)
        {
            InitProperties(section, key, defaultValue.GetType(), defaultValue, description, order);
        }

        public BepInExConfigMemberAttribute(string section, string key, uint defaultValue, string description = "", int order = 0)
        {
            InitProperties(section, key, defaultValue.GetType(), defaultValue, description, order);
        }

        public BepInExConfigMemberAttribute(string section, string key, long defaultValue, string description = "", int order = 0)
        {
            InitProperties(section, key, defaultValue.GetType(), defaultValue, description, order);
        }

        public BepInExConfigMemberAttribute(string section, string key, ulong defaultValue, string description = "", int order = 0)
        {
            InitProperties(section, key, defaultValue.GetType(), defaultValue, description, order);
        }

        public BepInExConfigMemberAttribute(string section, string key, float defaultValue, string description = "", int order = 0)
        {
            InitProperties(section, key, defaultValue.GetType(), defaultValue, description, order);
        }

        public BepInExConfigMemberAttribute(string section, string key, double defaultValue, string description = "", int order = 0)
        {
            InitProperties(section, key, defaultValue.GetType(), defaultValue, description, order);
        }

        public BepInExConfigMemberAttribute(string section, string key, decimal defaultValue, string description = "", int order = 0)
        {
            InitProperties(section, key, defaultValue.GetType(), defaultValue, description, order);
        }

        public BepInExConfigMemberAttribute(string section, string key, object defaultValue, string description = "", int order = 0)
        {
            InitProperties(section, key, defaultValue.GetType(), defaultValue, description, order);
        }
    }
}
