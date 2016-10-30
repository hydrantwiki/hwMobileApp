using System;
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

[assembly: ExportRenderer(typeof(HydrantsMap), typeof(HydrantsMapRenderer))]
namespace HydrantWiki.iOS.Renderers
{
    public class HydrantsMapRenderer : MapRenderer
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
                var formsMap = (HydrantsMap)e.NewElement;
                var nativeMap = Control as MKMapView;
                hydrantPins = formsMap.NearbyHydrants;

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

            if (hydrantPin != null)
            {
                annotationView = mapView.DequeueReusableAnnotation(hydrantPin.Hydrant.HydrantGuid.ToString());
                if (annotationView == null)
                {
                    annotationView = new HydrantsMKAnnotationView(annotation, hydrantPin.Hydrant.HydrantGuid.ToString());
                    annotationView.Image = UIImage.FromBundle("HydrantGreen");
                }
                annotationView.CanShowCallout = true;

                return annotationView;
            }

            return null;
        }

        void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
        {
            var customView = e.View as HydrantsMKAnnotationView;
            if (!string.IsNullOrWhiteSpace(customView.Url))
            {
                UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(customView.Url));
            }
        }

        void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            var customView = e.View as HydrantsMKAnnotationView;
            customPinView = new UIView();

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
            if (annotation != null)
            {
                var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
                foreach (var pin in hydrantPins)
                {
                    if (pin.Pin.Position == position)
                    {
                        return pin;
                    }
                }
            }

            return null;
        }
    }
}
