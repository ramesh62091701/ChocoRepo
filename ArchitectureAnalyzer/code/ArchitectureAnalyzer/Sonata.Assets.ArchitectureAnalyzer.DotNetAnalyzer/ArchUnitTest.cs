using Newtonsoft.Json;
using NUnit.Framework;
using ArchUnitNET.NUnit;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using Sonata.Assets.ArchitectureAnalyzer.DotNetAnalyzer.Models;

namespace Sonata.Assets.ArchitectureAnalyzer.DotNetAnalyzer
{

    [TestFixture]
    public class ArchUnitTest
    {
        [Test]
        public void TestArchitectureWithJson()
        {
            // Load the JSON file containing the architecture rules
            string json = File.ReadAllText("architecture-rules.json");
            var rules = JsonConvert.DeserializeObject<List<ArchitectureRule>>(json);

            // Iterate over the rules and check them
            foreach (var rule in rules)
            {
                var classes = ArchUnitNET.Fluent.Syntax.Elements.Types.Classes.That().ResideInNamespace(rule.Namespace).And().HaveFullNameMatching(rule.Assembly);
                var archRule = classes;

                if (rule.Type == "ShouldBeLocatedInAssembly")
                {
                    archRule = archRule.Should().HaveFullNameMatching(rule.Value);
                }
                else if (rule.Type == "ShouldBeAssignableTo")
                {
                    archRule = archRule.Should().BeAssignableTo(rule.Value);
                }
                // Add more conditions for different rules
                archRule.Check();
            }
        }

    }
}
