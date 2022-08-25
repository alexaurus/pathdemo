using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Diagnostics;
using pathdemo.Models;
using System.IO.Compression;

namespace pathdemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        
        [HttpGet]
        public void Get()
        {
            GenerateFiles();
            CreateZip();

        }

        public void GenerateFiles()
        {
            string tempPath = Path.Combine(Path.GetTempPath() + "Result");
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();


                string command = "java -jar ./JAR/swagger-codegen-cli.jar generate -i ./JAR/Sample.yml -l aspnetcore -o" + tempPath;

                process.StandardInput.WriteLine(command);
                process.WaitForExit(5000);
                process.StandardInput.Flush();
                process.StandardInput.Close();
                process.Close();
            }
        }

        public void CreateZip()
        {
            ZipFile.CreateFromDirectory(Path.Combine(Path.GetTempPath() + "Result"), Path.Combine(Path.GetTempPath() + "ZipResult.zip"));
        }
    }
}
