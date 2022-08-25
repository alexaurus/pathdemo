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
        public ActionResult<FileDTO> Get()
        {

            string outputname = "Zip" + DateTime.Now.ToFileTime().ToString() + ".zip";
            GenerateFiles();
            CreateZip(outputname);

            var fi =  GetFile(outputname);

            return File(fi.Data, fi.FileType, fi.FileName);

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

        public void CreateZip(string outputname)
        {
            ZipFile.CreateFromDirectory(Path.Combine(Path.GetTempPath() + "Result"), Path.Combine(Path.GetTempPath() + outputname));
        }

        public FileDTO GetFile(string outputname)
        {
            return new FileDTO
            {
                FileName = "ZipReturned.zip",
                Data = System.IO.File.ReadAllBytes(Path.Combine(Path.GetTempPath() + outputname)),
                FileType = "application/zip"
            };
        }
    }
}
