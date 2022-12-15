namespace WorkTool.Core.Modules.Ui.Services;

public class UiContextBuilder : IBuilder<UiContext>
{
    private readonly Dictionary<Type, List<Func<object>>> children;
    private readonly Dictionary<Type, Dictionary<string, CommandContext>> commandContexts;
    private readonly Dictionary<
        Type,
        Dictionary<string, List<KeyboardKeyGesture>>
    > commandKeyboardKeyGestures;
    private readonly Dictionary<Type, Func<object>> contents;
    private readonly Dictionary<
        Type,
        Dictionary<string, KeyValuePair<string, Func<object>>[]>
    > contextMenus;
    private readonly Dictionary<string, FunctionsInfo> functions;
    private readonly Dictionary<
        Type,
        Dictionary<string, KeyValuePair<string, Func<object>>[]>
    > menus;
    private readonly Dictionary<
        ClassPropertyPath,
        List<PropertyDefaultValue>
    > propertyDefaultValues;
    private readonly IResolver resolver;

    public UiContextBuilder(IResolver resolver)
    {
        this.resolver = resolver.ThrowIfNull();
        children = new Dictionary<Type, List<Func<object>>>();
        contextMenus =
            new Dictionary<Type, Dictionary<string, KeyValuePair<string, Func<object>>[]>>();
        contents = new Dictionary<Type, Func<object>>();
        functions = new Dictionary<string, FunctionsInfo>();
        menus = new Dictionary<Type, Dictionary<string, KeyValuePair<string, Func<object>>[]>>();
        commandKeyboardKeyGestures =
            new Dictionary<Type, Dictionary<string, List<KeyboardKeyGesture>>>();
        commandContexts = new Dictionary<Type, Dictionary<string, CommandContext>>();
        propertyDefaultValues = new Dictionary<ClassPropertyPath, List<PropertyDefaultValue>>();
    }

    public UiContext Build()
    {
        var menu = BuildMenu();
        var keyboardKeyBindings = BuildKeyboardKeyBindings();
        var commands = BuildCommands();
        var contextMenu = BuildContextMenu();
        var newChildren = BuildChildren();
        var newPropertyDefaultValues = BuildPropertyDefaultValues();

        return new UiContext(
            contents,
            menu,
            keyboardKeyBindings,
            commands,
            contextMenu,
            newChildren,
            newPropertyDefaultValues
        );
    }

    public UiContextBuilder AddPropertyDefaultValue(
        Expression expression,
        object value,
        [CallerArgumentExpression("value")] string name = ""
    )
    {
        return AddPropertyDefaultValues(expression, new PropertyDefaultValue(name, value));
    }

    public UiContextBuilder AddPropertyDefaultValues(
        Expression expression,
        params PropertyDefaultValue[] values
    )
    {
        var lambdaExpression = (LambdaExpression)expression;
        var memberExpression = (MemberExpression)lambdaExpression.Body;
        var declaringType = memberExpression.Member.DeclaringType.ThrowIfNull();
        var property = declaringType.GetProperty(memberExpression.Member.Name).ThrowIfNull();

        return AddPropertyDefaultValues(new ClassPropertyPath(declaringType, property), values);
    }

    public UiContextBuilder AddPropertyDefaultValues(
        ClassPropertyPath path,
        params PropertyDefaultValue[] values
    )
    {
        if (!propertyDefaultValues.ContainsKey(path))
        {
            propertyDefaultValues[path] = new List<PropertyDefaultValue>();
        }

        propertyDefaultValues[path].AddRange(values);

        return this;
    }

    public UiContextBuilder AddFunctionsFromService<T>()
    {
        return AddFunctionsFromService(typeof(T));
    }

    public UiContextBuilder AddChild<TChildrenView>(Func<object> child)
        where TChildrenView : IChildrenView
    {
        if (!children.ContainsKey(typeof(TChildrenView)))
        {
            children[typeof(TChildrenView)] = new List<Func<object>>();
        }

        children[typeof(TChildrenView)].Add(child);

        return this;
    }

    public UiContextBuilder AddChild<TChildrenView, TChild>()
        where TChildrenView : IChildrenView
        where TChild : notnull
    {
        if (!children.ContainsKey(typeof(TChildrenView)))
        {
            children[typeof(TChildrenView)] = new List<Func<object>>();
        }

        children[typeof(TChildrenView)].Add(() => resolver.Resolve<TChild>());

        return this;
    }

    public UiContextBuilder SetContent<TContentView>(Func<object> content)
        where TContentView : IContentView
    {
        contents[typeof(TContentView)] = content;

        return this;
    }

    public UiContextBuilder SetContent<TContentView, TContent>()
        where TContentView : IContentView
        where TContent : notnull
    {
        contents[typeof(TContentView)] = () => resolver.Resolve<TContent>();

        return this;
    }

    public UiContextBuilder AddFunctionsFromMethod(string functionName, MethodInfo method)
    {
        var call = Expression.Call(method);
        var lambda = Expression.Lambda(call);
        var @delegate = lambda.Compile();

        return AddFunction(functionName, @delegate);
    }

    public UiContextBuilder AddFunctionsFromService(Type serviceType)
    {
        var methods = serviceType.GetMethods();

        foreach (var method in methods)
        {
            var parameters = new List<ParameterExpression>();
            var serviceParameter = Expression.Parameter(serviceType);
            var methodParameters = method.GetParameters();

            foreach (var methodParameter in methodParameters)
            {
                parameters.Add(Expression.Parameter(methodParameter.ParameterType));
            }

            var call = Expression.Call(serviceParameter, method, parameters);

            var rootParameters = new List<ParameterExpression> { serviceParameter };

            rootParameters.AddRange(parameters);
            var @delegate = call.Lambda(rootParameters.ToArray()).Compile();
            var nameFunction = NameHelper.GetNameFunction(serviceType, parameters, method);
            AddFunction(nameFunction, @delegate);
        }

        return this;
    }

    public UiContextBuilder AddFromAssemblies()
    {
        SetConfigurationsFromAssemblies();
        AddFunctionsFromAssemblies();
        AddTabItemFunctionsFromAssemblies();
        AddUiMenuItemFromFromAssemblies();
        SetCommandsFromAssemblies();

        return this;
    }

    public UiContextBuilder AddFromAssembly(Assembly assembly)
    {
        SetConfigurationsFromAssembly(assembly);
        AddFunctionsFromAssembly(assembly);
        AddTabItemFunctionsFromAssembly(assembly);
        AddUiMenuItemFromFromAssembly(assembly);
        SetCommandsFromAssembly(assembly);

        return this;
    }

    public UiContextBuilder AddFunctionsFromAssemblies()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        return AddFunctionsFromAssemblies(assemblies);
    }

    public UiContextBuilder AddFunctionsFromAssemblies(IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            AddFunctionsFromAssembly(assembly);
        }

        return this;
    }

    public UiContextBuilder AddFunctionsFromAssembly(Assembly assembly)
    {
        var methods = assembly.GetTypes().SelectMany(x => x.GetMethods());

        foreach (var method in methods)
        {
            var function = method.GetCustomAttribute<UiFunctionAttribute>();

            if (function is null)
            {
                continue;
            }

            AddFunctionsFromMethod(function.Name, method);
        }

        var services = assembly.GetCustomAttributes<UiServiceAttribute>();

        foreach (var service in services)
        {
            AddFunctionsFromService(service.Type);
        }

        return this;
    }

    public UiContextBuilder SetConfigurationsFromAssemblies()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        return SetConfigurationsFromAssemblies(assemblies);
    }

    public UiContextBuilder SetConfigurationsFromAssemblies(IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            SetConfigurationsFromAssembly(assembly);
        }

        return this;
    }

    public UiContextBuilder SetConfigurationsFromAssembly(Assembly assembly)
    {
        var types = assembly
            .GetTypes()
            .Where(
                x => !x.IsInterface && !x.IsAbstract && x.IsAssignableTo(typeof(IUiConfiguration))
            );

        foreach (var type in types)
        {
            var configuration = (IUiConfiguration)resolver.Resolve(type);
            configuration.Configure(this);
        }

        return this;
    }

    public UiContextBuilder SetCommandsFromAssemblies()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        return SetCommandsFromAssemblies(assemblies);
    }

    public UiContextBuilder SetCommandsFromAssemblies(IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            SetCommandsFromAssembly(assembly);
        }

        return this;
    }

    public UiContextBuilder SetCommandsFromAssembly(Assembly assembly)
    {
        var commands = assembly.GetCustomAttributes<UiCommandAttribute>();

        foreach (var command in commands)
        {
            SetCommandCore(
                command.Target,
                command.FunctionName,
                () => resolver.Resolve(command.Content)
            );
        }

        return this;
    }

    public UiContextBuilder AddFunction(FunctionsInfo function)
    {
        functions.Add(function.Name, function);

        return this;
    }

    public UiContextBuilder AddFunction(string name, Delegate @delegate)
    {
        var command = new FunctionsInfo(name, @delegate);

        return AddFunction(command);
    }

    public UiContextBuilder SetCommand<TCommandView>(Expression expression, Func<object> content)
        where TCommandView : ICommandView
    {
        var functionName = NameHelper.GetNameFunction(expression);

        return SetCommand<TCommandView>(functionName, content);
    }

    public UiContextBuilder SetCommand<TCommandView>(string functionName, Func<object> content)
        where TCommandView : ICommandView
    {
        return SetCommandCore(typeof(TCommandView), functionName, content);
    }

    private UiContextBuilder SetCommandCore(Type target, string functionName, Func<object> content)
    {
        var command = functions[functionName];

        if (!commandContexts.ContainsKey(target))
        {
            commandContexts[target] = new Dictionary<string, CommandContext>();
        }

        commandContexts[target][functionName] = new CommandContext(command.Command, content);

        return this;
    }

    public UiContextBuilder AddTabItemFunction<TTabControlView>(
        string functionName,
        TabItemContext tabItemContext
    ) where TTabControlView : ITabControlView
    {
        var command = new FunctionsInfo(
            functionName,
            (TTabControlView view) => view.AddTabItem(tabItemContext)
        );

        return AddFunction(command);
    }

    public UiContextBuilder AddTabItemFunction<TTabControlView>(
        Expression expression,
        Func<object> header,
        Func<object> content
    ) where TTabControlView : ITabControlView
    {
        return AddTabItemFunction<TTabControlView>(
            NameHelper.GetNameFunction(expression),
            header,
            content
        );
    }

    public UiContextBuilder AddTabItemFunctionsFromAssemblies()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        return AddTabItemFunctionsFromAssemblies(assemblies);
    }

    public UiContextBuilder AddTabItemFunctionsFromAssemblies(IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            AddTabItemFunctionsFromAssembly(assembly);
        }

        return this;
    }

    public UiContextBuilder AddUiMenuItemFromFromAssemblies()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        return AddUiMenuItemFromFromAssemblies(assemblies);
    }

    public UiContextBuilder AddUiMenuItemFromFromAssemblies(IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            AddUiMenuItemFromFromAssembly(assembly);
        }

        return this;
    }

    public UiContextBuilder AddUiMenuItemFromFromAssembly(Assembly assembly)
    {
        var uiMenuItemFromTabItems = assembly.GetCustomAttributes<UiMenuItemFromTabItemAttribute>();

        foreach (var uiMenuItemFromTabItem in uiMenuItemFromTabItems)
        {
            var functionName = NameHelper.GetNameAddTabItemFunction(
                uiMenuItemFromTabItem.TabControlViewType,
                uiMenuItemFromTabItem.ContentType
            );

            var path = GetMenuPath(uiMenuItemFromTabItem.Path);
            AddMenuItemCore(uiMenuItemFromTabItem.MenuViewType, functionName, path);
        }

        return this;
    }

    public UiContextBuilder AddTabItemFunctionsFromAssembly(Assembly assembly)
    {
        var tabItems = assembly.GetCustomAttributes<UiTabItemAttribute>();

        foreach (var tabItem in tabItems)
        {
            var tabItemContext = new TabItemContext(
                () => tabItem.Header,
                () => resolver.Resolve(tabItem.ContentType)
            );
            var @delegate = CreateTabItemFunction(tabItem.TabControlViewType, tabItemContext);
            var functionName = NameHelper.GetNameAddTabItemFunction(
                tabItem.TabControlViewType,
                tabItem.ContentType
            );
            AddFunction(functionName, @delegate);
        }

        return this;
    }

    private Delegate CreateTabItemFunction(Type tabControlViewType, TabItemContext context)
    {
        var tabControlViewParameter = Expression.Parameter(tabControlViewType);

        var method = typeof(ITabControlView)
            .GetMethod(nameof(ITabControlView.AddTabItem))
            .ThrowIfNull();

        var contextConstant = Expression.Constant(context);
        var call = Expression.Call(tabControlViewParameter, method, contextConstant);

        return call.Lambda(tabControlViewParameter).Compile();
    }

    public UiContextBuilder AddTabItemFunction<TTabControlView, TContent>(
        string functionName,
        Func<object> header
    )
        where TTabControlView : ITabControlView
        where TContent : notnull
    {
        return AddTabItemFunction<TTabControlView>(
            functionName,
            header,
            () => resolver.Resolve<TContent>()
        );
    }

    public UiContextBuilder AddTabItemFunction<TTabControlView, TContent>(Func<object> header)
        where TTabControlView : ITabControlView
        where TContent : notnull
    {
        var functionName = NameHelper.GetNameAddTabItemFunction(
            typeof(TTabControlView),
            typeof(TContent)
        );

        return AddTabItemFunction<TTabControlView>(
            functionName,
            header,
            () => resolver.Resolve<TContent>()
        );
    }

    public UiContextBuilder AddTabItemFunction<TTabControlView, TContent>()
        where TTabControlView : ITabControlView
        where TContent : notnull
    {
        return AddTabItemFunction<TTabControlView, TContent>(
            NameHelper.GetNameAddTabItemFunction<TTabControlView, TContent>
        );
    }

    public UiContextBuilder AddTabItemFunction<TTabControlView>(
        string functionName,
        Func<object> header,
        Func<object> content
    ) where TTabControlView : ITabControlView
    {
        var tabItemContext = new TabItemContext(header, content);

        return AddTabItemFunction<TTabControlView>(functionName, tabItemContext);
    }

    public UiContextBuilder AddKeyboardKeyGesture<TKeyBindingView>(
        string functionName,
        KeyboardKeyGesture keyboardKeyGesture
    ) where TKeyBindingView : IKeyBindingView
    {
        if (!commandKeyboardKeyGestures.ContainsKey(typeof(TKeyBindingView)))
        {
            commandKeyboardKeyGestures[typeof(TKeyBindingView)] =
                new Dictionary<string, List<KeyboardKeyGesture>>();
        }

        if (!commandKeyboardKeyGestures[typeof(TKeyBindingView)].ContainsKey(functionName))
        {
            commandKeyboardKeyGestures[typeof(TKeyBindingView)][functionName] =
                new List<KeyboardKeyGesture>();
        }

        commandKeyboardKeyGestures[typeof(TKeyBindingView)][functionName].Add(keyboardKeyGesture);

        return this;
    }

    public UiContextBuilder AddContextMenuItem<TContextMenuView>(
        string functionName,
        IEnumerable<KeyValuePair<string, Func<object>>> menuPath
    ) where TContextMenuView : IContextMenuView
    {
        AddContextMenuItemCore(typeof(TContextMenuView), functionName, menuPath);

        return this;
    }

    public UiContextBuilder AddContextMenuItem<TContextMenuView>(
        string functionName,
        params string[] path
    ) where TContextMenuView : IContextMenuView
    {
        var menuPath = GetMenuPath(path);

        return AddContextMenuItem<TContextMenuView>(functionName, menuPath);
    }

    public UiContextBuilder AddContextMenuItem<TContextMenuView>(
        Expression expression,
        params string[] path
    ) where TContextMenuView : IContextMenuView
    {
        var functionName = NameHelper.GetNameFunction(expression);
        var menuPath = GetMenuPath(path);

        return AddContextMenuItem<TContextMenuView>(functionName, menuPath);
    }

    public UiContextBuilder AddMenuItem<TMenuView>(
        string functionName,
        IEnumerable<KeyValuePair<string, Func<object>>> menuPath
    ) where TMenuView : IMenuView
    {
        AddMenuItemCore(typeof(TMenuView), functionName, menuPath);

        return this;
    }

    private void AddMenuItemCore(
        Type menuViewType,
        string functionName,
        IEnumerable<KeyValuePair<string, Func<object>>> menuPath
    )
    {
        if (!menus.ContainsKey(menuViewType))
        {
            menus[menuViewType] = new Dictionary<string, KeyValuePair<string, Func<object>>[]>();
        }

        menus[menuViewType][functionName] = menuPath.ToArray();
    }

    private void AddContextMenuItemCore(
        Type menuViewType,
        string functionName,
        IEnumerable<KeyValuePair<string, Func<object>>> menuPath
    )
    {
        if (!contextMenus.ContainsKey(menuViewType))
        {
            contextMenus[menuViewType] =
                new Dictionary<string, KeyValuePair<string, Func<object>>[]>();
        }

        contextMenus[menuViewType][functionName] = menuPath.ToArray();
    }

    public UiContextBuilder AddMenuItem<TMenuView>(string functionName, params string[] path)
        where TMenuView : IMenuView
    {
        var menuPath = GetMenuPath(path);

        return AddMenuItem<TMenuView>(functionName, menuPath);
    }

    public UiContextBuilder AddMenuItem<TMenuView>(Expression expression, params string[] path)
        where TMenuView : IMenuView
    {
        var functionName = NameHelper.GetNameFunction(expression);
        var menuPath = GetMenuPath(path);

        return AddMenuItem<TMenuView>(functionName, menuPath);
    }

    private Dictionary<
        ClassPropertyPath,
        IEnumerable<PropertyDefaultValue>
    > BuildPropertyDefaultValues()
    {
        var result = new Dictionary<ClassPropertyPath, IEnumerable<PropertyDefaultValue>>();

        foreach (var child in propertyDefaultValues)
        {
            result[child.Key] = child.Value;
        }

        return result;
    }

    private Dictionary<Type, IEnumerable<Func<object>>> BuildChildren()
    {
        var result = new Dictionary<Type, IEnumerable<Func<object>>>();

        foreach (var child in children)
        {
            result[child.Key] = child.Value;
        }

        return result;
    }

    private Dictionary<Type, IEnumerable<CommandContext>> BuildCommands()
    {
        var result = new Dictionary<Type, IEnumerable<CommandContext>>();

        foreach (var command in commandContexts)
        {
            result.Add(command.Key, command.Value.Select(x => x.Value).ToArray());
        }

        return result;
    }

    private Dictionary<Type, IEnumerable<KeyboardKeyBinding>> BuildKeyboardKeyBindings()
    {
        var result = new Dictionary<Type, IEnumerable<KeyboardKeyBinding>>();

        foreach (var type in commandKeyboardKeyGestures)
        {
            var keyboardKeyGestures = new List<KeyboardKeyBinding>();

            foreach (var item in type.Value)
            {
                var command = functions[item.Key];
                keyboardKeyGestures.Add(new KeyboardKeyBinding(command.Command, item.Value));
            }

            result.Add(type.Key, keyboardKeyGestures);
        }

        return result;
    }

    private Dictionary<Type, TreeNode<string, MenuItemContext>[]> BuildMenu()
    {
        var result = new Dictionary<Type, TreeNode<string, MenuItemContext>[]>();

        foreach (var menu in menus)
        {
            var tree = new List<TreeNodeBuilder<string, MenuItemContext>>();

            foreach (var item in menu.Value)
            {
                var command = functions[item.Key];
                CreateMenu(command.Command, item.Value, tree);
            }

            result.Add(menu.Key, tree.Select(x => x.Build()).ToArray());
        }

        return result;
    }

    private Dictionary<Type, TreeNode<string, MenuItemContext>[]> BuildContextMenu()
    {
        var result = new Dictionary<Type, TreeNode<string, MenuItemContext>[]>();

        foreach (var menu in contextMenus)
        {
            var tree = new List<TreeNodeBuilder<string, MenuItemContext>>();

            foreach (var item in menu.Value)
            {
                var command = functions[item.Key];
                CreateMenu(command.Command, item.Value, tree);
            }

            result.Add(menu.Key, tree.Select(x => x.Build()).ToArray());
        }

        return result;
    }

    private MenuItemContext CreateMenuItem(Delegate? task, Func<object> value)
    {
        if (task is null)
        {
            return new MenuItemContext(null, value);
        }

        return new MenuItemContext(task, value);
    }

    private void CreateMenu(
        Delegate task,
        KeyValuePair<string, Func<object>>[] menuPath,
        List<TreeNodeBuilder<string, MenuItemContext>> menu
    )
    {
        if (menuPath.IsEmpty())
        {
            return;
        }

        var root = menuPath[0];
        var item = menu.SingleOrDefault(x => x.Key == root.Key);

        if (item is null)
        {
            item = new TreeNodeBuilder<string, MenuItemContext>()
                .SetKey(root.Key)
                .SetValue(CreateMenuItem(null, root.Value));

            menu.Add(item);
        }

        if (menuPath.Length == 1)
        {
            item.Value = CreateMenuItem(task, root.Value);

            return;
        }

        TreeNodeBuilder<string, MenuItemContext?> currentNode = item!;

        foreach (var key in menuPath[1..^1])
        {
            currentNode = currentNode[key.Key].ThrowIfNull();

            if (currentNode.Value is null)
            {
                currentNode.Value = CreateMenuItem(null, key.Value);
            }
        }

        var last = menuPath.Last();
        var node = currentNode[last.Key].ThrowIfNull();
        node.Value = CreateMenuItem(task, last.Value);
    }

    private IEnumerable<KeyValuePair<string, Func<object>>> GetMenuPath(string[] path)
    {
        return path.Select(x => new KeyValuePair<string, Func<object>>(x, () => x)).ToArray();
    }
}
