using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Azure.WebJobs;
using System.IO;

namespace AIMonitor
{
    public static class FirebaseMessage
    {
        public static void SendAsync(string title, string body, ExecutionContext context)
        {
            FirebaseApp firebaseApp;

            if (FirebaseApp.DefaultInstance == null)
            {
                var accountSettingsFile = Path.Combine(context.FunctionDirectory, "..\\ai-alerts-firebase-adminsdk.json");
                var appOptions = new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(accountSettingsFile)
                };
                firebaseApp = FirebaseApp.Create(appOptions);
            }
            else
            {
                firebaseApp = FirebaseApp.DefaultInstance;
            }

            var firebaseMessaging = FirebaseMessaging.GetMessaging(firebaseApp);

            var message = new Message
            {
                Topic = "All",
                Notification = new Notification
                {
                    Title = title,
                    Body = body,
                }
            };

            firebaseMessaging.SendAsync(message);
        }
    }
}