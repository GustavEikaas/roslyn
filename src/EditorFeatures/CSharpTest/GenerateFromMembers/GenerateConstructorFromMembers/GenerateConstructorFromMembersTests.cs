// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CodeStyle;
using Microsoft.CodeAnalysis.CSharp.CodeStyle;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.CodeRefactorings;
using Microsoft.CodeAnalysis.GenerateConstructorFromMembers;
using Microsoft.CodeAnalysis.PickMembers;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.GenerateConstructorFromMembers
{
    public class GenerateConstructorFromMembersTests : AbstractCSharpCodeActionTest
    {
        protected override CodeRefactoringProvider CreateCodeRefactoringProvider(Workspace workspace, TestParameters parameters)
            => new GenerateConstructorFromMembersCodeRefactoringProvider((IPickMembersService)parameters.fixProviderData);

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Z
{
    [|int a;|]
}",
@"using System.Collections.Generic;

class Z
{
    int a;

    public Z(int a{|Navigation:)|}
    {
        this.a = a;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleFieldWithCodeStyle()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Z
{
    [|int a;|]
}",
@"using System.Collections.Generic;

class Z
{
    int a;

    public Z(int a{|Navigation:)|} => this.a = a;
}",
options: Option(CSharpCodeStyleOptions.PreferExpressionBodiedConstructors, CSharpCodeStyleOptions.WhenPossibleWithSilentEnforcement));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseExpressionBodyWhenOnSingleLine_AndIsSingleLine()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Z
{
    [|int a;|]
}",
@"using System.Collections.Generic;

class Z
{
    int a;

    public Z(int a{|Navigation:)|} => this.a = a;
}",
options: Option(CSharpCodeStyleOptions.PreferExpressionBodiedConstructors, CSharpCodeStyleOptions.WhenOnSingleLineWithSilentEnforcement));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseExpressionBodyWhenOnSingleLine_AndIsNotSingleLine()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Z
{
    [|int a;
    int b;|]
}",
@"using System.Collections.Generic;

class Z
{
    int a;
    int b;

    public Z(int a, int b{|Navigation:)|}
    {
        this.a = a;
        this.b = b;
    }
}",
options: Option(CSharpCodeStyleOptions.PreferExpressionBodiedConstructors, CSharpCodeStyleOptions.WhenOnSingleLineWithSilentEnforcement));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMultipleFields()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Z
{
    [|int a;
    string b;|]
}",
@"using System.Collections.Generic;

class Z
{
    int a;
    string b;

    public Z(int a, string b{|Navigation:)|}
    {
        this.a = a;
        this.b = b;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSecondField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Z
{
    int a;
    [|string b;|]

    public Z(int a)
    {
        this.a = a;
    }
}",
@"using System.Collections.Generic;

class Z
{
    int a;
    string b;

    public Z(int a)
    {
        this.a = a;
    }

    public Z(string b{|Navigation:)|}
    {
        this.b = b;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFieldAssigningConstructor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Z
{
    [|int a;
    string b;|]

    public Z(int a)
    {
        this.a = a;
    }
}",
@"using System.Collections.Generic;

class Z
{
    int a;
    string b;

    public Z(int a)
    {
        this.a = a;
    }

    public Z(int a, string b{|Navigation:)|}
    {
        this.a = a;
        this.b = b;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFieldAssigningConstructor2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Z
{
    [|int a;
    string b;|]

    public Z(int a)
    {
        this.a = a;
    }
}",
@"using System.Collections.Generic;

class Z
{
    int a;
    string b;

    public Z(int a)
    {
        this.a = a;
    }

    public Z(int a, string b{|Navigation:)|}
    {
        this.a = a;
        this.b = b;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDelegatingConstructor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Z
{
    [|int a;
    string b;|]

    public Z(int a)
    {
        this.a = a;
    }
}",
@"using System.Collections.Generic;

class Z
{
    int a;
    string b;

    public Z(int a)
    {
        this.a = a;
    }

    public Z(int a, string b{|Navigation:)|} : this(a)
    {
        this.b = b;
    }
}",
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingWithExistingConstructor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Z
{
    [|int a;
    string b;|]

    public Z(int a)
    {
        this.a = a;
    }

    public Z(int a, string b{|Navigation:)|}
    {
        this.a = a;
        this.b = b;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMultipleProperties()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Z
{
    [|public int A { get; private set; }
    public string B { get; private set; }|]
}",
@"class Z
{
    public Z(int a, string b{|Navigation:)|}
    {
        A = a;
        B = b;
    }

    public int A { get; private set; }
    public string B { get; private set; }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMultiplePropertiesWithQualification()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Z
{
    [|public int A { get; private set; }
    public string B { get; private set; }|]
}",
@"class Z
{
    public Z(int a, string b{|Navigation:)|}
    {
        this.A = a;
        this.B = b;
    }

    public int A { get; private set; }
    public string B { get; private set; }
}", options: Option(CodeStyleOptions.QualifyPropertyAccess, true, NotificationOption.Error));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestStruct()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

struct S
{
    [|int i;|]
}",
@"using System.Collections.Generic;

struct S
{
    int i;

    public S(int i{|Navigation:)|}
    {
        this.i = i;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestStruct1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

struct S
{
    [|int i { get; set; }|]
}",
@"using System.Collections.Generic;

struct S
{
    public S(int i{|Navigation:)|} : this()
    {
        this.i = i;
    }

    int i { get; set; }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestStruct2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

struct S
{
    int i { get; set; }

    [|int y;|]
}",
@"using System.Collections.Generic;

struct S
{
    int i { get; set; }

    int y;

    public S(int y{|Navigation:)|} : this()
    {
        this.y = y;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestStruct3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

struct S
{
    [|int i { get; set; }|]

    int y;
}",
@"using System.Collections.Generic;

struct S
{
    int i { get; set; }

    int y;

    public S(int i{|Navigation:)|} : this()
    {
        this.i = i;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenericType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Program<T>
{
    [|int i;|]
}",
@"using System.Collections.Generic;

class Program<T>
{
    int i;

    public Program(int i{|Navigation:)|}
    {
        this.i = i;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSmartTagText1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestSmartTagTextAsync(
@"using System.Collections.Generic;

class Program
{
    [|bool b;
    HashSet<string> s;|]
}",
string.Format(FeaturesResources.Generate_constructor_0_1, "Program", "bool, HashSet<string>"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSmartTagText2()
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
string.Format(FeaturesResources.Generate_field_assigning_constructor_0_1, "Program", "bool, HashSet<string>"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSmartTagText3()
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
string.Format(FeaturesResources.Generate_delegating_constructor_0_1, "Program", "bool, HashSet<string>"),
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestContextualKeywordName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    [|int yield;|]
}",
@"class Program
{
    int yield;

    public Program(int yield{|Navigation:)|}
    {
        this.yield = yield;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateConstructorNotOfferedForDuplicate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class X
{
    public X(string v)
    {
    }

    static void Test()
    {
        new X(new [|string|]());
    }
}");
        }


        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Tuple()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Z
{
    [|(int, string) a;|]
}",
@"using System.Collections.Generic;

class Z
{
    (int, string) a;

    public Z((int, string) a{|Navigation:)|}
    {
        this.a = a;
    }
}");
        }

        [WorkItem(14219, "https://github.com/dotnet/roslyn/issues/14219")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnderscoreInName1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    [|int _field;|]
}",
@"class Program
{
    int _field;

    public Program(int field{|Navigation:)|}
    {
        _field = field;
    }
}");
        }

        [WorkItem(14219, "https://github.com/dotnet/roslyn/issues/14219")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnderscoreInName_PreferThis()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    [|int _field;|]
}",
@"class Program
{
    int _field;

    public Program(int field{|Navigation:)|}
    {
        this._field = field;
    }
}",
options: Option(CodeStyleOptions.QualifyFieldAccess, CodeStyleOptions.TrueWithSuggestionEnforcement));
        }

        [WorkItem(13944, "https://github.com/dotnet/roslyn/issues/13944")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGetter_Only_Auto_Props()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"abstract class Contribution
{
  [|public string Title { get; }
    public int Number { get; }|]
}",
@"abstract class Contribution
{
    protected Contribution(string title, int number{|Navigation:)|}
    {
        Title = title;
        Number = number;
    }

    public string Title { get; }
    public int Number { get; }
}",
options: Option(CodeStyleOptions.QualifyFieldAccess, CodeStyleOptions.TrueWithSuggestionEnforcement));
        }

        [WorkItem(13944, "https://github.com/dotnet/roslyn/issues/13944")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAbstract_Getter_Only_Auto_Props()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"abstract class Contribution
{
  [|public abstract string Title { get; }
    public int Number { get; }|]
}",
new TestParameters(options: Option(CodeStyleOptions.QualifyFieldAccess, CodeStyleOptions.TrueWithSuggestionEnforcement)));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleFieldWithDialog()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithPickMembersDialogAsync(
@"using System.Collections.Generic;

class Z
{
    int a;
    [||]
}",
@"using System.Collections.Generic;

class Z
{
    int a;

    public Z(int a{|Navigation:)|}
    {
        this.a = a;
    }
}",
chosenSymbols: new[] { "a" });
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleFieldWithDialog2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithPickMembersDialogAsync(
@"using System.Collections.Generic;

class [||]Z
{
    int a;
}",
@"using System.Collections.Generic;

class Z
{
    int a;

    public Z(int a{|Navigation:)|}
    {
        this.a = a;
    }
}",
chosenSymbols: new[] { "a" });
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingOnClassAttributes()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System.Collections.Generic;

[X][||]
class Z
{
    int a;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPickNoFieldWithDialog()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithPickMembersDialogAsync(
@"using System.Collections.Generic;

class Z
{
    int a;
    [||]
}",
@"using System.Collections.Generic;

class Z
{
    int a;

    public Z({|Navigation:)|}
    {
    }
}",
chosenSymbols: new string[] { });
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestReorderFieldsWithDialog()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithPickMembersDialogAsync(
@"using System.Collections.Generic;

class Z
{
    int a;
    string b;
    [||]
}",
@"using System.Collections.Generic;

class Z
{
    int a;
    string b;

    public Z(string b, int a{|Navigation:)|}
    {
        this.b = b;
        this.a = a;
    }
}",
chosenSymbols: new string[] { "b", "a" });
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddNullChecks1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithPickMembersDialogAsync(
@"
using System;
using System.Collections.Generic;

class Z
{
    int a;
    string b;
    [||]
}",
@"
using System;
using System.Collections.Generic;

class Z
{
    int a;
    string b;

    public Z(int a, string b{|Navigation:)|}
    {
        this.a = a;
        this.b = b ?? throw new ArgumentNullException(nameof(b));
    }
}",
chosenSymbols: new string[] { "a", "b" }, 
optionsCallback: options => options[0].Value = true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddNullChecks2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithPickMembersDialogAsync(
@"
using System;
using System.Collections.Generic;

class Z
{
    int a;
    string b;
    [||]
}",
@"
using System;
using System.Collections.Generic;

class Z
{
    int a;
    string b;

    public Z(int a, string b{|Navigation:)|}
    {
        if (b == null)
        {
            throw new ArgumentNullException(nameof(b));
        }

        this.a = a;
        this.b = b;
    }
}",
chosenSymbols: new string[] { "a", "b" },
optionsCallback: options => options[0].Value = true,
parameters: new TestParameters(options:
    Option(CodeStyleOptions.PreferThrowExpression, CodeStyleOptions.FalseWithSilentEnforcement)));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingOnMember1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Z
{
    int a;
    string b;
    [||]public void M() { }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingOnMember2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Z
{
    int a;
    string b;
    public void M()
    {
    }[||]

    public void N() { }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingOnMember3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Z
{
    int a;
    string b;
    public void M()
    {
 [||] 
    }

    public void N() { }
}");
        }

        [WorkItem(21067, "https://github.com/dotnet/roslyn/pull/21067")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFinalCaretPosition()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Z
{
    [|int a;|]
}",
@"using System.Collections.Generic;

class Z
{
    int a;

    public Z(int a{|Navigation:)|}
    {
        this.a = a;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
        [WorkItem(20595, "https://github.com/dotnet/roslyn/issues/20595")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ProtectedConstructorShouldBeGeneratedForAbstractClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"abstract class C 
{
    [|public int Prop { get; set; }|]
}",
@"abstract class C 
{
    protected C(int prop{|Navigation:)|}
    {
        Prop = prop;
    }

    public int Prop { get; set; }
}",
options: Option(CodeStyleOptions.QualifyFieldAccess, CodeStyleOptions.TrueWithSuggestionEnforcement));
        }

        [WorkItem(17643, "https://github.com/dotnet/roslyn/issues/17643")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithDialogNoBackingField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithPickMembersDialogAsync(
@"
class Program
{
    public int F { get; set; }
    [||]
}",
@"
class Program
{
    public int F { get; set; }

    public Program(int f{|Navigation:)|}
    {
        F = f;
    }
}",
chosenSymbols: null);
        }

        [WorkItem(25690, "https://github.com/dotnet/roslyn/issues/25690")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateConstructorFromMembers)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithDialogNoIndexer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithPickMembersDialogAsync(
@"
class Program
{
    public int P { get => 0; set { } }
    public int this[int index] { get => 0; set { } }
    [||]
}",
@"
class Program
{
    public int P { get => 0; set { } }
    public int this[int index] { get => 0; set { } }

    public Program(int p{|Navigation:)|}
    {
        P = p;
    }
}",
chosenSymbols: null);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateEqualsAndGetHashCode)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithDialogSetterOnlyProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithPickMembersDialogAsync(
@"
class Program
{
    public int P { get => 0; set { } }
    public int S { set { } }
    [||]
}",
@"
class Program
{
    public int P { get => 0; set { } }
    public int S { set { } }

    public Program(int p, int s{|Navigation:)|}
    {
        P = p;
        S = s;
    }
}",
chosenSymbols: null);
        }
    }
}
