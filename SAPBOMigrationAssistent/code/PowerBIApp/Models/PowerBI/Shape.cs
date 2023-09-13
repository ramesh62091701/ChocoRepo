using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBIApp.Models.PowerBI.Shape
{
    public class Color
    {
        public Expr expr { get; set; }
    }

    public class Expr
    {
        public Literal Literal { get; set; }
        public ThemeDataColor ThemeDataColor { get; set; }
    }

    public class Fill
    {
        public Properties properties { get; set; }
        public Selector selector { get; set; }
    }

    public class FillColor
    {
        public Solid solid { get; set; }
    }

    public class Literal
    {
        public string Value { get; set; }
    }

    public class Objects
    {
        public List<Shape> shape { get; set; }
        public List<Rotation> rotation { get; set; }
        public List<Fill> fill { get; set; }
        public List<Outline> outline { get; set; }
    }

    public class Outline
    {
        public Properties properties { get; set; }
    }

    public class Properties
    {
        public TileShape tileShape { get; set; }
        public ShapeAngle shapeAngle { get; set; }
        public FillColor fillColor { get; set; }
        public Show show { get; set; }
    }

    public class ShapeRoot
    {
        public string visualType { get; set; }
        public bool drillFilterOtherVisuals { get; set; }
        public Objects objects { get; set; }
    }

    public class Rotation
    {
        public Properties properties { get; set; }
    }

    public class Selector
    {
        public string id { get; set; }
    }

    public class Shape
    {
        public Properties properties { get; set; }
    }

    public class ShapeAngle
    {
        public Expr expr { get; set; }
    }

    public class Show
    {
        public Expr expr { get; set; }
    }

    public class Solid
    {
        public Color color { get; set; }
    }

    public class ThemeDataColor
    {
        public int ColorId { get; set; }
        public int Percent { get; set; }
    }

    public class TileShape
    {
        public Expr expr { get; set; }
    }


}
