using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace Mako.Ui
{
    public partial class ContinuousRadialMeter : GraphicsView, IDrawable
    {
        private double _min = 0;
        private double _max = 1;
        private double _warningFraction = 0.6;
        private double _hazardFraction = 0.8;
        private double _value = 0.5;

        private Color _okayBrush = Colors.Green;
        private Color _warningBrush = Colors.Yellow;
        private Color _hazardBrush = Colors.Red;

        public double Min { get { return _min; } set { _min = value; Invalidate(); } }
        public double Max { get { return _max; } set { _max = value; Invalidate(); } }
        public double WarningFraction { get { return _warningFraction; } set { _warningFraction = value; Invalidate(); } }
        public double HazardFraction { get { return _hazardFraction; } set { _hazardFraction = value; Invalidate(); } }
        public double Value { get { return _value; } set { _value = value; Invalidate(); } }

        public ContinuousRadialMeter()
        {
            Drawable = this;
            HorizontalOptions = LayoutOptions.Fill;
            VerticalOptions = LayoutOptions.Fill;
        }

        private void AddBar(ICanvas canvas, RectF canvasInfo, double start, double end, float opacity, Color color)
        {
            var xmid = canvasInfo.Width / 2;
            var ymid = canvasInfo.Height / 2;

            var startAngle = MathF.PI * 5 / 4;
            var angleSpread = MathF.PI * 6 / 4;

            var ss = startAngle - angleSpread * (float)start;
            var ee = startAngle - angleSpread * (float)end;

            float a;
            var p = new PathF();
            p.MoveTo(xmid + 0.4f * xmid * MathF.Cos(ss), ymid - 0.4f * ymid * MathF.Sin(ss));
            for (a = ss - 0.02f; a > ee; a -= 0.02f)
            {
                p.LineTo(xmid + 0.4f * xmid * MathF.Cos(a), ymid - 0.4f * ymid * MathF.Sin(a));
            }
            a = ee;

            p.LineTo(xmid + 0.4f * xmid * MathF.Cos(a), ymid - 0.4f * ymid * MathF.Sin(a));

            for (a = ee; a < ss; a += 0.02f)
            {
                p.LineTo(xmid + xmid * MathF.Cos(a), ymid - ymid * MathF.Sin(a));
            }
            a = ss;
            p.LineTo(xmid + xmid * MathF.Cos(a), ymid - ymid * MathF.Sin(a));
            p.Close();

            color = color.WithAlpha(opacity);

            canvas.FillColor = color;
            canvas.StrokeColor = color;
            canvas.DrawPath(p);
            canvas.FillPath(p);
        }

        private void AddFullOkayBar(ICanvas canvas, RectF canvasInfo, float opacity)
        {
            AddBar(canvas, canvasInfo, 0, _warningFraction, opacity, _okayBrush);
        }

        private void AddFullWarningBar(ICanvas canvas, RectF canvasInfo, float opacity)
        {
            AddBar(canvas, canvasInfo, _warningFraction, _hazardFraction, opacity, _warningBrush);
        }

        private void AddFullHazardBar(ICanvas canvas, RectF canvasInfo, float opacity)
        {
            AddBar(canvas, canvasInfo, _hazardFraction, 1, opacity, _hazardBrush);
        }

        public void Draw(ICanvas canvas, RectF canvasInfo)
        {
            if (_value <= _min || _value >= _max)
            {
                var opacity = _value <= _min ? 0.1f : 1;
                AddFullOkayBar(canvas, canvasInfo, opacity);
                AddFullWarningBar(canvas, canvasInfo, opacity);
                AddFullHazardBar(canvas, canvasInfo, opacity);
            }
            else
            {
                var valueFraction = (_value - _min) / (_max - _min);
                if (valueFraction <= _warningFraction)
                {
                    AddBar(canvas, canvasInfo, 0, valueFraction, 1, _okayBrush);
                    AddBar(canvas, canvasInfo, valueFraction, _warningFraction, 0.1f, _okayBrush);
                    AddFullWarningBar(canvas, canvasInfo, 0.1f);
                    AddFullHazardBar(canvas, canvasInfo, 0.1f);
                }
                else if (valueFraction <= _hazardFraction)
                {
                    AddFullOkayBar(canvas, canvasInfo, 1);
                    AddBar(canvas, canvasInfo, _warningFraction, valueFraction, 1, _warningBrush);
                    AddBar(canvas, canvasInfo, valueFraction, _hazardFraction, 0.1f, _warningBrush);
                    AddFullHazardBar(canvas, canvasInfo, 0.1f);
                }
                else
                {
                    AddFullOkayBar(canvas, canvasInfo, 1);
                    AddFullWarningBar(canvas, canvasInfo, 1);
                    AddBar(canvas, canvasInfo, _hazardFraction, valueFraction, 1, _hazardBrush);
                    AddBar(canvas, canvasInfo, valueFraction, 1, 0.1f, _hazardBrush);
                }
            }
        }
    }
}
