// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Test.Utilities;
using Microsoft.VisualStudio.LanguageServices.CSharp.LanguageService;
using Roslyn.Test.Utilities;
using Roslyn.Utilities;
using Xunit;

namespace Microsoft.VisualStudio.LanguageServices.CSharp.UnitTests.F1Help
{
    [UseExportProvider]
    public class F1HelpTests
    {
        private async Task TestAsync(string markup, string expectedText)
        {
            using (var workspace = TestWorkspace.CreateCSharp(markup))
            {
                var caret = workspace.Documents.First().CursorPosition;

                var service = new CSharpHelpContextService();
                var actualText = await service.GetHelpTermAsync(workspace.CurrentSolution.Projects.First().Documents.First(), workspace.Documents.First().SelectedSpans.First(), CancellationToken.None);
                Assert.Equal(expectedText, actualText);
            }
        }

        private async Task Test_KeywordAsync(string markup, string expectedText)
        {
            await TestAsync(markup, expectedText + "_CSharpKeyword");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestVoid()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await Test_KeywordAsync(
@"class C
{
    vo[||]id goo()
    {
    }
}", "void");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestReturn()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await Test_KeywordAsync(
@"class C
{
    void goo()
    {
        ret[||]urn;
    }
}", "return");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPartialType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await Test_KeywordAsync(
@"part[||]ial class C
{
    partial void goo();
}", "partialtype");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPartialMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await Test_KeywordAsync(
@"partial class C
{
    par[||]tial void goo();
}", "partialmethod");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWhereClause()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await Test_KeywordAsync(
@"using System.Linq;

class Program<T> where T : class
{
    void goo(string[] args)
    {
        var x = from a in args
                whe[||]re a.Length > 0
                select a;
    }
}", "whereclause");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWhereConstraint()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await Test_KeywordAsync(
@"using System.Linq;

class Program<T> wh[||]ere T : class
{
    void goo(string[] args)
    {
        var x = from a in args
                where a.Length > 0
                select a;
    }
}", "whereconstraint");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPreprocessor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"#regi[||]on
#endregion", "#region");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstructor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"namespace N
{
    class C
    {
        void goo()
        {
            var x = new [|C|]();
        }
    }
}", "N.C.#ctor");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenericClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"namespace N
{
    class C<T>
    {
        void goo()
        {
            [|C|]<int> c;
        }
    }
}", "N.C`1");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenericMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"namespace N
{
    class C<T>
    {
        void goo<T, U, V>(T t, U u, V v)
        {
            C<int> c;
            c.g[|oo|](1, 1, 1);
        }
    }
}", "N.C`1.goo``3");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOperator()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"namespace N
{
    class C
    {
        void goo()
        {
            var two = 1 [|+|] 1;
        }
    }", "+_CSharpKeyword");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestVar()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        var[||] x = 3;
    }
}", "var_CSharpKeyword");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEquals()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        var x =[||] 3;
    }
}", "=_CSharpKeyword");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFromIn()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        var x = from n i[||]n {
            1}

        select n
    }
}", "from_CSharpKeyword");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        new UriBuilder().Fragm[||]ent;
    }
}", "System.UriBuilder.Fragment");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForeachIn()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        foreach (var x in[||] {
            1} )
        {
        }
    }
}", "in_CSharpKeyword");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestRegionDescription()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Program
{
    static void Main(string[] args)
    {
        #region Begin MyR[||]egion for testing
        #endregion End
    }
}", "#region");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenericAngle()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Program
{
    static void generic<T>(T t)
    {
        generic[||]<int>(0);
    }
}", "Program.generic``1");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLocalReferenceIsType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        int x;
        x[||];
    }
}", "System.Int32");
        }

        [WorkItem(864266, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/864266")]
        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstantField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Program
{
    static void Main(string[] args)
    {
        var i = int.Ma[||]xValue;
    }
}", "System.Int32.MaxValue");
        }

        [WorkItem(862420, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/862420")]
        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestParameter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Class2
{
    void M1(int par[||]ameter)  // 1
    {
    }

    void M2()
    {
        int argument = 1;
        M1(parameter: argument);   // 2
    }
}", "System.Int32");
        }

        [WorkItem(862420, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/862420")]
        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArgumentType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Class2
{
    void M1(int pa[||]rameter)  // 1
    {
    }

    void M2()
    {
        int argument = 1;
        M1(parameter: argument);   // 2
    }
}", "System.Int32");
        }

        [WorkItem(862396, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/862396")]
        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNoToken()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Program
{
    static void Main(string[] args)
    {
    }
}[||]", "");
        }

        [WorkItem(862328, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/862328")]
        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLiteral()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Program
{
    static void Main(string[] args)
    {
        Main(new string[] { ""fo[||]o"" });
    }
}", "System.String");
        }

        [WorkItem(862478, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/862478")]
        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestColonColon()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        global:[||]:System.Console.Write("");
    }
}", "::_CSharpKeyword");
        }

        [WorkItem(864658, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/864658")]
        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNullable()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        int?[||] a = int.MaxValue;
        a.Value.GetHashCode();
    }
}", "System.Nullable`1");
        }

        [WorkItem(863517, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/863517")]
        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterLastToken()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        foreach (char var in ""!!!"")$$[||]
        {
        }
    }
}", "");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConditional()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Program
{
    static void Main(string[] args)
    {
        var x = true [|?|] true : false;
    }
}", "?_CSharpKeyword");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLocalVar()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        var a = 0;
        int v[||]ar = 1;
    }
}", "System.Int32");
        }

        [WorkItem(867574, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/867574")]
        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFatArrow()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        var a = new System.Action(() =[||]> {
        });
    }
}", "=>_CSharpKeyword");
        }

        [WorkItem(867572, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/867572")]
        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSubscription()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class CCC
{
    event System.Action e;

    void M()
    {
        e +[||]= () => {
        };
    }
}", "CCC.e.add");
        }

        [WorkItem(867554, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/867554")]
        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(@"// some comm[||]ents here", "comments");
        }

        [WorkItem(867529, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/867529")]
        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDynamic()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    void M()
    {
        dyna[||]mic d = 0;
    }
}", "dynamic_CSharpKeyword");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.F1Help)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestRangeVariable()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        var zzz = from y in args
                  select [||]y;
    }
}", "System.String");
        }
    }
}
