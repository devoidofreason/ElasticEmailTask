using System.Collections.Generic;

namespace Resolver
{
    internal class InputManager
    {
        private List<string> domains;

        public InputManager(string[] args)
        {
            domains = new List<string>(args);
        }

        public List<string> getDomains() => domains;
    }
}
