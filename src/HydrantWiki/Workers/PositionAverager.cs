using System;
using System.Threading.Tasks;
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

        public PositionAverager(LocationManager _location, int count)
        {
            m_Location = _location;
            m_DesiredPositionCount = count;
        }

        public async Task Start()
        {
            m_Cancelled = false;
            m_Average = new PositionAverage();
            m_Location.PositionChanged += PositionChanged;

            for (int i = 0; i < m_DesiredPositionCount; i++)
            {
                GeoPoint point = await m_Location.GetLocation();
                m_Average.Add(point);

                await Task.Delay(200);
            }

            m_Location.StopListening();
            m_Location.PositionChanged -= PositionChanged;
        }

        public void Cancel()
        {
            m_Cancelled = true;
        }

        void PositionChanged(GeoPoint position)
        {
            m_Average.Add(position);
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
