#region Using Statements
using System;
#endregion

namespace EmberEngine
{
    #if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Samples.SampleExplorer.SampleExplorer game = new Samples.SampleExplorer.SampleExplorer())
            {
                game.Run();
            }
        }
    }
    #endif
}
