using DocumentFormat.OpenXml.Drawing;
using Newtonsoft.Json;
using SAPBOAnalysis.Models;
using SAPBOAnalysis.Models.QueryHelper;
using SAPBOAnalysis.Models.Universe;
using SAPBOAnalysis.Models.UniverseDetails;
using System;

namespace SAPBOAnalysis
{
    public class UniverseAnalyzer
    {
        HttpHelper httpHelper = new HttpHelper();
        TokenHelper tokenHelper = new TokenHelper();
        int apiLimit = ConfigHelper.apiLimit;


        private Configuration config;
        public UniverseAnalyzer()
        {
            config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("appconfig.json"))!;

        }

        public async Task<List<Models.Universe.Universe>> GetUniverses()
        {
            var getToken = await tokenHelper.GetToken();
            var universesList = new List<Models.Universe.Universe>();
            var offset = 0;
            int resultCount;
            do
            {
                string url = $"{config.sapConnection.server}/raylight/v1/universes?offset={offset}&limit={apiLimit}";
                var universesResponse = await httpHelper.ExecuteGet<UniverseRoot>("application/json", getToken, url);
                var universes = universesResponse.universes.universe;
                resultCount = universes.Count;
                universesList.AddRange(universes);
                offset += apiLimit;
            } while (resultCount == apiLimit);
            return universesList;
        }

        public async Task<UniverseSchema> GetUniversClasses(string id)
        {
            var getToken = await tokenHelper.GetToken();
            var url = $"{config.sapConnection.server}/raylight/v1/universes/{id}";
            var universeDetailsRoot = await httpHelper.ExecuteGet<UniverseDetailsRoot>("application/json", getToken, url);
            return GetUniverseClassesAndObjects(universeDetailsRoot);
        }

        public async Task<Dictionary<string, List<object>>> Analyze()
        {
            var listUniverseDetailsRoot = new Dictionary<int, UniverseDetailsRoot>();
            var getToken = await tokenHelper.GetToken();
            var universeAnalysisList = new List<object>();
            var universeClassesList = new List<object>();
            var universeClassesObjectsList = new List<object>();

            //Get Universes
            var offset = 0;
            var resultCount = 0;

            do
            {
                string url = $"{config.sapConnection.server}/raylight/v1/universes?offset={offset}&limit={apiLimit}";
                var universesResponse = await httpHelper.ExecuteGet<UniverseRoot>("application/json", getToken, url);
                var universes = universesResponse.universes.universe;
                resultCount = universes.Count;

                foreach (var item in universes)
                {
                    var universeAnalysis = new UniverseAnalysis();
                    universeAnalysis.Id = item.id;
                    universeAnalysis.Name = item.name;
                    universeAnalysis.FolderId = item.folderId;
                    var folderNames = new List<string>();
                    var objects = new List<string>();
                    var calculatedObjects = new List<string>();

                    url = $"{config.sapConnection.server}/raylight/v1/universes/{item.id}";
                    var universeDetailsRoot = await httpHelper.ExecuteGet<UniverseDetailsRoot>("application/json", getToken, url);
                    listUniverseDetailsRoot.Add(item.id, universeDetailsRoot);

                    if (universeDetailsRoot?.universe?.outline?.folder != null)
                    {
                        foreach (var folder in universeDetailsRoot.universe.outline.folder)
                        {
                            if (folder.item != null)
                            {
                                folderNames.Add(folder.name);
                                universeClassesList.Add(new UniverseClasses { UniverseId = item.id, Folder = folder.name, FolderId = folder.id });
                                foreach (var obj in folder.item)
                                {
                                    objects.Add($"{folder.name}.{obj.name}");
                                    var universeClassObject = new UniverseClassesObjects
                                    {
                                        UniverseId = item.id,
                                        FolderId = folder.id,
                                        FolderName = folder.name,
                                        ObjectId = obj.id,
                                        ObjectName = obj.name,
                                        IsCalculated = false,
                                        Type = obj.type,
                                        DataType = obj.dataType
                                    };


                                    if (obj.aggregationFunction != null)
                                    {
                                        calculatedObjects.Add($"{folder.name}.{obj.name}");
                                        universeClassObject.IsCalculated = true;
                                    }

                                    universeClassesObjectsList.Add(universeClassObject);
                                }
                            }
                        }
                    }
                    universeAnalysis.FolderCount = folderNames.Count;
                    universeAnalysis.ObjectsCount = objects.Count;
                    universeAnalysis.CalculatedObjectsCount = calculatedObjects.Count;
                    universeAnalysis.FolderNames = string.Join(';', folderNames);
                    universeAnalysis.Objects = string.Join(';', objects);
                    universeAnalysis.CalculatedObjects = string.Join(';', calculatedObjects);
                    universeAnalysis.Type = item.type;

                    /*url = $"{config.sapConnection.server}/raylight/v1/universes/{item.id}/businessviews";
                    var universeBussinesViewRoot = await httpHelper.ExecuteGet<UniverseBussinesViewRoot>("application/json", getToken, url);
                    listUniverseBussinesViewRoot.Add(item.id, universeBussinesViewRoot);*/

                    universeAnalysisList.Add(universeAnalysis);
                }

                offset += apiLimit;
            } while (resultCount == apiLimit);

            var excelWorksheets = new Dictionary<string, List<object>>();
            excelWorksheets.Add("Universe", universeAnalysisList);
            excelWorksheets.Add("Universe Classess", universeClassesList);
            excelWorksheets.Add("Universe Objects", universeClassesObjectsList);

            Logger.Log("Universe completed.");

            return excelWorksheets;
        }

        private UniverseSchema GetUniverseClassesAndObjects(UniverseDetailsRoot universeDetailsRoot)
        {
            var schema = new UniverseSchema()
            {
                Id = universeDetailsRoot.universe.id,
                Name = universeDetailsRoot.universe.name,
                Type = universeDetailsRoot.universe.type,
            };
            var details = new List<QueryTableDetails>();
            schema.queryDetails = details;
            if (universeDetailsRoot?.universe?.outline?.folder != null)
            {

                foreach (var folder in universeDetailsRoot.universe.outline.folder)
                {
                    var detail = new QueryTableDetails();
                    detail.TableName = folder.name;
                    detail.Columns = new List<string>();
                    if (folder.item != null)
                    {
                        foreach (var obj in folder.item)
                        {
                            if (obj.type == "Dimension")
                            {
                                detail.Columns.Add(obj.name);
                            }
                        }
                    }
                    details.Add(detail);
                }
            }
            return schema;
        }
    }
}
