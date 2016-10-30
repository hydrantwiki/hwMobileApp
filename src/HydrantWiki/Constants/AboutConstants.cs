using System;
namespace HydrantWiki.Constants
{
    public static class AboutConstants
    {
        public const string AboutText = @"
            <html>
                <head>
                    <link href=""https://fonts.googleapis.com/css?family=Open+Sans"" rel=""stylesheet"">
                </head>
                <body>
                    <style>
                      body {
                        font-family: 'Open Sans', sans-serif;
                      }
                    </style>
                    <h2>HydrantWiki</h2>
                    <p>An open source and open data initiative to collect hydrant locations.</p>
                    <h3>How it works</h3>
                    <p>Users walk around and collect the location of hydrant tags from sidewalks and other safe locations.
                       These tags are then sent to the server for review.  Once reviewed they become visible as hydrants on 
                       the map.  For a tag to be collected the Hydrant's position and image must be captured.</p>
                    <p>Always capture hydrants tags from a safe location.  Do not tresspass to collect hydrant tags.</p>
                    <p>Copyright 2016 Brian Nelson</p>
                    <h3>Open Source projects used in HydrantWiki</h3>
                    <ul>
                        <li>LiteDb</li>
                        <li>NewtonSoft Json</li>
                        <li>XLabs - Xamarin Forms Labs</li> 
                        <li>RestSharp</li>
                        <li>Hydrant icons were provided by the Maps Icons Collection (mapicons.mapsmarker.com)</li>
                    </ul>
                </body>
            </html> ";

    }
}
