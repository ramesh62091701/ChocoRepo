namespace Sonata.Assets.ArchitectureAnalyzer.Core
{
    public interface IAnalyzer<TIn, TOut>
    {
        TOut Analyze(TIn input);
    }
}