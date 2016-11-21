using System.Collections.Generic;
using CoreGraphics;
using HydrantWiki.Controls;
using HydrantWiki.iOS.Renderers;
using HydrantWiki.Objects;
using MapKit;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ReviewTagMap), typeof(ReviewTagMapRenderer))]
namespace HydrantWiki.iOS.Renderers
{
    public class ReviewTagMapRenderer : MapRenderer
    {
        UIView customPinView;
        List<HydrantPin> hydrantPins;
        TagPin tagPin;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                var nativeMap = Control as MKMapView;
                nativeMap.GetViewForAnnotation = null;
                nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
                nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
                nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
            }

            if (e.NewElement != null)
            {
                var formsMap = (ReviewTagMap)e.NewElement;
                var nativeMap = Control as MKMapView;
                hydrantPins = formsMap.NearbyHydrants;
                tagPin = formsMap.TagToReview;

                nativeMap.GetViewForAnnotation = GetViewForAnnotation;
                //nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
                //nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
                //nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
            }
        }

        MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;

            if (annotation is MKUserLocation)
                return null;

            var anno = annotation as MKPointAnnotation;
            var hydrantPin = GetHydrantPin(anno);
            if (hydrantPin == null)
            {
                var tp = GetTagPin(anno);

                if (tp != null)
                {
                    annotationView = mapView.DequeueReusableAnnotation(tp.Tag.Id.ToString());
                    if (annotationView == null)
                    {
                        annotationView = new ReviewTagsMKAnnotationView(annotation, tp.Tag.Id.ToString());
                        annotationView.Image = UIImage.FromBundle("HydrantRed");
                    }
                    annotationView.CanShowCallout = true;

                    return annotationView;
                }
            }

            annotationView = mapView.DequeueReusableAnnotation(hydrantPin.Hydrant.HydrantGuid.ToString());
            if (annotationView == null)
            {
                annotationView = new ReviewTagsMKAnnotationView(annotation, hydrantPin.Hydrant.HydrantGuid.ToString());
                annotationView.Image = UIImage.FromBundle("HydrantGreen");
            }
            annotationView.CanShowCallout = true;

            return annotationView;
        }

        void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
        {
            var customView = e.View as ReviewTagsMKAnnotationView;
            if (!string.IsNullOrWhiteSpace(customView.Url))
            {
                UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(customView.Url));
            }
        }

        void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            var customView = e.View as ReviewTagsMKAnnotationView;
            customPinView = new UIView();

            if (customView.Id == "Xamarin")
            {
                customPinView.Frame = new CGRect(0, 0, 200, 84);
                var image = new UIImageView(new CGRect(0, 0, 200, 84));
                image.Image = UIImage.FromFile("xamarin.png");
                customPinView.AddSubview(image);
                customPinView.Center = new CGPoint(0, -(e.View.Frame.Height + 75));
                e.View.AddSubview(customPinView);
            }
        }

        void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            if (!e.View.Selected)
            {
                customPinView.RemoveFromSuperview();
                customPinView.Dispose();
                customPinView = null;
            }
        }

        HydrantPin GetHydrantPin(MKPointAnnotation annotation)
        {
            var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
            foreach (var pin in hydrantPins)
            {
                if (pin.Pin.Position == position)
                {
                    return pin;
                }
            }


            return null;
        }

        TagPin GetTagPin(MKPointAnnotation annotation)
        {
            var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);

            if (tagPin.Pin.Position == position)
            {
                return tagPin;
            }

            return null;
        }
    }
}
