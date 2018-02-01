using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PA2WebApp
{
    public class Node
    {
        public char value { get; set; }
        public List<Node> children { get; set; }
        public int depth { get; set; }

        public Node parent { get; set; }

        public Node(char value, int depth, Node parent)
        {
            this.value = value;
            this.children = new List<Node>();
            this.depth = depth;
            this.parent = parent;
        }

        public Boolean isLeaf()
        {
            return this.children.Count == 0;
        }

        public List<Node> getChildNodes()
        {
            return this.children;
        }
    }
}