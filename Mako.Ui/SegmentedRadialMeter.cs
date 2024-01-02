using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace Mako.Ui;

public class SegmentedRadialMeter : GraphicsView, IDrawable
{
    private double _min = 0;
    private double _max = 1;
    private double _warningFraction = 0.6;
    private double _hazardFraction = 0.8;
    private int _nsegments = 25;
    private double _value = 0.5;

    private Color _okayBrush = Colors.Green;
    private Color _warningBrush = Colors.Yellow;
    private Color _hazardBrush = Colors.Red;

    public double Min { get { return _min; } set { _min = value; Invalidate(); } }
    public double Max { get { return _max; } set { _max = value; Invalidate(); } }
    public double WarningFraction { get { return _warningFraction; } set { _warningFraction = value; Invalidate(); } }
    public double HazardFraction { get { return _hazardFraction; } set { _hazardFraction = value; Invalidate(); } }
    public int NumSegments { get { return _nsegments; } set { _nsegments = value; Invalidate(); } }
    public double Value { get { return _value; } set { _value = value; Invalidate(); } }
    public void Draw(ICanvas canvas, RectF canvasInfo)
    {
        var xmid = canvasInfo.Width / 2;
        var ymid = canvasInfo.Height / 2;

        var startAngle = (float)Math.PI * 5 / 4;
        var angleSpread = (float)Math.PI * 6 / 4;

        var valuePerSegment = (Max - Min) / _nsegments;
        var segmentStart = Min;
        var segmentEnd = Min + valuePerSegment;

        for (var i = 0; i < _nsegments; i++)
        {
            var leftEdgeAngle = startAngle - 2 * i * angleSpread / (2 * _nsegments - 1);
            var rightEdgeAngle = startAngle - (2 * i + 1) * angleSpread / (2 * _nsegments - 1);
            var p = new PathF();
            p.MoveTo(
                xmid + 0.4f * xmid * MathF.Cos(leftEdgeAngle),
                ymid - 0.4f * ymid * MathF.Sin(leftEdgeAngle));
            p.LineTo(
                xmid + xmid * MathF.Cos(leftEdgeAngle),
                ymid - ymid * MathF.Sin(leftEdgeAngle));
            p.LineTo(
                xmid + xmid * MathF.Cos(rightEdgeAngle),
                ymid - ymid * MathF.Sin(rightEdgeAngle));
            p.LineTo(
                xmid + 0.4f * xmid * MathF.Cos(rightEdgeAngle),
                ymid - 0.4f * ymid * MathF.Sin(rightEdgeAngle));
            p.Close();

            var color = (((double)i / _nsegments) > _hazardFraction)
                ? _hazardBrush
                : (((double)i / _nsegments) > _warningFraction)
                    ? _warningBrush
                    : _okayBrush;

            var alpha = (Value > segmentEnd)
                ? 1
                : (Value <= segmentStart)
                    ? 0.1f
                    : (0.1f + 0.9f * (float)((Value - segmentStart) / (valuePerSegment)));
            color = color.WithAlpha(alpha);

            canvas.FillColor = color;
            canvas.StrokeColor = color;
            canvas.DrawPath(p);
            canvas.FillPath(p);

            segmentStart += valuePerSegment;
            segmentEnd += valuePerSegment;
        }
    }

    public SegmentedRadialMeter()
    {
        Drawable = this;
        HorizontalOptions = LayoutOptions.Fill;
        VerticalOptions = LayoutOptions.Fill;
    }
}
