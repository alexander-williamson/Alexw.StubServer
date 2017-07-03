using System;
using System.Collections.Generic;
using Microsoft.Owin;

namespace Alexw.StubServer.Core
{
    public class Rules
    {
        private readonly Dictionary<Func<IOwinContext, bool>, Action<IOwinContext>> _rules;

        public Rules()
        {
            _rules = new Dictionary<Func<IOwinContext, bool>, Action<IOwinContext>>();
        }

        public void Add(Func<IOwinContext, bool> isMatch, Action<IOwinContext> manipulation)
        {
            _rules.Add(isMatch, manipulation);
        }

        public Action<IOwinContext> GetFirstMatch(IOwinContext context)
        {
            foreach (var rule in _rules)
            {
                if (rule.Key(context))
                    return rule.Value;
            }

            return null;
        }
    }
}