using System;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace content_moderator_quickstart
{
    class Program
    {

        
        
        // Create an image review client
      //  ContentModeratorClient clientImage = Authenticate(SubscriptionKey, Endpoint);
        // Create a text review client
        //ContentModeratorClient clientText = Authenticate(SubscriptionKey, Endpoint);
        // Create a human reviews client
       // ContentModeratorClient clientReviews = Authenticate(SubscriptionKey, Endpoint);
        
        // TEXT MODERATION
        // Name of the file that contains text
        private static readonly string TextFile = "TextFile.txt";
        // The name of the file to contain the output from the evaluation.
        private static string TextOutputFile = "TextModerationOutput.txt";
        static void Main(string[] args)
        {
            // Your Content Moderator subscription key is found in your Azure portal resource on the 'Keys' page. Add to your environment variables.
            string CONTENT_MODERATOR_SUBSCRIPTION_KEY="dbf8e3ad36fb4145aba2acfba0c91f82";
            string SubscriptionKey = Environment.GetEnvironmentVariable(CONTENT_MODERATOR_SUBSCRIPTION_KEY);
                    // Base endpoint URL. Add this to your environment variables. Found on 'Overview' page in Azure resource. For example: https://westus.api.cognitive.microsoft.com


            string CONTENT_MODERATOR_ENDPOINT="https://yogicontentmoderator.cognitiveservices.azure.com/";
            string Endpoint = Environment.GetEnvironmentVariable(CONTENT_MODERATOR_ENDPOINT);
            Console.WriteLine("Hello World!");
            // Moderate text from text in a file
            Console.WriteLine(Endpoint);
            var clientText = Authenticate(CONTENT_MODERATOR_SUBSCRIPTION_KEY,CONTENT_MODERATOR_ENDPOINT);
            ModerateText(clientText, TextFile, TextOutputFile);
        }


        /*
        * TEXT MODERATION
        * This example moderates text from file.
        */
        public static void ModerateText(ContentModeratorClient client, string inputFile, string outputFile)
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("TEXT MODERATION");
            Console.WriteLine();
            // Load the input text.
            string text = File.ReadAllText(inputFile);

            // Remove carriage returns
            text = text.Replace(Environment.NewLine, " ");
            // Convert string to a byte[], then into a stream (for parameter in ScreenText()).
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            MemoryStream stream = new MemoryStream(textBytes);

            Console.WriteLine("Screening {0}...", inputFile);
            // Format text

            // Save the moderation results to a file.
            using (StreamWriter outputWriter = new StreamWriter(outputFile, false))
            {
                using (client)
                {
                    // Screen the input text: check for profanity, classify the text into three categories,
                    // do autocorrect text, and check for personally identifying information (PII)
                    outputWriter.WriteLine("Autocorrect typos, check for matching terms, PII, and classify.");

                    // Moderate the text
                    var screenResult = client.TextModeration.ScreenText("text/plain", stream, "eng", true, true, null, true);
                    outputWriter.WriteLine(JsonConvert.SerializeObject(screenResult, Formatting.Indented));
                }

                outputWriter.Flush();
                outputWriter.Close();
            }

            Console.WriteLine("Results written to {0}", outputFile);
            Console.WriteLine();
        }
        
       public static ContentModeratorClient Authenticate(string _SubscriptionKey, string _Endpoint)
        {
            // Create and initialize an instance of the Content Moderator API wrapper.
            ContentModeratorClient Authenticate_client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(_SubscriptionKey));

            Authenticate_client.Endpoint = _Endpoint;
            return Authenticate_client;
        }
    }
}
