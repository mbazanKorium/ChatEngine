using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ChatEngine.Core.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace ChatEngine.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for a service that interacts with OpenAI's Chat Completion API.
    /// Provides methods for sending prompts, receiving responses, and managing conversation history.
    /// </summary>
    public interface IChatGPTService
    {
        /// <summary>
        /// Sends a single prompt to the Chat Completion API and retrieves the full response.
        /// </summary>
        /// <param name="prompt">The prompt to send to the model.</param>
        /// <returns>A <see cref="ChatMessageContent"/> object containing the full response.</returns>
        Task<ChatMessageContent> GetResponseAsync(string prompt);

        /// <summary>
        /// Sends a prompt to the Chat Completion API and retrieves the response in chunks (streaming mode).
        /// This allows real-time processing of the response as it is received.
        /// </summary>
        /// <param name="prompt">The prompt to send to the model.</param>
        /// <param name="cancellationToken">A token to cancel the streaming operation, if needed.</param>
        /// <returns>An asynchronous enumerable of <see cref="StreamingChatMessageContent"/> chunks, which may be null.</returns>
        IAsyncEnumerable<StreamingChatMessageContent?> GetResponseChunkAsync(
            string prompt,
            [EnumeratorCancellation] CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a system message to the conversation history.
        /// System messages are typically used to set the behavior or context of the assistant.
        /// </summary>
        /// <param name="systemMessage">The system message to add to the conversation history.</param>
        void AddSystemMessage(string systemMessage);

        /// <summary>
        /// Adds a user message to the conversation history.
        /// User messages represent the user's input in the chat.
        /// </summary>
        /// <param name="userMessage">The user message to add to the conversation history.</param>
        void AddUserMessage(string userMessage);

        /// <summary>
        /// Starts a new conversation by clearing the conversation history
        /// and setting an initial system message for personalization or context.
        /// </summary>
        /// <param name="initialPrompt">The initial system message to define the assistant's personality or behavior.</param>
        void StartConversation(InitialConversation conversation);

        /// <summary>
        /// Clears the current conversation history, removing all user and system messages.
        /// This is useful for starting a new, fresh conversation.
        /// </summary>
        void ClearChatHistory();
    }
}
