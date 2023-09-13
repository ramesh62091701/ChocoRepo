using Microsoft.VisualStudio.TestTools.UnitTesting;
using SAPBOAnalysis;
using SAPBOAnalysis.Models.QueryHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBOAnalysis.Tests
{
    [TestClass()]
    public class ReportAnalyzerTests
    {
        [TestMethod()]
        public async Task GetDataSchemaForDocumentTest()
        {
            var analyzer = new ReportAnalyzer();
            var documentId = "5903";
            var result = await analyzer.GetDataSchemaForDocument(documentId);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DocumentDataSchema));
        }
    }
}