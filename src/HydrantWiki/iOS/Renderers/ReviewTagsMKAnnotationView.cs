using MapKit;

namespace HydrantWiki.iOS.Renderers
{
    public class ReviewTagsMKAnnotationView : MKAnnotationView
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public ReviewTagsMKAnnotationView(IMKAnnotation annotation, string id)
            : base(annotation, id)
        {
        }
    }
}
