namespace WorkTool.Core.Modules.Graph.Models;

public class TreeNode<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, TreeNode<TKey, TValue>> nodes;

    public TreeNode<TKey, TValue> this[TKey key] => nodes[key];

    public TreeNode<TKey, TValue> this[params TKey[] keys]
    {
        get
        {
            var currentNode = this;

            foreach (var key in keys)
            {
                currentNode = currentNode[key];
            }

            return currentNode;
        }
    }

    public TreeNode<TKey, TValue> Parent { get; private set; }
    public TKey Key { get; }
    public TValue Value { get; }
    public IEnumerable<TValue> NodeValues { get; }
    public IEnumerable<TKey> NodeKeys { get; }
    public IEnumerable<TreeNode<TKey, TValue>> Nodes { get; }

    public TreeNode(TKey key, TValue value, IEnumerable<TreeNode<TKey, TValue>> nodes)
    {
        this.nodes = new Dictionary<TKey, TreeNode<TKey, TValue>>();
        Key = key.ThrowIfNull();
        Value = value.ThrowIfNull();

        foreach (var node in nodes)
        {
            this.nodes.Add(node.Key, node);
            node.Parent = this;
        }

        NodeValues = this.nodes.Values.Select(x => x.Value).ToArray();
        NodeKeys = this.nodes.Values.Select(x => x.Key).ToArray();
        Nodes = this.nodes.Values;
    }
}
