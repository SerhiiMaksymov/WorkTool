namespace WorkTool.Core.Modules.Graph.Services;

public class TreeNodeBuilder<TKey, TValue> : IBuilder<TreeNode<TKey, TValue>> where TKey : notnull
{
    private readonly Dictionary<TKey, TreeNodeBuilder<TKey, TValue>> nodes;

    public TreeNodeBuilder<TKey, TValue> this[TKey key]
    {
        get
        {
            if (nodes.ContainsKey(key))
            {
                return nodes[key];
            }

            var node = new TreeNodeBuilder<TKey, TValue>
            {
                Key = key
            };

            nodes[key] = node;

            return node;
        }
        set
        {
            nodes[key]        = value;
            nodes[key].Parent = this;
            nodes[key].Key    = key;
        }
    }

    public TreeNodeBuilder<TKey, TValue> this[TKey key, TValue defaultValue]
    {
        get
        {
            if (nodes.ContainsKey(key))
            {
                return nodes[key];
            }

            var node = new TreeNodeBuilder<TKey, TValue>
            {
                Key   = key,
                Value = defaultValue
            };

            nodes[key] = node;

            return node;
        }
    }

    public TreeNodeBuilder<TKey, TValue> this[TValue defaultValue, params TKey[] keys]
    {
        get
        {
            var currentNode = this;

            foreach (var key in keys)
            {
                currentNode = currentNode[key, defaultValue];
            }

            return currentNode;
        }
        set
        {
            var currentNode = this;

            foreach (var key in keys[..^1])
            {
                currentNode = currentNode[key, defaultValue];
            }

            currentNode[keys[^1]]        = value;
            currentNode[keys[^1]].Parent = this;
            currentNode[keys[^1]].Key    = keys[^1];
        }
    }

    public TreeNodeBuilder<TKey, TValue> this[params TKey[] keys]
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
        set
        {
            var currentNode = this;

            foreach (var key in keys[..^1])
            {
                currentNode = currentNode[key];
            }

            currentNode[keys[^1]]        = value;
            currentNode[keys[^1]].Parent = this;
            currentNode[keys[^1]].Key    = keys[^1];
        }
    }

    public TreeNodeBuilder<TKey, TValue> Parent { get; set; }
    public TKey                          Key    { get; set; }
    public TValue                        Value  { get; set; }

    public TreeNodeBuilder()
    {
        nodes = new Dictionary<TKey, TreeNodeBuilder<TKey, TValue>>();
    }

    public TreeNode<TKey, TValue> Build()
    {
        return new TreeNode<TKey, TValue>(Key, Value, nodes.Values.Select(x => x.Build()));
    }

    public TreeNodeBuilder<TKey, TValue> Add(TreeNodeBuilder<TKey, TValue> node)
    {
        nodes.Add(node.Key, node);
        node.Parent = this;

        return this;
    }

    public TreeNodeBuilder<TKey, TValue> SetNode(TValue        defaultValue, TreeNodeBuilder<TKey, TValue> value,
                                                 params TKey[] keys)
    {
        this[defaultValue, keys] = value;

        return this;
    }
}