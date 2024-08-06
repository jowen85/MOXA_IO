using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Diagnostics;

namespace ConfigManager
{
    public abstract class BaseCFG : ConfigurationSection
    {
        #region Variables
        protected static Dictionary<string, BaseCFG> m_dictInstance = new Dictionary<string, BaseCFG>();
        protected static Dictionary<string, Configuration> m_dictCFG = new Dictionary<string, Configuration>();
        #endregion Variables

        #region Methods
        protected static void Save(Dictionary<string, Configuration> cfgTbl, string sectionName)
        {
            Debug.Assert(cfgTbl != null);
            if (cfgTbl == null)
            {
                return;
            }
            Dictionary<string, Configuration>.ValueCollection configObjList = cfgTbl.Values;
            foreach (Configuration configObj in configObjList)
            {
                configObj.Save(ConfigurationSaveMode.Full);
            }
            ConfigurationManager.RefreshSection(sectionName);
        }

        protected static void Save(string path, Dictionary<string, Configuration> cfgTbl, string sectionName)
        {
            // Write a single file.
            Debug.Assert(cfgTbl != null);
            if (cfgTbl == null)
            {
                return;
            }
            if (cfgTbl.Keys.Contains(path))
            {
                cfgTbl[path].Save(ConfigurationSaveMode.Full);
            }
            ConfigurationManager.RefreshSection(sectionName);
        }

        protected static void Save(BaseCFG cfgObj, Dictionary<string, Configuration> cfgTbl, string sectionName)
        {
            // Write a single file.
            Debug.Assert(cfgTbl != null);
            if (cfgTbl == null)
            {
                return;
            }
            // Find the path from the config object instance.
            string path = m_dictInstance.SingleOrDefault(e => e.Value == cfgObj).Key;
            Debug.Assert(!string.IsNullOrEmpty(path), "Cannot find path from the given config object instance.");
            if (!string.IsNullOrEmpty(path))
            {
                cfgTbl[path].Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection(sectionName);
            }
        }

        protected static void SaveAs(string path, Dictionary<string, Configuration> cfgTbl, string newPath, string sectionName)
        {
            Debug.Assert(cfgTbl != null);
            if (cfgTbl == null)
            {
                return;
            }
            if (cfgTbl.Keys.Contains(path))
            {
                cfgTbl[path].SaveAs(newPath);
            }
            ConfigurationManager.RefreshSection(sectionName);
        }

        protected static BaseCFG Open(string className, string path, string section, out Configuration configObj)
        {
            BaseCFG instance = null;
            Configuration config = null;
            ConfigurationFileMap map = new ConfigurationFileMap(path);
            config = ConfigurationManager.OpenMappedMachineConfiguration(map);
            if (config.Sections[section] == null)
            {
                MapObject<BaseCFG> cfgFactory = new MapObject<BaseCFG>();
                instance = cfgFactory.CreateObject(className);
                config.Sections.Add(section, instance);
                // This will save only the section, not the properties.
                config.Save();
                ConfigurationManager.RefreshSection(section);
            }
            else
            {
                instance = config.Sections[section] as BaseCFG;
            }
            // Child class will use this config object to perform Save operation.
            configObj = config;
            return instance;
        }

        #endregion Methods
    }
}
