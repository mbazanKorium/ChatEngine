using ChatEngine.Core.Enums;
using ChatEngine.Core.Implementations;
using ChatEngine.Core.Models;

namespace ConsoleTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var service = new SemanticKernelService("");

            InitConversation(service, new InitialConversation()
            {
                Prompt = GetInitialPromptSystemMessage(),
                Personalities = new[] {
                        PersonalityEnum.NARRATOR,
                }
            }
            );

            string userPrompt = "Dame la bienvenida";

            do
            {
                Console.WriteLine();

                Console.WriteLine();

                Console.WriteLine();

                if (!string.IsNullOrEmpty(userPrompt))
                {
                    var serviceResponse = service.GetResponseChunkAsync(userPrompt);

                    await foreach (var item in serviceResponse)
                    {
                        WriteSystemMessage(item);
                    }

                    Console.WriteLine();

                    Console.WriteLine();

                    Console.WriteLine();

                    Console.ResetColor();

                    userPrompt = Console.ReadLine();

                }
            } while (userPrompt.ToLower() != "quit");

          
        }

        static void WriteSystemMessage(object message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{message}");
        }

        static void InitConversation(SemanticKernelService service, InitialConversation conversation)
        {
            service.StartConversation(conversation);
            service.AddUserMessage("Dame la bienvenida a la aventura");

        }

        static void InitConversation()
        {
            Console.WriteLine("Escribe algo en la consola para comenzar a interactuar: ");

        }

        static string GetInitialPromptSystemMessage()
        {
            return @"
            Eres un narrador de aventuras fantásticas. Interpretarás muchos personajes (NPCs) con personalidades específicas.
            Tu trabajo es guiar al usuario en una aventura narrada por ti, interactuando como los NPCs y reaccionando a sus decisiones.

            Aquí tienes una lista de los NPCs que interpretarás:
            1. **Zarok, el hechicero oscuro**:
               - Personalidad: Sarcástico, manipulador, pero con un trasfondo trágico.
               - Habla con un tono oscuro y utiliza frases elaboradas. Siempre intenta ganar la confianza del usuario con sutileza.

            2. **Lira, la elfa arquera**:
               - Personalidad: Valiente y amable, pero algo desconfiada de los desconocidos.
               - Habla con sinceridad y tiende a ofrecer ayuda, pero hará preguntas antes de confiar.

            3. **Grum, el mercader enano**:
               - Personalidad: Avaro y astuto, siempre buscando cerrar un trato.
               - Habla con un tono gruñón y directo. Usa frases como 'Nada es gratis en este mundo' o 'Hablemos de negocios.'

            Tu objetivo es mantener la personalidad de estos NPCs y enriquecer la aventura del usuario. Manten las respuestas cortas. Reacciona a las decisiones del usuario de manera coherente y creativa. 
            ";
        }

    }
}
