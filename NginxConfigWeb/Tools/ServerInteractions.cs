using Firebase.Database;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using NginxConfigWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NginxConfigWeb.Tools
{
    public static class ShellHelper
    {
        public static string Bash(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }

    public static class ServerInteractions
    {
        public static string firebaseRootUrl = "https://nginxconfiguration.firebaseio.com/";
        public static string firebaseToken;


        public static FirebaseClient firebase = new FirebaseClient(firebaseRootUrl, new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(firebaseToken) });

        public static async Task UpdateConfig()
        {
            string CopyFileToLocation = "/usr/local/nginx/conf/nginx.conf";

            string concat = "\n\nrtmp { \n" +
                "\tserver { \n" +
                "\t\tlisten 1935;\n" +
                "\t\tchunk_size 4096;\n\n";

            string SourceFile = "./nginx.conf.base";
            string newFile = "./nginx.conf.new";

            var FbChild = firebase.Child("applications");

            var FbObservable = FbChild.AsObservable<RtmpApplications>();

            if (System.IO.File.Exists(newFile))
                System.IO.File.Delete(newFile);

            System.IO.File.Copy(SourceFile, newFile);

            HttpClient webClient = new HttpClient();

            var apps = await FbChild.OnceAsync<RtmpApplications>();

            foreach (var app in apps)
            {
                string newCat = string.Empty;
                string url = $"{firebaseRootUrl}/applications/{app.Key}/.json?auth={firebaseToken}";

                HttpResponseMessage webResponse = await webClient.GetAsync(url);
                string jsonContent = await webResponse.Content.ReadAsStringAsync();
                JObject jObject = JObject.Parse(jsonContent);

                newCat += "\t\tapplication " + app.Key + " {\n";

                foreach (var obj in jObject)
                {
                    if (obj.Key == "push")
                    {
                        if (obj.Value != null && (obj.Value).HasValues == true)
                        {
                            foreach (var subObj in obj.Value.Children())
                            {
                                if (subObj.Path != "path[0]")
                                {
                                    var urls = subObj.Value<string>();
                                    if (urls != null)
                                        newCat += "\t\t\tpush: " + urls + ";\n";
                                }
                            }
                        }
                    }
                    else
                    {
                        newCat += "\t\t\t" + obj.Key + ": " + obj.Value + ";\n";
                    }
                }

                newCat += "\t\t} \n\n";

                concat += newCat;
            }

            concat += "\t} \n" +
                "} \n";


            // Write to File //
            using (System.IO.StreamWriter sw = System.IO.File.AppendText(newFile))
            {
                sw.Write(concat);
            }

            if (System.IO.File.Exists(CopyFileToLocation))
                System.IO.File.Delete(CopyFileToLocation);

            System.IO.File.Copy(newFile, CopyFileToLocation);
        }

        public static string StartServer()
        {
            return "/usr/local/nginx/sbin/nginx".Bash();
        }

        public static string StopServer()
        {
            return "/usr/local/nginx/sbin/nginx -s stop".Bash();
        }
    }
}
