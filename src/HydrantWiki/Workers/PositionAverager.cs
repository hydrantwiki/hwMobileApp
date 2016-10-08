using System;
using System.Threading.Tasks;
using HydrantWiki.Delegates;
using HydrantWiki.Managers;
using HydrantWiki.Objects;

namespace HydrantWiki.Workers
{
    public class PositionAverager
    {
        private readonly LocationManager m_Location;
        private readonly int m_DesiredPositionCount;
        private PositionAverage m_Average = null;
        private bool m_Cancelled = false;

        public event PositionChangedDelegate PositionUpdated;

        public PositionAverager(LocationManager _location, int count)
        {
            m_Location = _location;
            m_DesiredPositionCount = count;
        }

        public async Task Start()
        {
            m_Cancelled = false;
            m_Average = new PositionAverage();

            for (int i = 0; i < m_DesiredPositionCount; i++)
            {
                GeoPoint point = await m_Location.GetLocation();
                m_Average.Add(point);

                var updated = PositionUpdated;
                if (updated != null)
                {
                    updated(m_Average.GetAverage());
                }

                await Task.Delay(250);
            }
        }

        public void Cancel()
        {
            m_Cancelled = true;
        }

        public PositionAverage Average
        {
            get
            {
                return m_Average;
            }
        }
    }
}
