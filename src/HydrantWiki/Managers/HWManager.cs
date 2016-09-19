using System;
namespace HydrantWiki.Managers
{
    public class HWManager
    {
        private static HWManager m_Manager;

        private HWManager()
        {
        }

        public static HWManager GetInstance()
        {
            if (m_Manager == null)
            {
                m_Manager = new HWManager();
            }

            return m_Manager;
        }

    }
}
