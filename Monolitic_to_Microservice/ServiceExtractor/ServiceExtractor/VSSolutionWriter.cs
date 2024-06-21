namespace Service.Extractor.Console
{
    public class VSSolutionWriter
    {
        private VSSolution _vsSolution;

        public VSSolutionWriter(VSSolution vsSolution)
        {
            _vsSolution = vsSolution;
        }

        public void Write(string path)
        {
            File.WriteAllText(path, _vsSolution.ToString());
        }
    }
}
