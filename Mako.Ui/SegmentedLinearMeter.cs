using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Mako.Ui;

public class SegmentedLinearMeter : GraphicsView, IDrawable
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
        var height = canvasInfo.Height;
        var spread = canvasInfo.Width;
        var width = 1 * spread / (2 * _nsegments - 1);

        var valuePerSegment = (Max - Min) / _nsegments;
        var segmentStart = Min;
        var segmentEnd = Min + valuePerSegment;
        for (var i = 0; i < _nsegments; i++)
        {
            var leftEdge = 2 * i * spread / (2 * _nsegments - 1);
            var p = new PathF();
            p.MoveTo(leftEdge, 0);
            p.LineTo(leftEdge + width, 0);
            p.LineTo(leftEdge + width, height);
            p.LineTo(leftEdge, height);
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

    public SegmentedLinearMeter()
    {
        Drawable = this;
        HorizontalOptions = LayoutOptions.Fill;
        VerticalOptions = LayoutOptions.Fill;
    }
}
