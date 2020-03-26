using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace AIMonitor
{
    public static class FirebaseMessage
    {
        public static void SendAsync(string title, string body)
        {
            var appOptions = new AppOptions()
            {
                Credential = GoogleCredential.FromFile("ai-alerts-firebase-adminsdk.json")
            };

            FirebaseApp firebaseApp;

            if (FirebaseApp.DefaultInstance == null)
            {
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