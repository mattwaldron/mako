using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Linq;
using System;
using System.Numerics;
using System.ComponentModel;

namespace Mako.Ui;

public class AnalogClock : GraphicsView, IDrawable, INotifyPropertyChanged
{
    private int _hour = 12;
    private int _minute = 0;
    public int Hour { get { return _hour; } set { _hour = value; Invalidate(); } }
    public int Minute { get { return _minute; } set { _minute = value; Invalidate(); } }

    public string Time => $"{Hour}:{Minute:D2}";
    public bool CanChange { get; set; } = false;

    private PointF _center;
    private float _radius;
    private bool _changingMinutes;
    private bool _changingHours;

    public event PropertyChangedEventHandler? PropertyChanged;
    public void Draw(ICanvas canvas, RectF canvasInfo)
    {
        _radius = Math.Min(canvasInfo.Width, canvasInfo.Height) / 2;
        _center = new PointF(canvasInfo.Width / 2, canvasInfo.Height / 2);

        canvas.StrokeColor = Color.FromRgb(0, 0, 0);
        canvas.StrokeSize = 6;
        float angle;
        for (var i = 0; i < 12; i++)
        {
            angle = 2 * MathF.PI * i / 12;
            canvas.DrawLine(_center.X + 0.80f * _radius * MathF.Sin(angle), _center.Y - 0.80f * _radius * MathF.Cos(angle),
                            _center.X + 0.95f * _radius * MathF.Sin(angle), _center.Y - 0.95f * _radius * MathF.Cos(angle));
        }

        canvas.StrokeColor = Color.FromRgb(0x44, 0x44, 0x44);
        canvas.StrokeSize = 4;
        angle = 2 * MathF.PI * _minute / 60;
        canvas.DrawLine(_center.X, _center.Y, _center.X + _radius * MathF.Sin(angle), _center.Y - _radius * MathF.Cos(angle));

        canvas.StrokeColor = Color.FromRgb(0x88, 0x88, 0x88);
        canvas.StrokeSize = 8;
        angle = 2 * MathF.PI * (_hour % 12) / 12;
        canvas.DrawLine(_center.X, _center.Y, _center.X + 0.6f * _radius * MathF.Sin(angle), _center.Y - 0.6f * _radius * MathF.Cos(angle));
    }

    private float DetermineAngle(PointF coords)
    {
        coords.X -= _center.X;
        coords.Y -= _center.Y;
        coords.Y *= -1;
        float angle = 0;
        if (coords.X == 0)
        {
            angle = coords.Y > 0
                ? angle = MathF.PI
                : 0;
        }
        else if (coords.Y == 0)
        {
            angle = coords.X > 0
                ? MathF.PI / 2
                : MathF.PI * 3 / 2;
        }
        else
        {
            angle = coords.Y < 0
                ? MathF.PI - MathF.Atan(-coords.X / coords.Y)
                : MathF.Atan(coords.X / coords.Y);
        }
        while (angle < 0)
        {
            angle += MathF.PI * 2;
        }
        return angle;
    }

    private int ClosestHour(float angle)
    {
        var scale = 12 / (2 * MathF.PI);
        var h = (int)Math.Round(angle * scale);
        if (h == 0)
        {
            h = 12;
        }
        return h;
    }

    private int ClosestMinute(float angle)
    {
        var scale = 60 / (2 * MathF.PI);
        var m = (int)Math.Round(angle * scale);
        return (m % 60);
    }

    public void OnStartInteraction(object sender, TouchEventArgs args)
    {
        if (!CanChange)
        {
            return;
        }
        var press = args.Touches.FirstOrDefault();
        var distance = press.Distance(_center);
        if (distance > _radius)
        {
            return;
        }
        var angle = DetermineAngle(press);
        if (distance < 0.6 * _radius)
        {
            _changingHours = true;
            Hour = ClosestHour(angle);
        }
        else
        {
            _changingMinutes = true;
            Minute = ClosestMinute(angle);
        }
    }

    public void OnDragInteraction(object sender, TouchEventArgs args)
    {
        if (!CanChange)
        {
            return;
        }
        var touch = args.Touches.LastOrDefault();
        if (touch != null)
        {
            if (_changingHours)
            {
                Hour = ClosestHour(DetermineAngle(touch));
            }
            if (_changingMinutes)
            {
                Minute = ClosestMinute(DetermineAngle(touch));
            }
        }
    }

    public void OnEndInteraction(object sender, TouchEventArgs args)
    {
        if (!CanChange)
        {
            return;
        }
        var touch = args.Touches.LastOrDefault();
        if (touch != null)
        {
            if (_changingHours)
            {
                Hour = ClosestHour(DetermineAngle(touch));
            }
            if (_changingMinutes)
            {
                Minute = ClosestMinute(DetermineAngle(touch));
            }
        }
        _changingHours = false;
        _changingMinutes = false;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
    }

    public AnalogClock()
    {
        Drawable = this;
        HorizontalOptions = LayoutOptions.Fill;
        VerticalOptions = LayoutOptions.Fill;
        StartInteraction += OnStartInteraction;
        DragInteraction += OnDragInteraction;
        EndInteraction += OnEndInteraction;
    }
}
