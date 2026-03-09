// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.UseCollectionInitializer;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.UseCollectionInitializer
{
    public partial class UseCollectionInitializerTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (new CSharpUseCollectionInitializerDiagnosticAnalyzer(),
                new CSharpUseCollectionInitializerCodeFixProvider());

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnVariableDeclarator()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class C
{
    void M()
    {
        var c = [||]new List<int>();
        c.Add(1);
    }
}",
@"using System.Collections.Generic;

class C
{
    void M()
    {
        var c = new List<int>
        {
            1
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIndexAccess1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System.Collections.Generic;
class C
{
    void M()
    {
        var c = [||]new List<int>();
        c[1] = 2;
    }
}",
@"
using System.Collections.Generic;
class C
{
    void M()
    {
        var c = new List<int>
        {
            [1] = 2
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIndexAccess1_NotInCSharp5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"
using System.Collections.Generic;
class C
{
    void M()
    {
        var c = [||]new List<int>();
        c[1] = 2;
    }
}", new TestParameters(parseOptions: CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp5)));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestComplexIndexAccess1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System.Collections.Generic;
class C
{
    void M()
    {
        a.b.c = [||]new List<int>();
        a.b.c[1] = 2;
    }
}",
@"
using System.Collections.Generic;
class C
{
    void M()
    {
        a.b.c = new List<int>
        {
            [1] = 2
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIndexAccess2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System.Collections.Generic;
class C
{
    void M()
    {
        var c = [||]new List<int>();
        c[1] = 2;
        c[2] = """";
    }
}",
@"
using System.Collections.Generic;
class C
{
    void M()
    {
        var c = new List<int>
        {
            [1] = 2,
            [2] = """"
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIndexAccess3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System.Collections.Generic;
class C
{
    void M()
    {
        var c = [||]new List<int>();
        c[1] = 2;
        c[2] = """";
        c[3, 4] = 5;
    }
}",
@"
using System.Collections.Generic;
class C
{
    void M()
    {
        var c = new List<int>
        {
            [1] = 2,
            [2] = """",
            [3, 4] = 5
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIndexFollowedByInvocation()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System.Collections.Generic;
class C
{
    void M()
    {
        var c = [||]new List<int>();
        c[1] = 2;
        c.Add(0);
    }
}",
@"
using System.Collections.Generic;
class C
{
    void M()
    {
        var c = new List<int>
        {
            [1] = 2
        };
        c.Add(0);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInvocationFollowedByIndex()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System.Collections.Generic;
class C
{
    void M()
    {
        var c = [||]new List<int>();
        c.Add(0);
        c[1] = 2;
    }
}",
@"
using System.Collections.Generic;
class C
{
    void M()
    {
        var c = new List<int>
        {
            0
        };
        c[1] = 2;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithInterimStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class C
{
    void M()
    {
        var c = [||]new List<int>();
        c.Add(1);
        c.Add(2);
        throw new Exception();
        c.Add(3);
        c.Add(4);
    }
}",
@"using System.Collections.Generic;

class C
{
    void M()
    {
        var c = new List<int>
        {
            1,
            2
        };
        throw new Exception();
        c.Add(3);
        c.Add(4);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingBeforeCSharp3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {

            await TestMissingAsync(
@"using System.Collections.Generic;

class C
{
    void M()
    {
        var c = [||]new List<int>();
        c.Add(1);
    }
}", new TestParameters(parseOptions: CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp2)));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingOnNonIEnumerable()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System.Collections.Generic;

class C
{
    void M()
    {
        var c = [||]new C();
        c.Add(1);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingOnNonIEnumerableEvenWithAdd()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System.Collections.Generic;

class C
{
    void M()
    {
        var c = [||]new C();
        c.Add(1);
    }

    public void Add(int i)
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithCreationArguments()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class C
{
    void M()
    {
        var c = [||]new List<int>(1);
        c.Add(1);
    }
}",
@"using System.Collections.Generic;

class C
{
    void M()
    {
        var c = new List<int>(1)
        {
            1
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnAssignmentExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class C
{
    void M()
    {
        List<int> c = null;
        c = [||]new List<int>();
        c.Add(1);
    }
}",
@"using System.Collections.Generic;

class C
{
    void M()
    {
        List<int> c = null;
        c = new List<int>
        {
            1
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingOnRefAdd()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System.Collections.Generic;

class C
{
    void M()
    {
        var c = [||]new List<int>();
        c.Add(ref i);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestComplexInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class C
{
    void M()
    {
        List<int>[] array;
        array[0] = [||]new List<int>();
        array[0].Add(1);
        array[0].Add(2);
    }
}",
@"using System.Collections.Generic;

class C
{
    void M()
    {
        List<int>[] array;
        array[0] = new List<int>
        {
            1,
            2
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnNamedArg()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System.Collections.Generic;

class C
{
    void M()
    {
        var c = [||]new List<int>();
        c.Add(arg: 1);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingWithExistingInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System.Collections.Generic;

class C
{
    void M()
    {
        var c = [||]new List<int>() { 1 };
        c.Add(1);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAllInDocument1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class C
{
    void M()
    {
        List<int>[] array;
        array[0] = {|FixAllInDocument:new|} List<int>();
        array[0].Add(1);
        array[0].Add(2);
        array[1] = new List<int>();
        array[1].Add(3);
        array[1].Add(4);
    }
}",
@"using System.Collections.Generic;

class C
{
    void M()
    {
        List<int>[] array;
        array[0] = new List<int>
        {
            1,
            2
        };
        array[1] = new List<int>
        {
            3,
            4
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAllInDocument2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class C
{
    void M()
    {
        var list1 = {|FixAllInDocument:new|} List<int>(() => {
            var list2 = new List<int>();
            list2.Add(2);
        });
        list1.Add(1);
    }
}",
@"using System.Collections.Generic;

class C
{
    void M()
    {
        var list1 = new List<int>(() =>
        {
            var list2 = new List<int>
            {
                2
            };
        })
        {
            1
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAllInDocument3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class C
{
    void M()
    {
        var list1 = {|FixAllInDocument:new|} List<int>();
        list1.Add(() => {
            var list2 = new List<int>();
            list2.Add(2);
        });
    }
}",
@"using System.Collections.Generic;

class C
{
    void M()
    {
        var list1 = new List<int>
        {
            () =>
            {
                var list2 = new List<int>
                {
                    2
                };
            }
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTrivia1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System.Collections.Generic;
class C
{
    void M()
    {
        var c = [||]new List<int>();
        c.Add(1); // Goo
        c.Add(2); // Bar
    }
}",
@"
using System.Collections.Generic;
class C
{
    void M()
    {
        var c = new List<int>
        {
            1, // Goo
            2 // Bar
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestComplexInitializer2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class C
{
    void M()
    {
        var c = new [||]Dictionary<int, string>();
        c.Add(1, ""x"");
        c.Add(2, ""y"");
    }
}",
@"using System.Collections.Generic;

class C
{
    void M()
    {
        var c = new Dictionary<int, string>
        {
            { 1, ""x"" },
            { 2, ""y"" }
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
        [WorkItem(16158, "https://github.com/dotnet/roslyn/issues/16158")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIncorrectAddName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

public class Goo
{
    public static void Bar()
    {
        string item = null;
        var items = new List<string>();

        var values = new [||]List<string>(); // Collection initialization can be simplified
        values.Add(item);
        values.AddRange(items);
    }
}",
@"using System.Collections.Generic;

public class Goo
{
    public static void Bar()
    {
        string item = null;
        var items = new List<string>();

        var values = new List<string>
        {
            item
        }; // Collection initialization can be simplified
        values.AddRange(items);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
        [WorkItem(16241, "https://github.com/dotnet/roslyn/issues/16241")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedCollectionInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
        using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        var myStringArray = new string[] { ""Test"", ""123"", ""ABC"" };
        var myStringList = myStringArray?.ToList() ?? new [||]List<string>();
        myStringList.Add(""Done"");
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
        [WorkItem(17823, "https://github.com/dotnet/roslyn/issues/17823")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingWhenReferencedInInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
using System.Collections.Generic;

class C
{
    static void M()
    {
        var items = new [||]List<object>();
        items[0] = items[0];
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
        [WorkItem(17823, "https://github.com/dotnet/roslyn/issues/17823")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWhenReferencedInInitializer_LocalVar()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
using System.Collections.Generic;

class C
{
    static void M()
    {
        var items = new [||]List<object>();
        items[0] = 1;
        items[1] = items[0];
    }
}",
@"
using System.Collections.Generic;

class C
{
    static void M()
    {
        var items = new [||]List<object>
        {
            [0] = 1
        };
        items[1] = items[0];
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
        [WorkItem(17823, "https://github.com/dotnet/roslyn/issues/17823")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWhenReferencedInInitializer_LocalVar2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
using System.Collections.Generic;
using System.Linq;

class C
{
    void M()
    {
        var t = [||]new List<int>(new int[] { 1, 2, 3 });
        t.Add(t.Min() - 1);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
        [WorkItem(18260, "https://github.com/dotnet/roslyn/issues/18260")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWhenReferencedInInitializer_Assignment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
using System.Collections.Generic;

class C
{
    static void M()
    {
        List<object> items = null;
        items = new [||]List<object>();
        items[0] = 1;
        items[1] = items[0];
    }
}",
@"
using System.Collections.Generic;

class C
{
    static void M()
    {
        List<object> items = null;
        items = new [||]List<object>
        {
            [0] = 1
        };
        items[1] = items[0];
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
        [WorkItem(18260, "https://github.com/dotnet/roslyn/issues/18260")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWhenReferencedInInitializer_Assignment2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System.Collections.Generic;
using System.Linq;

class C
{
    void M()
    {
        List<int> t = null;
        t = [||]new List<int>(new int[] { 1, 2, 3 });
        t.Add(t.Min() - 1);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
        [WorkItem(18260, "https://github.com/dotnet/roslyn/issues/18260")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFieldReference()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System.Collections.Generic;

class C
{
    private List<int> myField;
    void M()
    {
        myField = [||]new List<int>();
        myField.Add(this.myField.Count);
    }
}");
        }

        [WorkItem(17853, "https://github.com/dotnet/roslyn/issues/17853")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingForDynamic()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System.Dynamic;

class C
{
    void Goo()
    {
        dynamic body = [||]new ExpandoObject();
        body[0] = new ExpandoObject();
    }
}");
        }

        [WorkItem(17953, "https://github.com/dotnet/roslyn/issues/17953")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingAcrossPreprocessorDirective()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
using System.Collections.Generic;

public class Goo
{
    public void M()
    {
        var items = new [||]List<object>();
#if true
        items.Add(1);
#endif
    }
}");
        }

        [WorkItem(17953, "https://github.com/dotnet/roslyn/issues/17953")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAvailableInsidePreprocessorDirective()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
using System.Collections.Generic;

public class Goo
{
    public void M()
    {
#if true
        var items = new [||]List<object>();
        items.Add(1);
#endif
    }
}",
@"
using System.Collections.Generic;

public class Goo
{
    public void M()
    {
#if true
        var items = new List<object>
        {
            1
        };
#endif
    }
}");
        }

        [WorkItem(18242, "https://github.com/dotnet/roslyn/issues/18242")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestObjectInitializerAssignmentAmbiguity()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
using System.Collections.Generic;

public class Goo
{
    public void M()
    {
        int lastItem;
        var list = [||]new List<int>();
        list.Add(lastItem = 5);
    }
}",
@"
using System.Collections.Generic;

public class Goo
{
    public void M()
    {
        int lastItem;
        var list = new List<int>
        {
            (lastItem = 5)
        };
    }
}");
        }

        [WorkItem(18242, "https://github.com/dotnet/roslyn/issues/18242")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestObjectInitializerCompoundAssignment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
using System.Collections.Generic;

public class Goo
{
    public void M()
    {
        int lastItem = 0;
        var list = [||]new List<int>();
        list.Add(lastItem += 5);
    }
}",
@"
using System.Collections.Generic;

public class Goo
{
    public void M()
    {
        int lastItem = 0;
        var list = new List<int>
        {
            (lastItem += 5)
        };
    }
}");
        }

        [WorkItem(19253, "https://github.com/dotnet/roslyn/issues/19253")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestKeepBlankLinesAfter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
using System.Collections.Generic;

class MyClass
{
    public void Main()
    {
        var list = [||]new List<int>();
        list.Add(1);

        int horse = 1;
    }
}",
@"
using System.Collections.Generic;

class MyClass
{
    public void Main()
    {
        var list = new List<int>
        {
            1
        };

        int horse = 1;
    }
}");
        }

        [WorkItem(23672, "https://github.com/dotnet/roslyn/issues/23672")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseCollectionInitializer)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingWithExplicitImplementedAddMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
using System.Collections.Generic;
using System.Dynamic;

public class Goo
{
    public void M()
    {
        IDictionary<string, object> obj = [||]new ExpandoObject();
        obj.Add(""string"", ""v"");
        obj.Add(""int"", 1);
        obj.Add("" object"", new { X = 1, Y = 2 });
        }
}");
        }
    }
}
