namespace WorkTool.Core.Modules.Ui.Services;

public class UiContext
{
    protected readonly Dictionary<Type, IEnumerable<Func<object>>> children;
    protected readonly Dictionary<Type, IEnumerable<CommandContext>> commands;
    protected readonly Dictionary<Type, Func<object>> contents;
    protected readonly Dictionary<Type, TreeNode<string, MenuItemContext>[]> contextMenus;
    protected readonly Dictionary<Type, IEnumerable<KeyboardKeyBinding>> keyboardKeyGestures;
    protected readonly Dictionary<Type, TreeNode<string, MenuItemContext>[]> menu;
    protected readonly Dictionary<
        ClassPropertyPath,
        IEnumerable<PropertyDefaultValue>
    > propertyDefaultValues;

    public UiContext(
        IReadOnlyDictionary<Type, Func<object>> contents,
        IReadOnlyDictionary<Type, TreeNode<string, MenuItemContext>[]> menu,
        IReadOnlyDictionary<Type, IEnumerable<KeyboardKeyBinding>> keyboardKeyGestures,
        IReadOnlyDictionary<Type, IEnumerable<CommandContext>> commands,
        IReadOnlyDictionary<Type, TreeNode<string, MenuItemContext>[]> contextMenus,
        IReadOnlyDictionary<Type, IEnumerable<Func<object>>> children,
        IReadOnlyDictionary<
            ClassPropertyPath,
            IEnumerable<PropertyDefaultValue>
        > propertyDefaultValues
    )
    {
        this.propertyDefaultValues = new Dictionary<
            ClassPropertyPath,
            IEnumerable<PropertyDefaultValue>
        >(propertyDefaultValues.ThrowIfNull());

        this.children = new Dictionary<Type, IEnumerable<Func<object>>>(children.ThrowIfNull());
        this.contents = new Dictionary<Type, Func<object>>(contents.ThrowIfNull());
        this.menu = new Dictionary<Type, TreeNode<string, MenuItemContext>[]>(menu.ThrowIfNull());

        this.keyboardKeyGestures = new Dictionary<Type, IEnumerable<KeyboardKeyBinding>>(
            keyboardKeyGestures.ThrowIfNull()
        );

        this.commands = new Dictionary<Type, IEnumerable<CommandContext>>(commands.ThrowIfNull());
        this.contextMenus = new Dictionary<Type, TreeNode<string, MenuItemContext>[]>(
            contextMenus.ThrowIfNull()
        );
    }

    public IEnumerable<PropertyDefaultValue> GetDefaultValues(ClassPropertyPath property)
    {
        if (propertyDefaultValues.TryGetValue(property, out var value))
        {
            return value;
        }

        return Enumerable.Empty<PropertyDefaultValue>();
    }

    public IEnumerable<PropertyDefaultValue> GetDefaultValues(Type type, PropertyInfo property)
    {
        return GetDefaultValues(new ClassPropertyPath(type, property));
    }

    public void InitView(object view)
    {
        var type = view.GetType();

        if (view is IChildrenView childrenView && children.ContainsKey(type))
        {
            foreach (var child in children[type])
            {
                childrenView.AddChild(child);
            }
        }

        if (view is IContextMenuView contextMenuView && contextMenus.ContainsKey(type))
        {
            foreach (var menuItem in contextMenus[type])
            {
                contextMenuView.AddMenuItem(menuItem);
            }
        }

        if (view is IMenuView menuView && menu.ContainsKey(type))
        {
            foreach (var menuItem in menu[type])
            {
                menuView.AddMenuItem(menuItem);
            }
        }

        if (view is IKeyBindingView keyBindingView && keyboardKeyGestures.ContainsKey(type))
        {
            foreach (var keyboardKeyGesture in keyboardKeyGestures[type])
            {
                foreach (var keyGesture in keyboardKeyGesture.KeyboardKeyGestures)
                {
                    keyBindingView.AddKeyBinding(keyGesture, keyboardKeyGesture.Command);
                }
            }
        }

        if (view is ICommandView commandView && commands.ContainsKey(type))
        {
            foreach (var command in commands[type])
            {
                commandView.AddCommand(command);
            }
        }

        if (view is IContentView contentView && contents.ContainsKey(type))
        {
            contentView.AddContent(contents[type]);
        }
    }
}
