﻿namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Heatmap : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Heatmap";
    public string CategoryDescription => "Heatmaps display values from 2D data " +
        "as an image with cells of different intensities";

    public class HeatmapQuickstart : RecipeBase
    {
        public override string Name => "Heatmap Quickstart";
        public override string Description => "Heatmaps can be created from 2D arrays";

        [Test]
        public override void Execute()
        {
            double[,] data = SampleData.MonaLisa();
            myPlot.Add.Heatmap(data);
        }
    }

    public class HeatmapInverted : RecipeBase
    {
        public override string Name => "Inverted Heatmap";
        public override string Description => "Heatmaps can be inverted by " +
            "reversing the order of colors in the colormap";

        [Test]
        public override void Execute()
        {
            double[,] data = SampleData.MonaLisa();

            var hm1 = myPlot.Add.Heatmap(data);
            hm1.Colormap = new ScottPlot.Colormaps.Viridis();
            hm1.Extent = new(0, 65, 0, 100);

            var hm2 = myPlot.Add.Heatmap(data);
            hm2.Colormap = new ScottPlot.Colormaps.Viridis().Reversed();
            hm2.Extent = new(100, 165, 0, 100);
        }
    }

    public class HeatmapColormap : RecipeBase
    {
        public override string Name => "Heatmap with custom Colormap";
        public override string Description => "A heatmap's Colormap is the logic " +
            "used to convert from cell value to cell color and they can set by the user. " +
            "ScottPlot comes with many common colormaps, " +
            "but users may implement IColormap and apply their own. " +
            "A colorbar can be added to indicate which colors map to which values.";

        [Test]
        public override void Execute()
        {
            double[,] data = SampleData.MonaLisa();

            var hm1 = myPlot.Add.Heatmap(data);
            hm1.Colormap = new ScottPlot.Colormaps.Turbo();

            // TODO: this isn't working quite right yet...
            myPlot.Add.ColorBar(hm1);
        }
    }

    public class HeatmapFlip : RecipeBase
    {
        public override string Name => "Flipped Heatmap";
        public override string Description => "Heatmaps can be flipped horizontally and/or vertically";

        [Test]
        public override void Execute()
        {
            double[,] data = SampleData.MonaLisa();

            myPlot.Add.Text("default", 0, 1.5);
            var hm1 = myPlot.Add.Heatmap(data);
            hm1.Extent = new CoordinateRect(0, 1, 0, 1);

            myPlot.Add.Text("flip X", 2, 1.5);
            var hm2 = myPlot.Add.Heatmap(data);
            hm2.Extent = new CoordinateRect(2, 3, 0, 1);
            hm2.FlipHorizontally = true;

            myPlot.Add.Text("flip Y", 4, 1.5);
            var hm3 = myPlot.Add.Heatmap(data);
            hm3.Extent = new CoordinateRect(4, 5, 0, 1);
            hm3.FlipVertically = true;

            myPlot.Add.Text("flip X&Y", 6, 1.5);
            var hm4 = myPlot.Add.Heatmap(data);
            hm4.Extent = new CoordinateRect(6, 7, 0, 1);
            hm4.FlipHorizontally = true;
            hm4.FlipVertically = true;

            myPlot.Axes.SetLimits(-.5, 7.5, -1, 2);
        }
    }

    public class HeatmapSmooth : RecipeBase
    {
        public override string Name => "Smooth Heatmap";
        public override string Description => "Enable the `Smooth` property for anti-aliased rendering";

        [Test]
        public override void Execute()
        {
            double[,] data = SampleData.MonaLisa();

            myPlot.Add.Text("Smooth = false", 0, 1.1);
            var hm1 = myPlot.Add.Heatmap(data);
            hm1.Extent = new CoordinateRect(0, 1, 0, 1);

            myPlot.Add.Text("Smooth = true", 1.1, 1.1);
            var hm2 = myPlot.Add.Heatmap(data);
            hm2.Extent = new CoordinateRect(1.1, 2.1, 0, 1);
            hm2.Smooth = true;
        }
    }
}
