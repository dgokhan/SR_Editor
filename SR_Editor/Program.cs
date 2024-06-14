using SR_Editor.Core;
using SR_Editor.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using SR_Editor.Modules.Server.Query;

namespace SR_Editor
{
    static class Program
    {
        // http://msdn.microsoft.com/en-us/library/ms681944(VS.85).aspx
        /// <summary>
        /// Allocates a new console for the calling process.
        /// </summary>
        /// <returns>nonzero if the function succeeds; otherwise, zero.</returns>
        /// <remarks>
        /// A process can be associated with only one console,
        /// so the function fails if the calling process already has a console.
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int AllocConsole();

        // http://msdn.microsoft.com/en-us/library/ms683150(VS.85).aspx
        /// <summary>
        /// Detaches the calling process from its console.
        /// </summary>
        /// <returns>nonzero if the function succeeds; otherwise, zero.</returns>
        /// <remarks>
        /// If the calling process is not already attached to a console,
        /// the error code returned is ERROR_INVALID_PARAMETER (87).
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int FreeConsole();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //AllocConsole();
            //var client = new SocketClient("79.137.109.177", 15880);
            //client.Connect();
            EditorApplication.EditorApplication.InitApplication();

            EditorApplication.EditorApplication.Start(true);
            //Console.ReadLine();
            //FreeConsole();
        }

        public static void ShowDebugWindow()
        {

            //UtilParameters pFormParams = new UtilParameters();
            //pFormParams.Add("ShardId", 1);
            //pFormParams.Add("CharId", 11412);
            //EditorApplication.EditorApplication.Module.CharacterModule.CharacterActionModule.Character.Show(pFormParams);
        }
    }
}
