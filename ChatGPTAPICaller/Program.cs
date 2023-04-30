using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ChatGPTDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Boolean run = true;
            List<dynamic> messages = new List<dynamic> {};
            while (run) {
                //await generateText(messages);
                await generateImage();
            }
        }

        static async Task generateText(List<dynamic> messages) {
                Console.WriteLine("Enter some text for generating text:"); // Prompts the user to enter some text
                string userInput = Console.ReadLine(); // Reads the user input from the console and stores it in a string variable
                Console.WriteLine("You entered: " + userInput); // Prints the user input to the console

                // Replace the following variables with your own values
                string apiKey = "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";
                string prompt = "Hello, ChatGPT!";
                messages.Add(
                    new {role = "system", content = userInput});
                String model1 = "gpt-3.5-turbo";
                int maxTokens = 50;

                // Create an HttpClient instance
                HttpClient httpClient = new HttpClient();

                // Set the endpoint URL
                string endpointUrl = $"https://api.openai.com/v1/chat/completions";

                // Create a request object with the required parameters
                var requestBody = new {
                    messages =  messages,
                    //prompt = prompt,
                    model = model1,
                    max_tokens = maxTokens
                };
                var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");

                // Add the API key to the request headers
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                // Make the POST request and get the response
                HttpResponseMessage response = await httpClient.PostAsync(endpointUrl, requestContent);

                // Get the response content as a string
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("response from assistant: " + responseContent);
                JObject json = JObject.Parse(responseContent);
                JToken jtoken = json.SelectToken("choices").Last().SelectToken("message.content");
                string content2 = jtoken.ToString();
                messages.Add(new {role = "assistant", content = content2});
                Console.WriteLine("messages");
                for (int i = 0; i < messages.Count;i++){
                    Console.WriteLine(messages[i]);
                }
                // Output the response to the console
                Console.WriteLine("response: " + content2);
        }

        static async Task generateImage() {
                Console.WriteLine("Enter some text for generating image:");
                string prompt = Console.ReadLine(); // Reads the user input from the console and stores it in a string variable
                Console.WriteLine("You entered: " + prompt); // Prints the user input to the console

                // Replace the following variables with your own values
                string apiKey = "a1-mDK7hxKvv8PEKT7ZBKPqM3hFjVy1_dfsvdaVPILvPnQnTmjuRXx6PR";
                //"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";
                string uuid = "mDK7hxKvv8PEKT7ZBKPqM3hFjVy1_a6438b96-ad19-4a66-abde-4e65d7971a7a";
                int numImages = 1;
                string response_format = "url";

                // Create an HttpClient instance
                HttpClient httpClient = new HttpClient();

                // Set the endpoint URL
                string endpointUrl = $"https://qrlh34e4y6.execute-api.us-east-1.amazonaws.com/sdCheckEle";
                //$"https://api.openai.com/v1/images/generations";

                // Create a request object with the required parameters
                var requestBody = new {
                    UUID = uuid,
                    token = apiKey
                    //n = numImages,
                    //response_format = response_format
                };
                var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");

                // Add the API key to the request headers
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                // Make the POST request and get the response
                HttpResponseMessage response = await httpClient.PostAsync(endpointUrl, requestContent);

                // Get the response content as a string
                string responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine("response: " + responseContent);
        }
    }
}
