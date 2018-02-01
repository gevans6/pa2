using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PA2WebApp
{
    public class Trie
    {
        public Node root;

        public Trie()
        {
            root = new Node('.', 0, null);
        }

        public Node Prefix(string word)
        {
            var current = root;

            for (int i = 0; i < word.Length; i++)
            {
                bool found = false;

                foreach (Node child in current.getChildNodes())
                {
                    if (child.value == word[i])
                    {
                        current = child;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    break;
                }
            }

            return current;
        }

        public void InsertWord(String word)
        {
            var current = Prefix(word);
            for (int i = current.depth; i < word.Length; i++)
            {
                var addNode = new Node(word[i], current.depth + 1, current);
                current.children.Add(addNode);
                current = addNode;
            }
            //This identifier, '!', signals that this is the end of a word
            current.children.Add(new Node('!', current.depth, current));
        }

        public void InsertMany(List<string> words)
        {
            foreach (string word in words)
            {
                InsertWord(word);
            }
        }

        //not used
        public Node Search(string word)
        {
            Node end = Prefix(word);
            System.Diagnostics.Debug.WriteLine(end.value);

            //find what end is. get the index of the word that is the 'end'. if that is successful, return the node itself and allow its chidlren to be found.
            if (end.depth == word.Length)
            {
                foreach (Node child in end.getChildNodes())
                {
                    if (child.value == '!' || Char.ToLower(child.value) == Char.ToLower(word[end.depth]))
                    {
                        return child;
                    }
                }
            }
            return null;
        }

        public List<string> SearchAll(string word)
        {
            Node end = Prefix(word);

            System.Diagnostics.Debug.WriteLine(end.value);

            List<string> answers = new List<string>();
            List<string> refinedAnswers = new List<string>();

            DFSSearch(answers, end, word);

            //remove exclamation points
            foreach (string a in answers)
            {
                System.Diagnostics.Debug.WriteLine(a);
                refinedAnswers.Add(a.TrimEnd('!'));
            }

            return refinedAnswers;
        }

        public void DFSSearch(List<string> answers, Node start, string word)
        {
            if (answers.Count >= 10)
            {
                return;
            }

            if (start.value == '!')
            {
                answers.Add(word);
                return;
            }

            //empty word
            if (word.Length < 1)
            {
                return;
            }

            //word doesn't exist
            if (word[word.Length - 1] != start.value && word.Length > 0)
            {
                return;
            }

            foreach (Node node in start.children)
            {
                DFSSearch(answers, node, word + node.value);
            }
        }
    }
}