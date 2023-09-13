namespace Sonata.Assets.Scanner.Core
{
    /// <summary>
    /// Generic Scanner interface
    /// </summary>
    /// <typeparam name="Tin">Inputs for the scanner</typeparam>
    /// <typeparam name="Tout">Output of the scanner</typeparam>
    public interface IScanner<Tin, Tout>
    {
        /// <summary>
        /// Uses input from the caller and runs the scanning process, populates the output 
        /// </summary>
        /// <param name="input">input for scanning</param>
        /// <returns>Scanning output</returns>
        public Tout Scan(Tin input);

    }
}