// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.AddConstructorParametersFromMembers;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.CodeRefactorings;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.GenerateFromMembers.AddConstructorParameters
{
    public class AddConstructorParametersFromMembersTests : AbstractCSharpCodeActionTest
    {
        protected override CodeRefactoringProvider CreateCodeRefactoringProvider(Workspace workspace, TestParameters parameters)
            => new AddConstructorParametersFromMembersCodeRefactoringProvider();

        [WorkItem(308077, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/308077")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddConstructorParametersFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAdd1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Program
{
    [|int i;
    string s;|]

    public Program(int i)
    {
        this.i = i;
    }
}",
@"using System.Collections.Generic;

class Program
{
    int i;
    string s;

    public Program(int i, string s)
    {
        this.i = i;
        this.s = s;
    }
}");
        }

        [WorkItem(308077, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/308077")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddConstructorParametersFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddOptional1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Program
{
    [|int i;
    string s;|]

    public Program(int i)
    {
        this.i = i;
    }
}",
@"using System.Collections.Generic;

class Program
{
    int i;
    string s;

    public Program(int i, string s = null)
    {
        this.i = i;
        this.s = s;
    }
}",
index: 1);
        }

        [WorkItem(308077, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/308077")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddConstructorParametersFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddToConstructorWithMostMatchingParameters1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Program
{
    [|int i;
    string s;
    bool b;|]

    public Program(int i)
    {
        this.i = i;
    }

    public Program(int i, string s) : this(i)
    {
        this.s = s;
    }
}",
@"using System.Collections.Generic;

class Program
{
    int i;
    string s;
    bool b;

    public Program(int i)
    {
        this.i = i;
    }

    public Program(int i, string s, bool b) : this(i)
    {
        this.s = s;
        this.b = b;
    }
}");
        }

        [WorkItem(308077, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/308077")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddConstructorParametersFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddOptionalToConstructorWithMostMatchingParameters1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Program
{
    [|int i;
    string s;
    bool b;|]

    public Program(int i)
    {
        this.i = i;
    }

    public Program(int i, string s) : this(i)
    {
        this.s = s;
    }
}",
@"using System.Collections.Generic;

class Program
{
    int i;
    string s;
    bool b;

    public Program(int i)
    {
        this.i = i;
    }

    public Program(int i, string s, bool b = false) : this(i)
    {
        this.s = s;
        this.b = b;
    }
}",
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddConstructorParametersFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSmartTagDisplayText1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestSmartTagTextAsync(
@"using System.Collections.Generic;

class Program
{
    [|bool b;
    HashSet<string> s;|]

    public Program(bool b)
    {
        this.b = b;
    }
}",
string.Format(FeaturesResources.Add_parameters_to_0, "Program(bool)"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddConstructorParametersFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSmartTagDisplayText2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestSmartTagTextAsync(
@"using System.Collections.Generic;

class Program
{
    [|bool b;
    HashSet<string> s;|]

    public Program(bool b)
    {
        this.b = b;
    }
}",
string.Format(FeaturesResources.Add_optional_parameters_to_0, "Program(bool)"),
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddConstructorParametersFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTuple()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    [|(int, string) i;
    (string, int) s;|]

    public Program((int, string) i)
    {
        this.i = i;
    }
}",
@"class Program
{
    (int, string) i;
    (string, int) s;

    public Program((int, string) i, (string, int) s)
    {
        this.i = i;
        this.s = s;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddConstructorParametersFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTupleWithNames()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    [|(int a, string b) i;
    (string c, int d) s;|]

    public Program((int a, string b) i)
    {
        this.i = i;
    }
}",
@"class Program
{
    (int a, string b) i;
    (string c, int d) s;

    public Program((int a, string b) i, (string c, int d) s)
    {
        this.i = i;
        this.s = s;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddConstructorParametersFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTupleWithDifferentNames()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    [|(int a, string b) i;
    (string c, int d) s;|]

    public Program((int e, string f) i)
    {
        this.i = i;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddConstructorParametersFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTupleOptional()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    [|(int, string) i;
    (string, int) s;|]

    public Program((int, string) i)
    {
        this.i = i;
    }
}",
@"class Program
{
    (int, string) i;
    (string, int) s;

    public Program((int, string) i, (string, int) s = default((string, int)))
    {
        this.i = i;
        this.s = s;
    }
}",
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddConstructorParametersFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTupleOptionalWithNames()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    [|(int a, string b) i;
    (string c, int d) s;|]

    public Program((int a, string b) i)
    {
        this.i = i;
    }
}",
@"class Program
{
    (int a, string b) i;
    (string c, int d) s;

    public Program((int a, string b) i, (string c, int d) s = default((string c, int d)))
    {
        this.i = i;
        this.s = s;
    }
}",
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddConstructorParametersFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTupleOptionalWithDifferentNames()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    [|(int a, string b) i;
    (string c, int d) s;|]

    public Program((int e, string f) i)
    {
        this.i = i;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddConstructorParametersFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTupleWithNullable()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    [|(int?, bool?) i;
    (byte?, long?) s;|]

    public Program((int?, bool?) i)
    {
        this.i = i;
    }
}",
@"class Program
{
    (int?, bool?) i;
    (byte?, long?) s;

    public Program((int?, bool?) i, (byte?, long?) s)
    {
        this.i = i;
        this.s = s;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddConstructorParametersFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTupleWithGenericss()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    [|(List<int>, List<bool>) i;
    (List<byte>, List<long>) s;|]

    public Program((List<int>, List<bool>) i)
    {
        this.i = i;
    }
}",
@"class Program
{
    (List<int>, List<bool>) i;
    (List<byte>, List<long>) s;

    public Program((List<int>, List<bool>) i, (List<byte>, List<long>) s)
    {
        this.i = i;
        this.s = s;
    }
}");
        }
    }
}
