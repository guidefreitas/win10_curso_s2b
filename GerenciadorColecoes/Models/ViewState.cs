using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorColecoes.Models
{
    public class ViewState
    {
        private static Dictionary<String, Object> states = new Dictionary<string, object>();

        public static void Set(String name, Object value)
        {
            states.Add(name, value);
        }

        public static Object Get(String name)
        {
            Object value = null;

            if (states.ContainsKey(name))
            {
                value = states[name];
            }

            return value;
        }
    }
}
