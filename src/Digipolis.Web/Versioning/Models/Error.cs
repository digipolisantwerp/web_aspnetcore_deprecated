using System;
using System.Collections.Generic;

namespace Digipolis.Web.Versioning.Models
{
        public class Error
        {
            public Error()
            {
                _messages = new List<string>();
            }

            public Error(string message, params object[] args) : this()
            {
                AddMessage(message, args);
            }

            public Error(IEnumerable<string> messages) : this()
            {
                _messages.AddRange(messages);
            }

            private readonly List<string> _messages;
            public IEnumerable<string> Messages
            {
                get { return _messages; }
            }

            public void AddMessage(string message, params object[] args)
            {
                if (!String.IsNullOrWhiteSpace(message))
                    _messages.Add(String.Format(message, args));
            }
        }
    }