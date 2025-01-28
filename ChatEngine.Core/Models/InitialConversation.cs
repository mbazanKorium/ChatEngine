using ChatEngine.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatEngine.Core.Models
{
    public class InitialConversation
    {
        public int Id { get; set; }
        public string Prompt { get; set; }
        public required IEnumerable<PersonalityEnum> Personalities { get; set; } = Enumerable.Empty<PersonalityEnum>();

    }
}
