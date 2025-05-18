using System;
using System.Diagnostics;
using System.Reflection;
using EmberAPI;

namespace scraper.Data.TVDBv4
{
    public class TVDBv4_Data : Interfaces.ScraperModule_Data_TV
    {
        public string ModuleName => "TVDBv4_Data";

        public string ModuleVersion => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        public bool ScraperEnabled
        {
            get
            {
                return true;
            }
            set
            {
                // Do nothing
            }
        }

        public event Interfaces.ScraperModule_Data_TV.ModuleSettingsChangedEventHandler ModuleSettingsChanged;
        public event Interfaces.ScraperModule_Data_TV.ScraperEventEventHandler ScraperEvent;
        public event Interfaces.ScraperModule_Data_TV.ScraperSetupChangedEventHandler ScraperSetupChanged;
        public event Interfaces.ScraperModule_Data_TV.SetupNeedsRestartEventHandler SetupNeedsRestart;

        public void Init(string sAssemblyName)
        {
            throw new NotImplementedException();
        }

        public Containers.SettingsPanel InjectSetupScraper()
        {
            throw new NotImplementedException();
        }

        public void SaveSetupScraper(bool DoDispose)
        {
            throw new NotImplementedException();
        }

        public void ScraperOrderChanged()
        {
            throw new NotImplementedException();
        }

        public Interfaces.ModuleResult_Data_TVEpisode Scraper_TVEpisode(ref Database.DBElement oDBElement, Structures.ScrapeOptions ScrapeOptions)
        {
            throw new NotImplementedException();
        }

        public Interfaces.ModuleResult_Data_TVSeason Scraper_TVSeason(ref Database.DBElement oDBElement, Structures.ScrapeOptions ScrapeOptions)
        {
            throw new NotImplementedException();
        }

        public Interfaces.ModuleResult_Data_TVShow Scraper_TVShow(ref Database.DBElement oDBTV, ref Structures.ScrapeModifiers ScrapeModifiers, ref Enums.ScrapeType ScrapeType, ref Structures.ScrapeOptions ScrapeOptions)
        {
            throw new NotImplementedException();
        }
    }
}
