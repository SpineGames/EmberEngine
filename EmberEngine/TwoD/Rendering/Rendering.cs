using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EmberEngine.TwoD
{
    public class AdvancedRendering
    {
        public static void DrawLine(Vector2 Point1, Vector2 Point2, Color Color, GraphicsDevice Graphics, float depth = 0)
        {
            VertexPositionColor[] line = new VertexPositionColor[2];
            line[0] = new VertexPositionColor(new Vector3(Point1, depth), Color);
            line[1] = new VertexPositionColor(new Vector3(Point2, depth) ,Color);

            Graphics.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, line, 0, 1);
        }

        public static void DrawCrossHairs(Vector2 position, int length, Color Color, GraphicsDevice Graphics, float depth = 0)
        {
            DrawLine(
                new Vector2(position.X - length, position.Y),
                new Vector2(position.X + length, position.Y),
                Color, Graphics);
            DrawLine(
                new Vector2(position.X, position.Y - length),
                new Vector2(position.X, position.Y + length),
                Color, Graphics);
        }
    }
}
