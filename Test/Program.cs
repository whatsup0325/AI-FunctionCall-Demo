using Azure;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Test
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
            kernelBuilder.AddOpenAIChatCompletion(
                modelId: "gpt-4o-mini",
                apiKey: ""

            );
            kernelBuilder.Plugins.AddFromType<OrderPizzaPlugin>("OrderPizza");
            Kernel kernel = kernelBuilder.Build();

            IChatCompletionService chatCompletion = kernel.GetRequiredService<IChatCompletionService>();

            OpenAIPromptExecutionSettings settings = new()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            ChatHistory chatHistory = [];
            string firstMessage = "你好，請給我菜單";
            chatHistory.AddUserMessage(firstMessage);
            Console.WriteLine(firstMessage);

            while (true)
            {
                ChatMessageContent result = await chatCompletion.GetChatMessageContentAsync(chatHistory, settings, kernel);

                // Check if the AI model has generated a response.
                if (result.Content is not null)
                {
                    Console.WriteLine(result.Content);
                }

                // Adding AI model response containing chosen functions to chat history as it's required by the models to preserve the context.
                chatHistory.Add(result);
                var userInput = Console.ReadLine();

                if (string.IsNullOrEmpty(userInput))                
                    break;
                

                chatHistory.AddUserMessage(userInput);
            }


        }
    }
}
