using System;

namespace Wonga.QA.Framework
{
    public class Application
    {
        public Guid Id { get; set; }

        public Application(Guid id)
        {
            Id = id;
        }
    }
}