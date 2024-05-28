using System;
using System.Text;

namespace Menelabs
{
    public class GraphQlPrettifier : IPrettifyGraphQl
    {
        private enum StateMachine { Fields, FieldSeparator, Args }
        private readonly StringBuilder stringBuilder = new();
        private int indent = 0;
        private bool mustIdent = false;
        private StateMachine stateMachine = StateMachine.Fields;

        public string Prettify(string graphQl)
        {
            Clear();


            for (int i = 0; i < graphQl.Length; i++)
            {
                var ch = graphQl[i];
                if (ch == '{')
                {
                    stateMachine = StateMachine.Fields;
                    // prepend a space on {
                    if (i - 1 > 0 && graphQl[i - 1] != ' ')
                        Append(' ');

                    Append(ch);

                    // and append a newline
                    Indent();
                    AppendLine();
                }
                else if (ch == '}')
                {
                    // prepend a newline
                    Outdent();
                    AppendLine();
                    Append(ch);
                    stateMachine = StateMachine.FieldSeparator;

                }
                else if (ch == ' ')
                {
                    char lastChar = ch;
                    // skip multiple spaces
                    for (i++; i < graphQl.Length; i++)
                    {
                        lastChar = graphQl[i];
                        if (lastChar != ' ')
                            break;
                    }

                    i--;
                    stateMachine = StateMachine.FieldSeparator;
                }
                else if (ch == '(')
                {
                    stateMachine = StateMachine.Args;
                    Append(ch);
                }
                else if (ch == ')')
                {
                    Append(ch);
                    stateMachine = StateMachine.FieldSeparator;
                }
                else if (ch == ':' || ch == ',')
                {
                    Append(ch);
                    Append(' ');
                }
                // skip quoted string
                else if (ch == '"')
                {
                    Append(ch);
                    for (i++; i < graphQl.Length; i++)
                    {
                        Append(graphQl[i]);
                        if (graphQl[i] == '"')
                            break;
                    }
                }
                else if (char.IsLetterOrDigit(ch))
                {
                    if (stateMachine == StateMachine.FieldSeparator)
                    {
                        AppendLine();
                        stateMachine = StateMachine.Fields;
                    }

                    Append(ch);
                }

                else
                {
                    Append(ch);
                }
            }

            return GetPrettyQuery();
        }

        private string GetPrettyQuery()
        {
            return stringBuilder.ToString();
        }

        private void Clear()
        {
            stringBuilder.Clear();
            indent = 0;
            mustIdent = false;
        }
        private void Indent()
        {
            indent++;
        }

        private void Outdent()
        {
            indent--;
        }

        public void AppendLine()
        {
            stringBuilder.AppendLine();
            mustIdent = true;
        }

        public void Append(char query)
        {
            AddIndentation();
            stringBuilder.Append(query);
        }

        public void Append(string query)
        {
            AddIndentation();
            stringBuilder.Append(query);
        }

        private void AddIndentation()
        {
            if (mustIdent)
            {
                for (int i = 0; i < indent; i++)
                {
                    stringBuilder.Append("  ");
                }
                mustIdent = false;
            }
        }
    }
}
