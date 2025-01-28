using System;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ChatEngine.Core.Models;
using ChatEngine.Core.Interfaces;
using ChatEngine.Core.Enums;

namespace ChatEngine.Core.Implementations
{
    public class SemanticKernelService : IChatGPTService
    {
        private readonly Kernel _kernel;
        private readonly IKernelBuilder _kernelBuilder;
        private OpenAIChatCompletionService chatCompletionService;
        private readonly ChatHistory _chatHistory = new();
        private readonly SemanticKernelOptions _options = new();


        public SemanticKernelService(string apiKey)
        {
            _kernelBuilder = Kernel.CreateBuilder();
            _kernelBuilder.AddOpenAIChatCompletion(
                modelId:_options.ModelId,
                apiKey: apiKey,
                orgId: null, // Optional
                serviceId: null, // Optional; for targeting specific services within Semantic Kernel
                httpClient: new HttpClient() // Optional; if not provided, the HttpClient from the kernel will be used
            );
            
            chatCompletionService = new(
                modelId: _options.ModelId,
                apiKey: apiKey,
                organization: null, // Optional
                httpClient: new HttpClient() // Optional; if not provided, the HttpClient from the kernel will be used
            );

            _kernel = _kernelBuilder.Build();
        }

        public SemanticKernelService(string apiKey, SemanticKernelOptions options): this(apiKey)
        {
            _options = options;
        }

        public async Task<ChatMessageContent> GetResponseAsync(string prompt)
        {
            try
            {
                _chatHistory.AddUserMessage(prompt);

                var response = await chatCompletionService.GetChatMessageContentAsync(
                    _chatHistory,
                    kernel: _kernel
                );

                return response;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Network error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }


        public async IAsyncEnumerable<StreamingChatMessageContent?> GetResponseChunkAsync(
     string prompt,
     [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _chatHistory.AddUserMessage(prompt);
            var response = chatCompletionService.GetStreamingChatMessageContentsAsync(
                chatHistory: _chatHistory,
                kernel: _kernel
            );

            await foreach (var chunk in response.WithCancellation(cancellationToken))
            {
                yield return chunk;
            }
        }

        public void AddSystemMessage(string systemMessage)
        {
            _chatHistory.AddSystemMessage(systemMessage);
        }

        public void AddUserMessage(string userMessage)
        {
            _chatHistory.AddUserMessage(userMessage);
        }

        public void ClearChatHistory()
        {
            _chatHistory.Clear();
        }

        public void StartConversation(InitialConversation conversation)
        {
            _chatHistory.Clear();
            foreach (var personality in conversation.Personalities)
            {
                AddPersonality(personality);
            }

            AddSystemMessage(conversation.Prompt);
        }

        private void AddPersonality(PersonalityEnum personality)
        {
            _chatHistory.AddSystemMessage($"Agrega esta personalidad para tus respuestas futuras : {personality.ToString()}");
        }
    }
}
