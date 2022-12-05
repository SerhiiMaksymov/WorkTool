namespace WorkTool.Core.Modules.CommandLine.Services;

public class CommandLineArgumentParser : IStreamParser<ICommandLineToken, string>
{
    public IEnumerable<ICommandLineToken> Parse(IEnumerable<string> values)
    {
        var context = new CommandLineParserContext();

        foreach (var value in values)
        {
            var token = Parse(value, context);

            if (token is null)
            {
                continue;
            }

            yield return token;
        }
    }

    private ICommandLineToken Parse(string value, CommandLineParserContext context)
    {
        if (value.StartsWith('-'))
        {
            switch (context.PreviousType)
            {
                case CommandLineTokenType.ArgumentName:
                {
                    throw new Exception("Unexpected argument name.");
                }
            }

            context.PreviousType = CommandLineTokenType.ArgumentName;

            return new ArgumentNameCommandLineToken(value);
        }

        switch (context.PreviousType)
        {
            case CommandLineTokenType.Start:
            {
                context.PreviousType = CommandLineTokenType.Name;

                return new NameCommandLineToken(value);
            }
            case CommandLineTokenType.Name:
            {
                context.PreviousType = CommandLineTokenType.Name;

                return new NameCommandLineToken(value);
            }
            case CommandLineTokenType.ArgumentName:
            {
                context.PreviousType = CommandLineTokenType.ArgumentValue;

                return new ArgumentValueCommandLineToken(value);
            }
            case CommandLineTokenType.ArgumentValue:
            {
                throw new Exception("Unexpected argument value.");
            }
            default:
            {
                throw new Exception("Unexpected token.");
            }
        }
    }
}
