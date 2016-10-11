using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using HydrantWiki.Daos;
using HydrantWiki.Interfaces;
using HydrantWiki.Objects;
using LiteDB;

namespace HydrantWiki.Managers
{
    public class HWManager
    {
        private static HWManager m_Manager;

        private LiteDatabase m_Database;
        private string m_DataFolder;
        private string m_DatabaseFile;

        private IPlatformManager m_PlatformManager;
        private ApiManager m_ApiManager;
        private SettingManager m_SettingManager;

        private HWManager()
        {
            m_ApiManager = new ApiManager(this);
            m_SettingManager = new SettingManager(this);

            m_DataFolder = HydrantWikiApp.DataFolder;
            m_DatabaseFile = Path.Combine(m_DataFolder, "HWMobile.db");
            m_Database = new LiteDatabase(m_DatabaseFile);
        }


        public static HWManager GetInstance()
        {
            if (m_Manager == null)
            {
                m_Manager = new HWManager();
            }

            return m_Manager;
        }

        public LiteDatabase Database
        {
            get
            {
                return m_Database;
            }
        }

        public IPlatformManager PlatformManager
        {
            get
            {
                return m_PlatformManager;
            }

            internal set
            {
                m_PlatformManager = value;
            }
        }

        public ApiManager ApiManager
        {
            get
            {
                return m_ApiManager;
            }
        }

        public SettingManager SettingManager
        {
            get
            {
                return m_SettingManager;
            }
        }


        public bool AreUserCredentialsCached()
        {
            string username = m_SettingManager.GetUsername();
            string authToken = m_SettingManager.GetAuthToken();

            if (string.IsNullOrWhiteSpace(username)
                || string.IsNullOrWhiteSpace(authToken))
            {
                return false;
            }

            return true;
        }

        public bool Login(string username, string password)
        {
            User user = m_ApiManager.Authenticate(username, password);

            if (user != null)
            {
                m_SettingManager.SetUser(user);
                HydrantWikiApp.User = user;

                return true;
            } else {
                return false;
            }
        }

        #region Tags
        public void Persist(Tag _tag)
        {
            TagDao dao = new TagDao(m_Database);
            dao.Persist(_tag);
        }

        public List<Tag> GetRecentTags()
        {
            TagDao dao = new TagDao(m_Database);
            return dao.GetRecent();
        }

        public List<Tag> GetTagsNotSentToServer()
        {
            TagDao dao = new TagDao(m_Database);
            return dao.GetNotSentToServer();
        }

        #endregion

    }
}
