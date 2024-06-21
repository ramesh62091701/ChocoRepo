using Service.Extractor.Console;

namespace Service.Extractor.Tests
{
    public class VSSolutionReaderTest
    {
        [Fact]
        public void ShouldReadVSSolutionProperly()
        {
            var path = "D:\\Projects\\Widex\\wsa.ids.services\\src\\services\\Flextool.UserScoping.Service\\Microservice.Flextool.UserScoping.sln";
            VSSolutionReader reader = new VSSolutionReader(path);
        }

        [Fact]
        public void ShouldWriteVSSolutionProperly()
        {
            var path = "D:\\Projects\\Widex\\wsa.ids.services\\src\\services\\Flextool.UserScoping.Service\\Microservice.Flextool.UserScoping.sln";
            VSSolutionReader reader = new VSSolutionReader(path);

            VSSolutionWriter write = new VSSolutionWriter(reader.VSSolution);
            write.Write("sample.sln");
        }
    }
}