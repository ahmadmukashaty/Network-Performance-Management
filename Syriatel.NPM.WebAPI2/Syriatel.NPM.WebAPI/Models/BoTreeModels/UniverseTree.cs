using Syriatel.NPM.WebAPI.Models.OracleConnections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Syriatel.NPM.BoManager.OracleConnections
{
    public class UniverseTree
    {
        private OracleBoData ocl = null;
        
        public ClassDescriptor tree = null;

        private int Id = 0;

        public UniverseTree(string universe)
        {
            ocl = new OracleBoData();
            ocl.GetSubsetsCountersData(universe);
            CreateUniverseTree(universe);
        }

        private void CreateUniverseTree(string universe)
        {
            this.tree = new ClassDescriptor();
            this.tree.value = universe;
            this.tree.id = Id;
            Id++;
            foreach (CounterDescriptor cd in ocl.Counters)
                if (cd.path != null && cd.Subset != null && cd.Subset.Universe == universe)
                    SetPathInTree(tree, cd.path);

            foreach (CounterDescriptor cd in ocl.Counters)
                if (cd.path != null && cd.Subset != null && cd.Subset.Universe == universe)
                {
                    SetCounterInTree(cd);
                }
        }

        private void SetPathInTree(ClassDescriptor tree, string path)
        {
            string name = GetFirstClassFromPath(path);
            if (name == null)
                return;
            if (tree.children != null)
            {
                int index = GetChildIndex(tree.children, name);
                if (index != -1)
                {
                    ClassDescriptor subTree = (ClassDescriptor)tree.children[index];
                    string subPath = RemoveFirstClassFromPath(path);
                    SetPathInTree(subTree, subPath);
                }
                else
                {
                    AddNode(tree, path);
                }
            }
            else
            {
                AddNode(tree, path);
            }
        }

        private void AddNode(ClassDescriptor tree, string path)
        {
            if (tree != null)
            {
                ClassDescriptor subTree = CreateTreeFromPath(path);
                if (tree.children == null)
                    tree.children = new List<BoObject>();
                tree.children.Add(subTree);
            }
        }

        private ClassDescriptor CreateTreeFromPath(string path)
        {
            if (path == null)
                return null;

            ClassDescriptor tree = new ClassDescriptor();
            string name = GetFirstClassFromPath(path);
            if(name != null)
            {
                tree.value = name;
                tree.id = Id;
                Id++;
                string subPath = RemoveFirstClassFromPath(path);
                if (subPath != null)
                {
                    ClassDescriptor child = CreateTreeFromPath(subPath);
                    if (tree.children == null)
                        tree.children = new List<BoObject>();
                    tree.children.Add(child);
                }
            }
            return tree;
        }

        private string GetFirstClassFromPath(string path)
        {
            if(path != null)
            {
                string[] words = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                return words[0];
            }
            return null;
        }

        private string RemoveFirstClassFromPath(string path)
        {
            string[] words = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if(words.Length>1)
            {
                string newPath = "";
                for (int i = 1; i < words.Length; i++)
                {
                    newPath += (i == 1) ? (words[i]) : ("/" + words[i]);
                }
                return newPath;
            }
            return null;
        }

        private int GetChildIndex(IList<BoObject> children, string child)
        {
            for (int i = 0; i < children.Count;i++)
            {
                if (children[i].value == child)
                    return i;
            }
            return -1;
        }

        private void SetCounterInTree(CounterDescriptor cd)
        {
            ClassDescriptor tempTree = tree;
            string path = cd.path;
            while(path != null)
            {
                string subPath = GetFirstClassFromPath(path);
                path = RemoveFirstClassFromPath(path);
                int index = GetChildIndex(tempTree.children, subPath);
                tempTree = (ClassDescriptor)tempTree.children[index];
            }
            if (tempTree.children == null)
                tempTree.children = new List<BoObject>();
            cd.id = Id;
            Id++;
            tempTree.children.Add(cd);
        }

    }
}
