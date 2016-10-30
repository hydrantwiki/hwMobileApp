using MapKit;

namespace HydrantWiki.iOS.Renderers
{
    public class HydrantsMKAnnotationView : MKAnnotationView
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public HydrantsMKAnnotationView(IMKAnnotation annotation, string id)
            : base(annotation, id)
        {
        }
    }
}
