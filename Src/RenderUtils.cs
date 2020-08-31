using System;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace GameWorld
{
    /// <summary>
    /// Functions to help with rendering the game world.
    /// </summary>
    public class RenderUtils
    {
        /// <summary>
        /// Create and return a single actor given the mid point, rotation and colour.
        /// </summary>
        public static Polygon DrawActor(Point _mid, float _rot, SolidColorBrush _col)
        {
            Polygon ActorPol = new Polygon
            {
                Stroke = Brushes.Black,
                Fill = _col,
                StrokeThickness = 2,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            Point[] basePoints = new Point[]{new Point(0, -10), new Point(-6, 10), new Point(6, 10)};
            PointCollection points = new PointCollection();
            for (var i = 0; i < 3; ++i)
            {
                Point rotPoint = new Point(
                    (basePoints[i].X * Math.Cos(_rot)) - (basePoints[i].Y * Math.Sin(_rot)),
                    (basePoints[i].Y * Math.Cos(_rot)) + (basePoints[i].X * Math.Sin(_rot))
                );
                Point endPoint = new Point(rotPoint.X + _mid.X, rotPoint.Y + _mid.Y);
                points.Add(endPoint);
            }
            ActorPol.Points = points;
            return ActorPol;
        }
    }
}