﻿using System.Collections.Generic;
using System.Drawing;
using Buddy.Common.Structures;

namespace Buddy.Common.Printers
{
    public class Drawer
    {
        private static int _number;

        public static void DrawGraph(Size size, IGraph graph, IList<Coordinate> coords, string fileName, bool fill)
        {
            var vertexBrush = Brushes.Red;
            var vertexPen = new Pen(Color.Red, 1);
            var edgePen = new Pen(Color.Blue, 1);

            var bitmap = new Bitmap(size.Width, size.Height);
            using (var image = Graphics.FromImage(bitmap))
            {
                for (var i = 0; i < graph.RowIndex.Length - 1; i++)
                {
                    for (var k = graph.RowIndex[i]; k < graph.RowIndex[i + 1]; k++)
                    {
                        var j = (int)graph.ColumnIndex[k];
                        var a = coords[i];
                        var b = coords[j];
                        image.DrawLine(edgePen, a.FloatX, a.FloatY, b.FloatX, b.FloatY);
                    }
                }

                const int scale = 1;
                for(var i=0;i<graph.VerticesAmount;i++)
                {
                    var x = new Coordinate(coords[i].X, coords[i].Y);
                    var radius = graph.Radius[i] * scale;

                    x.X -= radius / 2;
                    x.Y -= radius / 2;
                    if (fill)
                    {
                        image.FillEllipse(vertexBrush, x.FloatX, x.FloatY, (float)radius, (float)radius);
                    }
                    else
                    {
                        image.DrawEllipse(vertexPen, x.FloatX, x.FloatY, (float)radius, (float)radius);
                    }
                }
            }
            bitmap.Save(string.Format("{0}. {1}",_number++, fileName));
            edgePen.Dispose();
            vertexPen.Dispose();
        }
    }
}