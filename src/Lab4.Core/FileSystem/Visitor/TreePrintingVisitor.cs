using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Nodes;
using System.Text;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Visitor;

public class TreePrintingVisitor : IFileSystemVisitor
{
    private readonly StringBuilder _output = new();
    private readonly Stack<bool> _lastInLevel = new();
    private int _currentDepth = 0;

    public string IndentSymbol { get; set; } = "    ";

    public string BranchSymbol { get; set; } = "├── ";

    public string LastBranchSymbol { get; set; } = "└── ";

    public string VerticalSymbol { get; set; } = "│   ";

    public void Visit(IFileSystemNode node, int maxDepth = 1)
    {
        _output.Clear();
        _lastInLevel.Clear();
        _currentDepth = 0;

        if (node.Strategy.HasChildren && maxDepth > 0)
        {
            var children = node.GetChildren(1).ToList();

            for (int i = 0; i < children.Count; i++)
            {
                bool isLast = i == children.Count - 1;
                _currentDepth = 1;
                VisitRecursive(children[i], maxDepth, isLast);
                _currentDepth = 0;
            }
        }
        else
        {
            _currentDepth = 0;
            string nodePrefix = node.Strategy.DisplayPrefix;
            _output.AppendLine($"{nodePrefix} {node.Name}");
        }
    }

    public void Clear()
    {
        _output.Clear();
        _lastInLevel.Clear();
        _currentDepth = 0;
    }

    public string GetResult()
    {
        return _output.ToString();
    }

    private string BuildPrefix()
    {
        if (_currentDepth <= 1)
            return string.Empty;

        var prefixBuilder = new StringBuilder();
        var levels = _lastInLevel.ToList();

        for (int i = 0; i < _currentDepth - 2; i++)
        {
            bool isLastAtLevel = levels[i];
            prefixBuilder.Append(isLastAtLevel ? IndentSymbol : VerticalSymbol);
        }

        if (_lastInLevel.Count > 0)
        {
            bool isLast = _lastInLevel.Peek();
            prefixBuilder.Append(isLast ? LastBranchSymbol : BranchSymbol);
        }

        return prefixBuilder.ToString();
    }

    private void VisitRecursive(IFileSystemNode node, int maxDepth, bool isLastChild = false)
    {
        if (_currentDepth >= maxDepth)
            return;

        if (_currentDepth > 1)
        {
            _lastInLevel.Push(isLastChild);
        }

        string prefix = BuildPrefix();
        string nodePrefix = node.Strategy.DisplayPrefix;
        _output.AppendLine($"{prefix}{nodePrefix}{node.Name}");

        if (node.Strategy.HasChildren && _currentDepth < maxDepth)
        {
            var children = node.GetChildren(1).ToList();

            if (children.Count != 0)
            {
                _lastInLevel.Push(false);
                _currentDepth++;

                for (int i = 0; i < children.Count; i++)
                {
                    bool isLast = i == children.Count - 1;
                    VisitRecursive(children[i], maxDepth, isLast);
                }

                _currentDepth--;
                _lastInLevel.Pop();
            }
        }

        if (_currentDepth > 0)
        {
            _lastInLevel.Pop();
        }
    }
}