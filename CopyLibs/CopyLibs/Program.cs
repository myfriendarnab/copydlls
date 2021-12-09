using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magnum.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace CopyLibs
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("settings.json").Build();

            /*
            var binder = ConfigurationBinderFactory.New(ca =>
            {
                ca.AddJsonFile("settings.json");
            });
            var settings = ConfigurationBinder.Bind<>()
            */
            
            var settings = config.GetSection("Settings").Get<Settings>();

            //var basepath = "C:\\Origin-OrderManagementWarehouse\\Origin-OrderWarehouse\\Release3.0\\";
            //var debugPath = "bin\\Debug\\netcoreapp3.1\\";
            //var frameworkPath = "Framework\\";
            //var fwprojs = new[] { "Maersk.Framework.Common", "Maersk.Framework.Logger", "Maersk.Framework.Repository" };
            //var svcType = "Warehouse";
            //var exclude = new[] { ".sonarlint", ".vs", ".vscode", ".git", "Maersk.Warehouse.Infrastructure", "packages", ".idea" };
            
            var basepath = settings.basePath;
            var debugPath = settings.debugPath;
            var frameworkPath = settings.frameworkPath;
            var fwprojs = settings.fwprojs;
            var svcType = settings.svcType;
            var exclude = settings.exclude;
            var filesToCopy = new List<string>();
            var filesToOverride = new List<string>();

            try
            {
                foreach (var item in fwprojs)
                {
                    var fwdir = basepath + frameworkPath + item + "\\" + debugPath;
                    var dllFile = Directory.GetFiles(fwdir, $"{item}.dll", SearchOption.TopDirectoryOnly).FirstOrDefault();
                    var pdbFile = Directory.GetFiles(fwdir, $"{item}.pdb", SearchOption.TopDirectoryOnly).FirstOrDefault();
                    var depsFile = Directory.GetFiles(fwdir, $"{item}.deps.json", SearchOption.TopDirectoryOnly).FirstOrDefault();
                    filesToCopy.AddRange(new[] { dllFile, pdbFile, depsFile });
                    filesToOverride.AddRange(new[] { $"{item}.dll", $"{item}.pdb", $"{item}.deps.json" });
                }

                var scanbasePath = basepath + svcType;
                var excludePaths = exclude.ToList().Select(x => scanbasePath + "\\" + x);
                var scanPaths = Directory
                                                .EnumerateDirectories(scanbasePath)
                                                .Except(excludePaths)
                                                .Where(x => !x.EndsWith(".Tests"));

                foreach (var path in scanPaths)
                {
                    foreach (var copyfile in filesToCopy)
                    {
                        var fname = path + "\\" + debugPath + filesToOverride.FirstOrDefault(x => copyfile.EndsWith(x));
                        File.Copy(copyfile, fname, true);
                        Console.WriteLine($"successfully copied:\t {copyfile} \t >=======>> \t {fname}");
                        Console.WriteLine("\n \n");
                    }
                }

                var workingdir = scanbasePath + $"\\Maersk.{svcType}.MicroService\\" + debugPath;
                var executable = "Maersk." + svcType + ".MicroService.exe";
                var process = new System.Diagnostics.Process();
                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                    FileName = executable,
                    WorkingDirectory = workingdir
                };
                process.StartInfo = startInfo;
                process.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
