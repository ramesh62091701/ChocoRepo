using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBIApp.Models.PowerBI.DataTransform
{
    public class Column
    {
        public Expression Expression { get; set; }
        public string Property { get; set; }
    }

    public class DataRole
    {
        public string Name { get; set; }
        public int Projection { get; set; }
        public bool isActive { get; set; }
    }

    public class Expr
    {
        public Column Column { get; set; }
    }

    public class Expression
    {
        public Column Column { get; set; }
    }

    public class Expression2
    {
        public SourceRef SourceRef { get; set; }
    }

    public class Filter
    {
        public int type { get; set; }
        public Expression expression { get; set; }
    }

    public class ProjectionOrdering
    {
        public List<int> Values { get; set; }
    }

    public class QueryMetadata
    {
        public List<Select> Select { get; set; }
        public List<Filter> Filters { get; set; }
    }

    public class Roles
    {
        public bool Values { get; set; }
    }

    public class DataTransformRoot
    {
        public ProjectionOrdering projectionOrdering { get; set; }
        public QueryMetadata queryMetadata { get; set; }
        public List<VisualElement> visualElements { get; set; }
        public List<Select> selects { get; set; }
    }

    public class Select
    {
        public string Restatement { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string Format { get; set; }
    }

    public class Select2
    {
        public string displayName { get; set; }
        public string format { get; set; }
        public string queryName { get; set; }
        public Roles roles { get; set; }
        public Type type { get; set; }
        public Expr expr { get; set; }
    }

    public class SourceRef
    {
        public string Entity { get; set; }
    }

    public class Type
    {
        public object category { get; set; }
        public int underlyingType { get; set; }
    }

    public class VisualElement
    {
        public List<DataRole> DataRoles { get; set; }
    }


}
