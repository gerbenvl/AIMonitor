# AI Monitor

## Intro
Azure Function that can monitor Azure Application Insights instances. If an exception is found a message will be sent using FireBase.

## Run locally

### Local.settings.json

Create a local.settings.json like this:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "AiInstancesToMonitor": "[
    {
      \"name\": \"Some application\",
	  \"applicationId\": \"your application insights application id\",
      \"apiKey\": \"your application insights application api key\"
    }
    ]"
  }
}
```
In "AiInstancesToMonitor" you can put an array of applications to monitor.

Sorry that "AiInstancesToMonitor" is saved as a big string. Unfortunately json arrays are not properly supported in this file.

### FireBase service account

Add a file named "ai-alerts-firebase-adminsdk.json" with your google FireBase serviceaccount in it.

## Deploy
Deploy the function to Azure.

Add the setting named "AiInstancesToMonitor" to your "Application settings". Here you can add it as a normal json array. For example: 

```json
[
   {
      "name":"name of your application",
      "applicationId":"your application insights application id",
      "apiKey":"your application insights application api key"
   }
]
```
