using System;
using System.Collections.Generic;
using HydrantWiki.Constants;
using HydrantWiki.Daos;
using HydrantWiki.Objects;

namespace HydrantWiki.Managers
{
    public class SettingManager : IDisposable
    {
        public HWManager m_Manager;

        public SettingManager(HWManager _manager)
        {
            m_Manager = _manager;
        }

        private List<Setting> GetSettings()
        {
            SettingDao dao = new SettingDao(m_Manager.Database);
            return dao.GetAll();
        }

        private void Persist(Setting _setting)
        {
            SettingDao dao = new SettingDao(m_Manager.Database);
            dao.Persist(_setting);
        }

        private Setting GetSetting(string _name)
        {
            SettingDao dao = new SettingDao(m_Manager.Database);
            return dao.GetSetting(_name);
        }

        public string GetSettingValue(string _name)
        {
            SettingDao dao = new SettingDao(m_Manager.Database);
            Setting setting = dao.GetSetting(_name);

            if (setting != null)
            {
                return setting.Value;
            }

            return null;
        }


        public void SetSetting(string _name, string _value)
        {
            SettingDao dao = new SettingDao(m_Manager.Database);
            Setting setting = dao.GetSetting(_name);

            if (setting == null)
            {
                setting = new Setting();
                setting.Name = _name;
            }

            setting.Value = _value;
            dao.Persist(setting);
        }

        public void SetSetting(string _name, Guid _value)
        {
            SetSetting(_name, _value.ToString());
        }

        public void SetSetting(string _name, bool _value)
        {
            SetSetting(_name, _value.ToString());
        }

        public void SetSetting(string _name, int _value)
        {
            SetSetting(_name, _value.ToString());
        }

        public bool GetBoolSetting(string _name)
        {
            Setting setting = GetSetting(_name);

            if (setting == null) return false;

            bool output;
            if (bool.TryParse(setting.Value, out output))
            {
                return output;
            }

            return false;
        }

        public Guid? GetGuidSetting(string _name)
        {
            Setting setting = GetSetting(_name);

            if (setting == null) return null;

            Guid output;
            if (Guid.TryParse(setting.Value, out output))
            {
                return output;
            }

            return null;
        }

        public int GetIntSetting(string _name)
        {
            Setting setting = GetSetting(_name);

            if (setting == null) return 0;

            int output;
            if (int.TryParse(setting.Value, out output))
            {
                return output;
            }

            return 0;
        }

        public Guid GetInstallId()
        {
            try
            {
                Guid? id = GetGuidSetting(SettingConstants.InstallId);

                if (id == null)
                {
                    id = Guid.NewGuid();

                    SetSetting(SettingConstants.InstallId, id.Value);
                }

                return id.Value;
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public string GetUsername()
        {
            return GetSettingValue(SettingConstants.Username);
        }

        public void SetUsername(string username)
        {
            SetSetting(SettingConstants.Username, username);
        }

        public string GetDisplayName()
        {
            return GetSettingValue(SettingConstants.DisplayName);
        }

        public void SetDisplayName(string displayName)
        {
            SetSetting(SettingConstants.DisplayName, displayName);
        }

        public string GetAuthToken()
        {
            return GetSettingValue(SettingConstants.Token);
        }

        public void SetAuthToken(string token)
        {
            SetSetting(SettingConstants.Token, token);
        }

        public string GetUserType()
        {
            return GetSettingValue(SettingConstants.UserType);
        }

        public void SetUserType(string userType)
        {
            SetSetting(SettingConstants.UserType, userType);
        }

        public void Dispose()
        {
            m_Manager = null;
        }

        public void SetTagCount(int _count)
        {
            SetSetting(SettingConstants.TagCount, _count);
        }

        public int GetTagCount()
        {
            return GetIntSetting(SettingConstants.TagCount);
        }

        public User GetUser()
        {
            string username = GetUsername();
            string authtoken = GetAuthToken();
            string displayname = GetDisplayName();
            string userType = GetUserType();

            if (username != null
                && authtoken != null)
            {
                User user = new User
                {
                    Username = username,
                    AuthorizationToken = authtoken,
                    DisplayName = displayname,
                    UserType = userType
                };

                return user;
            }

            return null;
        }

        public void ClearUser()
        {
            SetUsername(null);
            SetAuthToken(null);
            SetUserType(null);
            SetDisplayName(null);
        }

        public void SetUser(User user)
        {
            if (user != null)
            {
                SetUsername(user.Username);
                SetAuthToken(user.AuthorizationToken);
                SetDisplayName(user.DisplayName);
                SetUserType(user.UserType);
            }
        }
    }
}

