using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace YourExperience
{
    static class UnselectNode
    {
        private static TreeNode _newNode, _selectedNode;
        private static TreeView _treeView;
        private static RichTextBox _textBoxContent;
        public static void SetValues(TreeView treeView, RichTextBox textBoxContent)
        {
            _treeView = treeView;
            _textBoxContent = textBoxContent;
        }

        public static void Start(TreeNode selectedNode, TreeNode newNode)
        {
            _newNode = newNode;
            _selectedNode = selectedNode;
            Thread thread = new Thread(run);
            thread.Start();
        }

        private static void run()
        {
            while (true)
            {
                Thread.Sleep(2);
                _treeView.Invoke(new MethodInvoker(delegate ()
                {
                    if (_treeView.SelectedNode == _newNode)
                    {
                        _treeView.SelectedNode = _selectedNode;
                        _textBoxContent.Invoke(new MethodInvoker(delegate ()
                        {
                            _textBoxContent.Select();
                        }));
                        return;
                    }
                }));
            }
        }
    }
}
