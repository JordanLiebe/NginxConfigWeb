using Firebase.Database;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NginxConfigWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NginxConfigWeb.Tools
{
    public static class FirebaseInteractions
    {
        public static string firebaseRootUrl = "https://nginxconfiguration.firebaseio.com/";

        public static async Task<List<RtmpApplications>> GetApplicationsAsync(string Token)
        {
            FirebaseClient firebase = new FirebaseClient(firebaseRootUrl, new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(Token) });
            var applications = firebase.Child("applications");
            var apps = await applications.OnceAsync<RtmpApplications>();
            List<RtmpApplications> RtmpApps = new List<RtmpApplications>();

            foreach(var app in apps)
            {
                RtmpApps.Add(new RtmpApplications
                {
                    name = app.Key,
                    live = app.Object.live,
                    push_urls = app.Object.push_urls,
                    record = app.Object.record
                });
            }

            return RtmpApps;
        }

        public static async Task<RtmpApplications> GetApplicationByIdAsync(string Id, string Token)
        {
            FirebaseClient firebase = new FirebaseClient(firebaseRootUrl, new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(Token) });
            var applications = firebase.Child("applications");
            var apps = await applications.OnceAsync<RtmpApplications>();
            RtmpApplications AppPointer = null;

            foreach(var app in apps)
            {
                if (app.Key == Id)
                {
                    app.Object.name = app.Key;

                    List<string> pushUrls = new List<string>(app.Object.push_urls);

                    for (int i = pushUrls.Count; i < 5; i++)
                    {
                        pushUrls.Add(string.Empty);
                    }

                    app.Object.push_urls = pushUrls.ToArray();

                    AppPointer = app.Object;
                }
            }

            return AppPointer;
        }

        public static async Task<bool> CreateApplication(RtmpApplications app, string Token)
        {
            FirebaseClient firebase = new FirebaseClient(firebaseRootUrl, new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(Token) });
            var applications = firebase.Child($"applications/{app.name}");

            try
            {
                app.name = null;
                string json = JsonConvert.SerializeObject(app);
                await applications.PutAsync(json);
            }
            catch(Exception)
            {
                return false;
            }
            
            return true;
        }

        public static async Task<bool> UpdateApplication(RtmpApplications app, string Token)
        {
            FirebaseClient firebase = new FirebaseClient(firebaseRootUrl, new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(Token) });
            var applications = firebase.Child($"applications/{app.name}");

            try
            {
                app.name = null;
                string json = JsonConvert.SerializeObject(app);
                await applications.PutAsync(json);
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        public static async Task<bool> RemoveApplication(RtmpApplications app, string Token)
        {
            FirebaseClient firebase = new FirebaseClient(firebaseRootUrl, new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(Token) });
            var applications = firebase.Child($"applications/{app.name}");

            try
            {
                await applications.DeleteAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
