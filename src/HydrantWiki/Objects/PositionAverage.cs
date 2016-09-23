using System;
using System.Collections.Generic;

namespace HydrantWiki.Objects
{
    public class PositionAverage
    {
        private List<GeoPoint> m_Positions;

        public void Add(GeoPoint _position)
        {
            m_Positions.Add(_position);
        }

        public void Add(
            double _latitude,
            double _logitude,
            double? _altitude,
            double? _accuracy)
        {
            if (_latitude != 0.0 && _logitude != 0.0)
            {
                GeoPoint position = new GeoPoint
                {
                    Latitude = _latitude,
                    Longitude = _logitude,
                    Altitude = _altitude,
                    Accuracy = _accuracy,
                    DeviceDateTime = DateTime.UtcNow
                };

                m_Positions.Add(position);
            }
        }

        public GeoPoint GetAverage()
        {
            int count = 0;
            double latitude = 0;
            double longitude = 0;

            double altitude = 0;
            int altitudeCount = 0;

            double accuracy = 0;
            int accuracyCount = 0;

            foreach (var position in m_Positions)
            {
                count++;
                latitude += position.Latitude;
                longitude += position.Longitude;

                if (position.Altitude.HasValue)
                {
                    altitude += position.Altitude.Value;
                    altitudeCount++;
                }

                if (position.Accuracy.HasValue)
                {
                    accuracy += position.Accuracy.Value;
                    accuracyCount++;
                }
            }

            if (count > 0)
            {
                GeoPoint position = new GeoPoint
                {
                    DeviceDateTime = DateTime.UtcNow,
                    Latitude = latitude / count,
                    Longitude = longitude / count,
                    WasAveraged = true
                }

                if (altitudeCount > 0)
                {
                    position.Altitude = altitude / altitudeCount;
                }

                if (accuracyCount > 0)
                {
                    position.Accuracy = accuracy / accuracyCount;
                }

                return position;
            }

            return null;
        }



    }
}
