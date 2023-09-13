using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBIApp.Models.PowerBI.Query
{
    public class Binding
    {
        public Primary Primary { get; set; }
        public DataReduction DataReduction { get; set; }
        public int Version { get; set; }
    }

    public class Column
    {
        public Expression Expression { get; set; }
        public string Property { get; set; }
    }

    public class Command
    {
        public SemanticQueryDataShapeCommand SemanticQueryDataShapeCommand { get; set; }
    }

    public class DataReduction
    {
        public int DataVolume { get; set; }
        public Primary Primary { get; set; }
    }

    public class Expression
    {
        public SourceRef SourceRef { get; set; }
    }

    public class From
    {
        public string Name { get; set; }
        public string Entity { get; set; }
        public int Type { get; set; }
    }

    public class Grouping
    {
        public List<int> Projections { get; set; }
        public int Subtotal { get; set; }
    }

    public class Primary
    {
        public List<Grouping> Groupings { get; set; }
        public Window Window { get; set; }
    }

    public class Query
    {
        public int Version { get; set; }
        public List<From> From { get; set; }
        public List<Select> Select { get; set; }
    }

    public class QueryRoot
    {
        public List<Command> Commands { get; set; }
    }

    public class Select
    {
        public Column Column { get; set; }
        public string Name { get; set; }
        public string NativeReferenceName { get; set; }
    }

    public class SemanticQueryDataShapeCommand
    {
        public Query Query { get; set; }
        public Binding Binding { get; set; }
        public int ExecutionMetricsKind { get; set; }
    }

    public class SourceRef
    {
        public string Source { get; set; }
    }

    public class Window
    {
        public int Count { get; set; }
    }


}
