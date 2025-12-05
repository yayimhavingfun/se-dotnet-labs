using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Nodes;
using System.Text;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Visitor;

public class TreePrintingVisitor : FileSystemVisitorBase
{
    private readonly StringBuilder _output = new();
    private readonly Stack<bool> _lastInLevel = new();
    private int _currentDepth = 0;

    public string FilePrefix { get; set; } = "[F]";

    public string DirectoryPrefix { get; set; } = "[D]";

    public string IndentSymbol { get; set; } = "    ";

    public string BranchSymbol { get; set; } = "├── ";

    public string LastBranchSymbol { get; set; } = "└── ";

    public string VerticalSymbol { get; set; } = "│   ";

    public override void VisitFile(IFileSystemNode fileNode)
    {
        ArgumentNullException.ThrowIfNull(fileNode);

        string prefix = BuildPrefix();
        _output.AppendLine($"{prefix}{FilePrefix}{fileNode.Name}");
    }

    public override void VisitDirectory(IFileSystemNode directoryNode, IEnumerable<IFileSystemNode>? children)
    {
        ArgumentNullException.ThrowIfNull(directoryNode);

        string prefix = BuildPrefix();
        _output.AppendLine($"{prefix}{DirectoryPrefix}{directoryNode.Name}");

        if (children == null)
            return;

        var childrenList = children.ToList();
        if (childrenList.Count == 0)
            return;

        _lastInLevel.Push(false);

        for (int i = 0; i < childrenList.Count; i++)
        {
            IFileSystemNode child = childrenList[i];

            bool isLastChild = i == childrenList.Count - 1;
            _lastInLevel.Push(isLastChild);
            _currentDepth++;

            try
            {
                if (child.IsDirectory)
                {
                    IEnumerable<IFileSystemNode> grandChildren = child.GetChildren(1);
                    VisitDirectory(child, grandChildren);
                }
                else
                {
                    VisitFile(child);
                }
            }
            finally
            {
                _currentDepth--;
                _lastInLevel.Pop();
            }
        }

        _lastInLevel.Pop();
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
        if (_currentDepth == 0)
            return string.Empty;

        var prefixBuilder = new StringBuilder();
        var levels = _lastInLevel.ToList();

        for (int i = 0; i < _currentDepth - 1; i++)
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
}