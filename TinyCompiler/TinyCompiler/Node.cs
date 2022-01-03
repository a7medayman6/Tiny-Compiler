using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCompiler
{
    public class Node
    {
        public List<Node> children;
        public string Name;
        public Node()
        {
            children = new List<Node>();
        }
        public Node(string Name)
        {
            children = new List<Node>();
            this.Name = Name;
        }
    }
}
