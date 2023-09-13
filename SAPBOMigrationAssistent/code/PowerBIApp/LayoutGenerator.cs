using Microsoft.PowerBI.Api.Models;
using Newtonsoft.Json;
using PowerBIApp.Models.GenericLayout;
using PowerBIApp.Models.PowerBI;
using PowerBIApp.Models.PowerBI.Layout;
using PowerBIApp.Models.PowerBI.LayoutConfig;
using PowerBIApp.Models.PowerBI.Query;
using PowerBIApp.Models.PowerBI.SectionConfig;
using PowerBIApp.Models.PowerBI.Shape;
using PowerBIApp.Models.PowerBI.TableEx;
using PowerBIApp.Models.PowerBI.VisualContainerConfig;

namespace PowerBIApp
{
    public class LayoutGenerator
    {
        private double x;
        private double y;
        private const double sectionHeight = 5;
        private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

        public void GenerateLayout(string reportName, List<Table> tables)
        {
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            x = 10;
            y = 0;
            //var report = File.ReadAllText(".\\models\\report.json");
            //var obj = JsonConvert.DeserializeObject<GenericLayoutRoot>(report, jsonSerializerSettings);
            var layout = GenerateLayoutInternal(reportName, tables);
            var layoutfile = JsonConvert.SerializeObject(layout, jsonSerializerSettings);
            Utility.CreatePbit(layoutfile);
        }

        private GenericLayoutRoot GenerateLayoutInternal(string reportName, List<Table> tables)
        {
            var layout = new GenericLayoutRoot();
            layout.pages = new List<Models.GenericLayout.Page>();

            var page = new PowerBIApp.Models.GenericLayout.Page();
            layout.pages.Add(page);

            page.name = reportName;
            page.displayName = reportName;
            page.containers = new List<Container>();
            foreach (var table in tables)
            {
                var container = new Container();
                container.tableName = table.Name;
                container.columns = new List<string>();
                foreach (var item in table.Columns)
                {
                    container.columns.Add(item.Name);
                }
                page.containers.Add(container);
            }
            return layout;
        }

        public LayoutRoot Generate(GenericLayoutRoot genericLayoutRoot)
        {
            var layoutRoot = new LayoutRoot();
            layoutRoot.id = 0;

            //Add resource packages
            layoutRoot.resourcePackages = new List<ResourcePackage>();
            layoutRoot.resourcePackages.Add(GetResourcePackage());

            //Add sections
            layoutRoot.sections = new List<Models.PowerBI.Layout.Section>();
            for (int i = 0; i < genericLayoutRoot.pages.Count; i++)
            {
                var page = genericLayoutRoot.pages[i];
                var section = GetSection(page, i);
                layoutRoot.sections.Add(section);
            }
            var layoutConfig = GetLayoutConfigRoot();
            layoutRoot.config = JsonConvert.SerializeObject(layoutConfig, jsonSerializerSettings);
            layoutRoot.layoutOptimization = 0;
            return layoutRoot;
        }

        private ResourcePackage GetResourcePackage()
        {
            var resourcePackage = new ResourcePackage();
            resourcePackage.resourcePackage = new ResourcePackage2();
            resourcePackage.resourcePackage.name = "SharedResources";
            resourcePackage.resourcePackage.type = 2;
            resourcePackage.resourcePackage.disabled = false;
            resourcePackage.resourcePackage.items = new List<Item>();

            resourcePackage.resourcePackage.items.Add(new Item
            {
                type = 202,
                path = "BaseThemes/CY23SU04.json",
                name = "CY23SU04"
            });
            return resourcePackage;
        }

        private Models.PowerBI.Layout.Section GetSection(PowerBIApp.Models.GenericLayout.Page page, int index)
        {
            var section = new Models.PowerBI.Layout.Section();
            section.id = 0;
            section.name = page.name;
            section.displayName = page.displayName;
            section.filters = "[]";
            section.ordinal = 0;
            section.height = page.height;
            section.width = page.width;
            section.displayOption = 2;

            section.visualContainers = new List<VisualContainer>();
            for (int i = 0; i < page.containers.Count; i++)
            {
                Container? container = page.containers[i];
                section.visualContainers.Add(GetVisualContainer(container, i));
            }
 
            var sectionConfig = GetSectionConfig();
            section.config = JsonConvert.SerializeObject(sectionConfig, jsonSerializerSettings);
            return section;
        }
        private SectionConfigRoot GetSectionConfig()
        {
            var sectionConfigRoot = new SectionConfigRoot();
            return sectionConfigRoot;
        }

        private LayoutConfigRoot GetLayoutConfigRoot()
        {
            var layoutConfigRoot = new LayoutConfigRoot();
            layoutConfigRoot.version = "5.44";
            layoutConfigRoot.themeCollection = new ThemeCollection();
            layoutConfigRoot.themeCollection.baseTheme = new BaseTheme();
            layoutConfigRoot.themeCollection.baseTheme.name = "CY23SU04";
            layoutConfigRoot.themeCollection.baseTheme.version = "5.46";
            layoutConfigRoot.themeCollection.baseTheme.type = 2;
            layoutConfigRoot.activeSectionIndex = 1;
            layoutConfigRoot.defaultDrillFilterOtherVisuals = true;
            layoutConfigRoot.settings = new Settings();
            layoutConfigRoot.settings.useNewFilterPaneExperience = true;
            layoutConfigRoot.settings.allowChangeFilterTypes = true;
            layoutConfigRoot.settings.useStylableVisualContainerHeader = true;
            layoutConfigRoot.settings.queryLimitOption = 6;
            layoutConfigRoot.settings.exportDataMode = 1;
            layoutConfigRoot.settings.useDefaultAggregateDisplayName = true;
            layoutConfigRoot.objects = new Models.PowerBI.LayoutConfig.Objects();
            layoutConfigRoot.objects.section = new List<Models.PowerBI.LayoutConfig.Section>();
            var section = new Models.PowerBI.LayoutConfig.Section();
            layoutConfigRoot.objects.section.Add(section);
            section.properties = new Models.PowerBI.LayoutConfig.Properties();
            section.properties.verticalAlignment = new VerticalAlignment();
            section.properties.verticalAlignment.expr = new Models.PowerBI.LayoutConfig.Expr();
            section.properties.verticalAlignment.expr.Literal = new Models.PowerBI.LayoutConfig.Literal();
            section.properties.verticalAlignment.expr.Literal.Value = "Top";
            layoutConfigRoot.objects.outspacePane = new List<OutspacePane>();
            var op = new OutspacePane();
            layoutConfigRoot.objects.outspacePane.Add(op);
            op.properties = new Models.PowerBI.LayoutConfig.Properties();
            op.properties.expanded = new Expanded();
            op.properties.expanded.expr = new Models.PowerBI.LayoutConfig.Expr();
            op.properties.expanded.expr.Literal = new Models.PowerBI.LayoutConfig.Literal();
            op.properties.expanded.expr.Literal.Value = "false";
            return layoutConfigRoot;
        }

        private VisualContainer GetVisualContainer(Container container, int index)
        {
            var visualContainer = new VisualContainer();

            visualContainer.x = x;
            visualContainer.y = y + sectionHeight;
            visualContainer.z = index;
            visualContainer.width = container.width;
            visualContainer.height = container.height;
            y = y + Convert.ToInt32(visualContainer.height);
            visualContainer.config = JsonConvert.SerializeObject(GetVisualContainerConfigRoot(container, visualContainer), jsonSerializerSettings);
            visualContainer.filters = "[]";
            visualContainer.tabOrder = index;
            return visualContainer;
        }

        private VisualContainerConfigRoot GetVisualContainerConfigRoot(Container container, VisualContainer visualContainer)
        {
            var root = new VisualContainerConfigRoot();
            root.name = Guid.NewGuid().ToString();
              switch (container.visualType)
            {
                case "textbox":
                    var singleVisual = new Models.PowerBI.VisualContainerConfig.SingleVisual();
                    singleVisual.visualType = container.visualType;
                    singleVisual.drillFilterOtherVisuals = true;
                    singleVisual.objects = new Models.PowerBI.VisualContainerConfig.Objects();
                    singleVisual.objects.general = new List<Models.PowerBI.VisualContainerConfig.General>();
                    var general = GetGeneral(container.value);
                    singleVisual.objects.general.Add(general);
                    root.singleVisual = singleVisual;
                    break;
                case "shape":
                    var shapeRoot = new ShapeRoot();
                    root.singleVisual = shapeRoot;
                    shapeRoot.visualType = container.visualType;
                    shapeRoot.drillFilterOtherVisuals = true;
                    shapeRoot.objects = new Models.PowerBI.Shape.Objects();

                    shapeRoot.objects.shape = new List<Shape>();
                    var shape = new Shape();
                    shapeRoot.objects.shape.Add(shape);
                    shape.properties = new Models.PowerBI.Shape.Properties();
                    shape.properties.tileShape = new TileShape();
                    shape.properties.tileShape.expr = new Models.PowerBI.Shape.Expr();
                    shape.properties.tileShape.expr.Literal = new Models.PowerBI.Shape.Literal();
                    shape.properties.tileShape.expr.Literal.Value = "rectangle";

                    shapeRoot.objects.rotation = new List<Rotation>();
                    var rotation = new Rotation();
                    shapeRoot.objects.rotation.Add(rotation);
                    rotation.properties = new Models.PowerBI.Shape.Properties();
                    rotation.properties.shapeAngle = new ShapeAngle();
                    rotation.properties.shapeAngle.expr = new Models.PowerBI.Shape.Expr();
                    rotation.properties.shapeAngle.expr.Literal = new Models.PowerBI.Shape.Literal();
                    rotation.properties.shapeAngle.expr.Literal.Value = "0L";

                    shapeRoot.objects.outline = new List<Outline>();
                    var outline = new Outline();
                    shapeRoot.objects.outline.Add(outline);
                    outline.properties = new Models.PowerBI.Shape.Properties();
                    outline.properties.show = new Show();
                    outline.properties.show.expr = new Models.PowerBI.Shape.Expr();
                    outline.properties.show.expr.Literal = new Models.PowerBI.Shape.Literal();
                    outline.properties.show.expr.Literal.Value = "false";

                    shapeRoot.objects.fill = new List<Fill>();
                    var fill1 = new Fill();
                    shapeRoot.objects.fill.Add(fill1);
                    fill1.properties = new Models.PowerBI.Shape.Properties();
                    fill1.properties.fillColor = new FillColor();
                    fill1.properties.fillColor.solid = new Solid();
                    fill1.properties.fillColor.solid.color = new Color();

                    fill1.properties.fillColor.solid.color.expr = new Models.PowerBI.Shape.Expr();
                    fill1.properties.fillColor.solid.color.expr.ThemeDataColor = new ThemeDataColor();
                    fill1.properties.fillColor.solid.color.expr.ThemeDataColor.ColorId = 0;
                    fill1.properties.fillColor.solid.color.expr.ThemeDataColor.Percent = 0;
                    fill1.selector = new Selector();
                    fill1.selector.id = "default";

                    var fill2 = new Fill();
                    shapeRoot.objects.fill.Add(fill2);
                    fill2.properties = new Models.PowerBI.Shape.Properties();
                    fill2.properties.show = new Show();
                    fill2.properties.show.expr = new Models.PowerBI.Shape.Expr();
                    fill2.properties.show.expr.Literal = new Models.PowerBI.Shape.Literal();
                    fill2.properties.show.expr.Literal.Value = "true";
                    break;
                case "tableEx":
                    var tableEx = GetTableEx(root, container, visualContainer);
                    root.singleVisual = tableEx;
                    break;
                default:
                    break;
            }

           
            root.layouts = new List<Models.PowerBI.VisualContainerConfig.Layout> { };
            root.layouts.Add(GetVisualContainerLayout(visualContainer));
            return root;
        }

        private Models.PowerBI.VisualContainerConfig.General GetGeneral(string value)
        {
            var root = new Models.PowerBI.VisualContainerConfig.General();
            root.properties = new Models.PowerBI.VisualContainerConfig.Properties();
            root.properties.paragraphs = new List<Models.PowerBI.VisualContainerConfig.Paragraph>();
            var paragraph = new Models.PowerBI.VisualContainerConfig.Paragraph();
            root.properties.paragraphs.Add(paragraph);
            paragraph.textRuns = new List<Models.PowerBI.VisualContainerConfig.TextRun>();
            var textRun = new Models.PowerBI.VisualContainerConfig.TextRun();
            textRun.value = value;
            textRun.textStyle = new Models.PowerBI.VisualContainerConfig.TextStyle
            {
                color= "#414fb1",
                fontSize= "16pt",
                fontWeight= "bold"

            };
            paragraph.horizontalTextAlignment = "center";
            paragraph.textRuns.Add(textRun);
           
            return root;
        }

        private Models.PowerBI.VisualContainerConfig.Layout GetVisualContainerLayout(VisualContainer visualContainer)
        {
            var root = new Models.PowerBI.VisualContainerConfig.Layout();
            root.id = 0;
            root.position = new Models.PowerBI.VisualContainerConfig.Position()
            {
                x = visualContainer.x,
                y = visualContainer.y,
                height = visualContainer.height,
                width = visualContainer.width,
                tabOrder = 0
            };
            return root;
        }

        private TableExRoot GetTableEx(VisualContainerConfigRoot root, Container container, VisualContainer visualContainer)
        {
            var tableExRoot = new TableExRoot();
            tableExRoot.visualType = container.visualType;
            tableExRoot.drillFilterOtherVisuals = true;
            tableExRoot.projections = new Projections();
            tableExRoot.projections.Values = new List<Value>();
            foreach (var item in container.columns)
            {
                var val = new Value();
                val.queryRef = $"{container.schema} {container.tableName}.{item}";
                tableExRoot.projections.Values.Add(val);
            }

            tableExRoot.prototypeQuery = new PrototypeQuery();
            tableExRoot.prototypeQuery.Version = 2;
            tableExRoot.prototypeQuery.From = new List<Models.PowerBI.TableEx.From>();
            var from = new Models.PowerBI.TableEx.From();
            from.Name = "h";
            from.Entity = $"{container.schema} {container.tableName}";
            from.Type = 0;
            tableExRoot.prototypeQuery.From.Add(from);
            tableExRoot.prototypeQuery.Select = new List<Models.PowerBI.TableEx.Select>();
            foreach (var item in container.columns)
            {
                var select = new Models.PowerBI.TableEx.Select();
                select.Column = new Models.PowerBI.TableEx.Column();
                select.Column.Expression = new Models.PowerBI.TableEx.Expression();
                select.Column.Expression.SourceRef = new Models.PowerBI.TableEx.SourceRef();
                select.Column.Expression.SourceRef.Source = "h";
                select.Column.Property = item;
                select.Name = $"{container.schema} {container.tableName}.{item}";
                select.NativeReferenceName = item;
                tableExRoot.prototypeQuery.Select.Add(select);
            }

            visualContainer.query = JsonConvert.SerializeObject(GetQuery(root, container, visualContainer), jsonSerializerSettings);

            return tableExRoot;
        }

        private QueryRoot GetQuery(VisualContainerConfigRoot root, Container container, VisualContainer visualContainer)
        {
            var queryRoot = new QueryRoot();
            queryRoot.Commands = new List<Command>();
            var comm = new Command();
            queryRoot.Commands.Add(comm);
            comm.SemanticQueryDataShapeCommand = new SemanticQueryDataShapeCommand();
            comm.SemanticQueryDataShapeCommand.Query = new Query();
            comm.SemanticQueryDataShapeCommand.Query.Version = 2;
            comm.SemanticQueryDataShapeCommand.Query.From = new List<Models.PowerBI.Query.From>();
            var from = new Models.PowerBI.Query.From();
            from.Name = "h";
            from.Entity = $"{container.schema} {container.tableName}";
            from.Type = 0;
            comm.SemanticQueryDataShapeCommand.Query.From.Add(from);

            comm.SemanticQueryDataShapeCommand.Query.Select = new List<Models.PowerBI.Query.Select>();
            foreach (var item in container.columns)
            {
                var select = new Models.PowerBI.Query.Select();
                select.Column = new Models.PowerBI.Query.Column();
                select.Column.Expression = new Models.PowerBI.Query.Expression();
                select.Column.Expression.SourceRef = new Models.PowerBI.Query.SourceRef();
                select.Column.Expression.SourceRef.Source = "h";
                select.Column.Property = item;
                select.Name = $"{container.schema} {container.tableName}.{item}";
                select.NativeReferenceName = item;
                comm.SemanticQueryDataShapeCommand.Query.Select.Add(select);
            }
            return queryRoot;
        }
    }
}
