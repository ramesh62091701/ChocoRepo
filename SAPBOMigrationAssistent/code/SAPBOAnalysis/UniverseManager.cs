using SAPBOAnalysis.Models.QueryHelper;
using SAPBOAnalysis.Models;

namespace SAPBOAnalysis
{
    public class UniverseManager
    {
        public async Task<UniverseSchema> GetUniverseClassesAndObjects(string universeId)
        {
            UniverseAnalyzer analyser = new UniverseAnalyzer();
            var uni = await analyser.GetUniversClasses(universeId);
            var schema = new UniverseSchema()
            {
                Id = uni.Id,
                Name = uni.Name,
                Type = uni.Type,
            };
            var details = new List<QueryTableDetails>();
            schema.queryDetails = details;
            var relations = new List<RelationShip>();
            schema.RelationShips = relations;
            var fileName = Path.GetFileNameWithoutExtension(uni.Name);
            var tables = SpreadsheetReader.Read($"./Universes/{fileName}.xlsx", "Tables", "E");
            for (var i = 1; i < tables.Count; i++)
            {
                var folder = tables[i];
                var detail = new QueryTableDetails();
                detail.TableName = folder;
                detail.Columns = new List<string>();
                detail.Measures = new List<Measures>();
                details.Add(detail);
            }

            var expressions = SpreadsheetReader.Read($"./Universes/{fileName}.xlsx", "Joins", "H");
            for (var i = 1; i < expressions.Count; i++)
            {
                
                var expression = expressions[i];
                var parts = expression.Split("=");
                if(parts.Length != 2 || i > 6)
                {
                    continue;
                }
                var partsFrom = parts[0].Split(".");
                var partsTo = parts[1].Split(".");
                var relation = new RelationShip();
                relation.FromTable = partsFrom[0];
                relation.ToTable = partsTo[0];
                relation.FromColumn = partsFrom[1];
                relation.ToColumn = partsTo[1];
                relations.Add(relation);
            }

            var measureNames = SpreadsheetReader.Read($"./Universes/{fileName}.xlsx", "Objects", "F");
            var measureTypes = SpreadsheetReader.Read($"./Universes/{fileName}.xlsx", "Objects", "H");
            var measureFunctions = SpreadsheetReader.Read($"./Universes/{fileName}.xlsx", "Objects", "J");


            for (var i = 1; i < measureFunctions.Count; i++)
            {

                var measureFunction = measureFunctions[i];
                if (!measureFunction.StartsWith("sum", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                var measure = new Measures();
                measure.Name = measureNames[i];
                measure.Format = string.Empty;
                measure.Expression = measureFunction;
                var table = measureFunction.Split(".");

                var tab = details.First(d => table[0].Contains(d.TableName));
                tab.Measures.Add(measure);
            }

            return schema;
        }
    }
}
