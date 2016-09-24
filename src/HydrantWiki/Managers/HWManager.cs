using System;
using HydrantWiki.Interfaces;

namespace HydrantWiki.Managers
{
    public class HWManager
    {
        private static HWManager m_Manager;

        private IPlatformManager m_PlatformManager;
        private ApiManager m_ApiManager;

        private HWManager()
        {
            m_ApiManager = new ApiManager(this);
        }

        public static HWManager GetInstance()
        {
            if (m_Manager == null)
            {
                m_Manager = new HWManager();
            }

            return m_Manager;
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


    }
}
