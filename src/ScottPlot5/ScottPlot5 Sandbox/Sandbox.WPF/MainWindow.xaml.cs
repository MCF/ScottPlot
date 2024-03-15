using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using ScottPlot;
using ScottPlot.Plottables;

namespace Sandbox.WPF;

public partial class MainWindow : Window
{
    AxisLine? PlottableBeingDragged = null;

    public MainWindow()
    {
        InitializeComponent();
        WpfPlot1.Plot.Add.Signal(Generate.Sin());
        WpfPlot1.Plot.Add.Signal(Generate.Cos());

        //WpfPlot1.Plot.ScaleFactor = 4;
        WpfPlot1.Plot.ScaleFactor = WpfPlot1.DisplayScale;

        Debug.WriteLine($"WpfPlot1.DisplayScale: {WpfPlot1.DisplayScale}");
        Debug.WriteLine($"WpfPlot1.Plot.ScaleFactor: {WpfPlot1.Plot.ScaleFactor}");

        var vl = WpfPlot1.Plot.Add.VerticalLine(23);
        vl.IsDraggable = true;
        vl.Text = "VLine";

        var hl = WpfPlot1.Plot.Add.HorizontalLine(0.42);
        hl.IsDraggable = true;
        hl.Text = "HLine";

        WpfPlot1.Refresh();

        // use events for custom mouse interactivity
        WpfPlot1.MouseDown += FormsPlot1_MouseDown;
        WpfPlot1.MouseUp += FormsPlot1_MouseUp;
        WpfPlot1.MouseMove += FormsPlot1_MouseMove;
    }

    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
        var position = GetScaledMousePosition(e);
        var lineUnderMouse = GetLineUnderMouse(position.X, position.Y);
        if (lineUnderMouse is not null)
        {
            PlottableBeingDragged = lineUnderMouse;
            WpfPlot1.Interaction.Disable(); // disable panning while dragging
        }
    }

    private void FormsPlot1_MouseUp(object? sender, MouseEventArgs e)
    {
        PlottableBeingDragged = null;
        WpfPlot1.Interaction.Enable(); // enable panning again
        WpfPlot1.Refresh();
    }

    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        var position = GetScaledMousePosition(e);

        // this rectangle is the area around the mouse in coordinate units
        CoordinateRect rect = WpfPlot1.Plot.GetCoordinateRect(position.X, position.Y, radius: 10);

        if (PlottableBeingDragged is null)
        {
            // set cursor based on what's beneath the plottable
            var lineUnderMouse = GetLineUnderMouse(position.X, position.Y);
            if (lineUnderMouse is null) Cursor = Cursors.Arrow;
            else if (lineUnderMouse.IsDraggable && lineUnderMouse is VerticalLine) Cursor = Cursors.SizeWE;
            else if (lineUnderMouse.IsDraggable && lineUnderMouse is HorizontalLine) Cursor = Cursors.SizeNS;
        }
        else
        {
            // update the position of the plottable being dragged
            if (PlottableBeingDragged is HorizontalLine hl)
            {
                hl.Y = rect.VerticalCenter;
                hl.Text = $"{hl.Y:0.00}";
            }
            else if (PlottableBeingDragged is VerticalLine vl)
            {
                vl.X = rect.HorizontalCenter;
                vl.Text = $"{vl.X:0.00}";
            }
            WpfPlot1.Refresh();
        }
    }

    private Pixel GetScaledMousePosition(MouseEventArgs e)
    {
        var position = e.GetPosition(WpfPlot1);
        if (WpfPlot1.DisplayScale != 1.0)
        {
            position.X *= WpfPlot1.DisplayScale;
            position.Y *= WpfPlot1.DisplayScale;
        }

        return new Pixel(position.X, position.Y);
    }

    private AxisLine? GetLineUnderMouse(float x, float y)
    {
        float r = 10;
        if (WpfPlot1.DisplayScale != 1.0)
        {
            r *= WpfPlot1.DisplayScale;
        }

        CoordinateRect rect = WpfPlot1.Plot.GetCoordinateRect(x, y, radius: r);

        foreach (AxisLine axLine in WpfPlot1.Plot.GetPlottables<AxisLine>())
        {
            if (axLine.IsUnderMouse(rect))
                return axLine;
        }

        return null;
    }
}
