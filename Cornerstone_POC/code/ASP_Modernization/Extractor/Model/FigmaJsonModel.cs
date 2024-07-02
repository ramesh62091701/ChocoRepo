using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Model
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AbsoluteBoundingBox
    {
        public double x { get; set; }
        public double y { get; set; }
        public double width { get; set; }
        public double height { get; set; }
    }

    public class ArcData
    {
        public double startingAngle { get; set; }
        public double endingAngle { get; set; }
        public double innerRadius { get; set; }
    }

    public class Background
    {
        public string blendMode { get; set; }
        public bool visible { get; set; }
        public string type { get; set; }
        public Color color { get; set; }
    }

    public class BackgroundColor
    {
        public double r { get; set; }
        public double g { get; set; }
        public double b { get; set; }
        public double a { get; set; }
    }

    public class Child
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string scrollBehavior { get; set; }
        public string blendMode { get; set; }
        public AbsoluteBoundingBox absoluteBoundingBox { get; set; }
        public object absoluteRenderBounds { get; set; }
        public Constraints constraints { get; set; }
        public string layoutAlign { get; set; }
        public double layoutGrow { get; set; }
        public string layoutSizingHorizontal { get; set; }
        public string layoutSizingVertical { get; set; }
        public List<Fill> fills { get; set; }
        public List<Stroke> strokes { get; set; }
        public double strokeWeight { get; set; }
        public string strokeAlign { get; set; }
        public Styles styles { get; set; }
        public List<object> exportSettings { get; set; }
        public List<Effect> effects { get; set; }
        public string characters { get; set; }
        public Style style { get; set; }
        public int layoutVersion { get; set; }
        public List<object> characterStyleOverrides { get; set; }
        public List<string> lineTypes { get; set; }
        public List<int> lineIndentations { get; set; }
        public List<Child> children { get; set; }
        public bool? clipsContent { get; set; }
        public List<Background> background { get; set; }
        public double? cornerRadius { get; set; }
        public double? cornerSmoothing { get; set; }
        public BackgroundColor backgroundColor { get; set; }
        public string layoutMode { get; set; }
        public double? itemSpacing { get; set; }
        public string counterAxisAlignItems { get; set; }
        public double? paddingLeft { get; set; }
        public double? paddingRight { get; set; }
        public double? paddingTop { get; set; }
        public double? paddingBottom { get; set; }
        public string layoutWrap { get; set; }
        public string transitionNodeID { get; set; }
        public double? transitionDuration { get; set; }
        public string transitionEasing { get; set; }
        public string componentId { get; set; }
        public ComponentProperties componentProperties { get; set; }
        public List<Override> overrides { get; set; }
        public string counterAxisSizingMode { get; set; }
        public string primaryAxisAlignItems { get; set; }
        public ComponentPropertyReferences componentPropertyReferences { get; set; }
        public bool? visible { get; set; }
        public double rotation { get; set; }
        public bool preserveRatio { get; set; }
        public ArcData arcData { get; set; }
    }

    public class Color
    {
        public double r { get; set; }
        public double g { get; set; }
        public double b { get; set; }
        public double a { get; set; }
    }

    public class ComponentProperties
    {
        public Size Size { get; set; }
        public Rotation Rotation { get; set; }

        public Type Type { get; set; }
        public State State { get; set; }
    }

    public class ComponentPropertyReferences
    {
        public string characters { get; set; }
    }

    public class Constraints
    {
        public string vertical { get; set; }
        public string horizontal { get; set; }
    }

    public class Effect
    {
        public string type { get; set; }
        public bool visible { get; set; }
        public Color color { get; set; }
        public string blendMode { get; set; }
        public Offset offset { get; set; }
        public double radius { get; set; }
        public bool showShadowBehindNode { get; set; }
    }

    public class Fill
    {
        public string blendMode { get; set; }
        public string type { get; set; }
        public Color color { get; set; }
        public bool? visible { get; set; }
        public List<GradientHandlePosition> gradientHandlePositions { get; set; }
        public List<GradientStop> gradientStops { get; set; }
    }

    public class GradientHandlePosition
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class GradientStop
    {
        public Color color { get; set; }
        public double position { get; set; }
    }

    public class Offset
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class Override
    {
        public string id { get; set; }
        public List<string> overriddenFields { get; set; }
    }

    public class FigmaJsonModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool visible { get; set; }
        public string type { get; set; }
        public string scrollBehavior { get; set; }
        public string blendMode { get; set; }
        public List<Child> children { get; set; }
        public AbsoluteBoundingBox absoluteBoundingBox { get; set; }
        public object absoluteRenderBounds { get; set; }
        public Constraints constraints { get; set; }
        public string layoutSizingHorizontal { get; set; }
        public string layoutSizingVertical { get; set; }
        public bool clipsContent { get; set; }
        public List<object> background { get; set; }
        public List<object> fills { get; set; }
        public List<object> strokes { get; set; }
        public double strokeWeight { get; set; }
        public string strokeAlign { get; set; }
        public BackgroundColor backgroundColor { get; set; }
        public string layoutMode { get; set; }
        public double itemSpacing { get; set; }
        public string counterAxisAlignItems { get; set; }
        public string layoutWrap { get; set; }
        public List<object> exportSettings { get; set; }
        public List<object> effects { get; set; }
    }

    public class Rotation
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class Size
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class State
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class Stroke
    {
        public string blendMode { get; set; }
        public string type { get; set; }
        public Color color { get; set; }
    }

    public class Style
    {
        public string fontFamily { get; set; }
        public string fontPostScriptName { get; set; }
        public int fontWeight { get; set; }
        public string textAutoResize { get; set; }
        public double fontSize { get; set; }
        public string textAlignHorizontal { get; set; }
        public string textAlignVertical { get; set; }
        public double letterSpacing { get; set; }
        public double lineHeightPx { get; set; }
        public double lineHeightPercent { get; set; }
        public double lineHeightPercentFontSize { get; set; }
        public string lineHeightUnit { get; set; }
    }

    public class Styles
    {
        public string fill { get; set; }
        public string text { get; set; }
        public string fills { get; set; }
        public string strokes { get; set; }
        public string stroke { get; set; }
        public string effect { get; set; }
    }

    public class Type
    {
        public string value { get; set; }
        public string type { get; set; }
    }


}
