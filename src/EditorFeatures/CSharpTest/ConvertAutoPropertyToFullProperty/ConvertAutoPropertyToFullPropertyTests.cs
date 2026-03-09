// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp.ConvertAutoPropertyToFullProperty;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.CodeRefactorings;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Test.Utilities;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.ConvertAutoPropertyToFullProperty
{
    public partial class ConvertAutoPropertyToFullPropertyTests : AbstractCSharpCodeActionTest
    {
        protected override CodeRefactoringProvider CreateCodeRefactoringProvider(Workspace workspace, TestParameters parameters)
            => new CSharpConvertAutoPropertyToFullPropertyCodeRefactoringProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SimpleAutoPropertyTest()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public int G[||]oo { get; set; }
}
";
            var expected = @"
class goo
{
    private int _goo;

    public int Goo
    {
        get
        {
            return _goo;
        }
        set
        {
            _goo = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExtraLineAfterProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public int G[||]oo { get; set; }

}
";
            var expected = @"
class goo
{
    private int _goo;

    public int Goo
    {
        get
        {
            return _goo;
        }
        set
        {
            _goo = value;
        }
    }

}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WithInitialValue()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public int G[||]oo { get; set; } = 2
}
";
            var expected = @"
class goo
{
    private int _goo = 2;

    public int Goo
    {
        get
        {
            return _goo;
        }
        set
        {
            _goo = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WithCalculatedInitialValue()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    const int num = 345;
    public int G[||]oo { get; set; } = 2*num
}
";
            var expected = @"
class goo
{
    const int num = 345;
    private int _goo = 2 * num;

    public int Goo
    {
        get
        {
            return _goo;
        }
        set
        {
            _goo = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WithPrivateSetter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public int G[||]oo { get; private set; }
}
";
            var expected = @"
class goo
{
    private int _goo;

    public int Goo
    {
        get
        {
            return _goo;
        }
        private set
        {
            _goo = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WithFieldNameAlreadyUsed()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    private int _goo;

    public int G[||]oo { get; private set; }
}
";
            var expected = @"
class goo
{
    private int _goo;
    private int _goo1;

    public int Goo
    {
        get
        {
            return _goo1;
        }
        private set
        {
            _goo1 = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WithComments()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    // Comments before
    public int G[||]oo { get; private set; } //Comments during
    //Comments after
}
";
            var expected = @"
class goo
{
    private int _goo;

    // Comments before
    public int Goo
    {
        get
        {
            return _goo;
        }
        private set
        {
            _goo = value;
        }
    } //Comments during
    //Comments after
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WithExpressionBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public int G[||]oo { get; set; }
}
";
            var expected = @"
class goo
{
    private int _goo;

    public int Goo { get => _goo; set => _goo = value; }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: PreferExpressionBodiedAccessorsWhenPossible);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WithExpressionBodyWhenOnSingleLine()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public int G[||]oo { get; set; }
}
";
            var expected = @"
class goo
{
    private int _goo;

    public int Goo { get => _goo; set => _goo = value; }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: PreferExpressionBodiedAccessorsWhenOnSingleLine);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WithExpressionBodyWhenOnSingleLine2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public int G[||]oo
    {
        get;
        set;
    }
}
";
            var expected = @"
class goo
{
    private int _goo;

    public int Goo
    {
        get => _goo;
        set => _goo = value;
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: PreferExpressionBodiedAccessorsWhenOnSingleLine);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WithExpressionBodyWithTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public int G[||]oo { get /* test */ ; set /* test2 */ ; }
}
";
            var expected = @"
class goo
{
    private int _goo;

    public int Goo { get /* test */ => _goo; set /* test2 */ => _goo = value; }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: PreferExpressionBodiedAccessorsWhenPossible);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WithPropertyOpenBraceOnSameLine()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public int G[||]oo { get; set; }
}
";
            var expected = @"
class goo
{
    private int _goo;

    public int Goo {
        get
        {
            return _goo;
        }
        set
        {
            _goo = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessorsAndPropertyOpenBraceOnSameLine);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WithAccessorOpenBraceOnSameLine()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public int G[||]oo { get; set; }
}
";
            var expected = @"
class goo
{
    private int _goo;

    public int Goo
    {
        get {
            return _goo;
        }
        set {
            _goo = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessorsAndAccessorOpenBraceOnSameLine);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StaticProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public static int G[||]oo { get; set; }
}
";
            var expected = @"
class goo
{
    private static int s_goo;

    public static int Goo
    {
        get
        {
            return s_goo;
        }
        set
        {
            s_goo = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ProtectedProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    protected int G[||]oo { get; set; }
}
";
            var expected = @"
class goo
{
    private int _goo;

    protected int Goo
    {
        get
        {
            return _goo;
        }
        set
        {
            _goo = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InternalProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    internal int G[||]oo { get; set; }
}
";
            var expected = @"
class goo
{
    private int _goo;

    internal int Goo
    {
        get
        {
            return _goo;
        }
        set
        {
            _goo = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WithAttributes()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    [A]
    public int G[||]oo { get; set; }
}
";
            var expected = @"
class goo
{
    private int _goo;

    [A]
    public int Goo
    {
        get
        {
            return _goo;
        }
        set
        {
            _goo = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CommentsInAccessors()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    /// <summary>
    /// test stuff here
    /// </summary>
    public int testg[||]oo { /* test1 */ get /* test2 */; /* test3 */ set /* test4 */; /* test5 */ } /* test6 */
}
";
            var expected = @"
class goo
{
    private int _testgoo;

    /// <summary>
    /// test stuff here
    /// </summary>
    public int testgoo
    { /* test1 */
        get /* test2 */
        {
            return _testgoo;
        } /* test3 */
        set /* test4 */
        {
            _testgoo = value;
        } /* test5 */
    } /* test6 */
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OverrideProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class MyBaseClass
{
    public virtual string Name { get; set; }
}

class MyDerivedClass : MyBaseClass
{
    public override string N[||]ame {get; set;}
}
";
            var expected = @"
class MyBaseClass
{
    public virtual string Name { get; set; }
}

class MyDerivedClass : MyBaseClass
{
    private string _name;

    public override string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SealedProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class MyClass
{
    public sealed string N[||]ame {get; set;}
}
";
            var expected = @"
class MyClass
{
    private string _name;

    public sealed string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task VirtualProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class MyBaseClass
{
    public virtual string N[||]ame { get; set; }
}

class MyDerivedClass : MyBaseClass
{
    public override string Name {get; set;}
}
";
            var expected = @"
class MyBaseClass
{
    private string _name;

    public virtual string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }
}

class MyDerivedClass : MyBaseClass
{
    public override string Name {get; set;}
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PrivateProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class MyClass
{
    private string N[||]ame { get; set; }
}
";
            var expected = @"
class MyClass
{
    private string _name;

    private string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AbstractProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class MyBaseClass
{
    public abstract string N[||]ame { get; set; }
}

class MyDerivedClass : MyBaseClass
{
    public override string Name {get; set;}
}
";
            await TestMissingAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExternProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class MyBaseClass
{
    extern string N[||]ame { get; set; }
}
";
            await TestMissingAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GetterOnly()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public int G[||]oo { get;}
}
";
            var expected = @"
class goo
{
    private readonly int _goo;

    public int Goo
    {
        get
        {
            return _goo;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GetterOnlyExpressionBodies()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public int G[||]oo { get;}
}
";
            var expected = @"
class goo
{
    private readonly int _goo;

    public int Goo => _goo;
}
";
            await TestInRegularAndScriptAsync(text, expected, options: PreferExpressionBodiesOnAccessorsAndMethods);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SetterOnly()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public int G[||]oo
    {
        set {}
    }
}
";
            await TestMissingAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExpressionBodiedAccessors()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
   private int _testgoo;

   public int testg[||]oo {get => _testgoo; set => _testgoo = value; }
}
";
            await TestMissingAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CursorAtBeginning()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    [||]public int Goo { get; set; }
}
";
            var expected = @"
class goo
{
    private int _goo;

    public int Goo
    {
        get
        {
            return _goo;
        }
        set
        {
            _goo = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CursorAtEnd()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public int Goo[||] { get; set; }
}
";
            var expected = @"
class goo
{
    private int _goo;

    public int Goo
    {
        get
        {
            return _goo;
        }
        set
        {
            _goo = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CursorOnAccessors()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public int Goo { g[||]et; set; }
}
";
            await TestMissingAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MoreThanOneGetter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public int Goo { g[||]et; get; }
}
";
            await TestMissingAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MoreThanOneSetter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public int Goo { get; s[||]et; set; }
}
";
            await TestMissingAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CustomFieldName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public int G[||]oo { get; set; }
}
";
            var expected = @"
class goo
{
    private int testingGoo;

    public int Goo
    {
        get
        {
            return testingGoo;
        }
        set
        {
            testingGoo = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: UseCustomFieldName);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NonStaticPropertyWithCustomStaticFieldName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public int G[||]oo { get; set; }
}
";
            var expected = @"
class goo
{
    private int _goo;

    public int Goo
    {
        get
        {
            return _goo;
        }
        set
        {
            _goo = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: UseCustomStaticFieldName);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StaticPropertyWithCustomStaticFieldName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class goo
{
    public static int G[||]oo { get; set; }
}
";
            var expected = @"
class goo
{
    private static int staticfieldtestGoo;

    public static int Goo
    {
        get
        {
            return staticfieldtestGoo;
        }
        set
        {
            staticfieldtestGoo = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: UseCustomStaticFieldName);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InInterface()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
interface IGoo
{
    public int Goo { get; s[||]et; }
}
";
            await TestMissingAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InStruct()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
struct goo
{
    public int G[||]oo { get; set; }
}
";
            var expected = @"
struct goo
{
    private int _goo;

    public int Goo
    {
        get
        {
            return _goo;
        }
        set
        {
            _goo = value;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected, options: DoNotPreferExpressionBodiedAccessors);
        }

        [WorkItem(22146, "https://github.com/dotnet/roslyn/issues/22146")]
        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PartialClasses()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
partial class Program
{
    int P { get; set; }
}

partial class Program
{
    int [||]Q { get; set; }
}
";
            var expected = @"
partial class Program
{
    int P { get; set; }
}

partial class Program
{
    private int _q;

    int Q { get => _q; set => _q = value; }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [WorkItem(22146, "https://github.com/dotnet/roslyn/issues/22146")]
        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PartialClassInSeparateFiles1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file1 = @"
partial class Program
{
    int [||]P { get; set; }
}";
            var file2 = @"
partial class Program
{
    int Q { get; set; }
}";
            var file1AfterRefactor = @"
partial class Program
{
    private int _p;

    int P { get => _p; set => _p = value; }
}";

            var xmlString = string.Format(@"
<Workspace>
    <Project Language=""{0}"" CommonReferences=""true"">
        <Document FilePath=""file1"">{1}</Document>
        <Document FilePath=""file2"">{2}</Document>
    </Project>
</Workspace>", LanguageNames.CSharp, file1, file2);

            using (var testWorkspace = TestWorkspace.Create(xmlString))
            {
                // refactor file1 and check
                var (_, action) = await GetCodeActionsAsync(testWorkspace, parameters: default);
                await TestActionAsync(
                    testWorkspace,
                    file1AfterRefactor,
                    action,
                    conflictSpans: ImmutableArray<TextSpan>.Empty,
                    renameSpans: ImmutableArray<TextSpan>.Empty,
                    warningSpans: ImmutableArray<TextSpan>.Empty,
                    navigationSpans: ImmutableArray<TextSpan>.Empty,
                    parameters: default);
            }
        }

        [WorkItem(22146, "https://github.com/dotnet/roslyn/issues/22146")]
        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PartialClassInSeparateFiles2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var file1 = @"
partial class Program
{
    int P { get; set; }
}";
            var file2 = @"
partial class Program
{
    int Q[||] { get; set; }
}";
            var file2AfterRefactor = @"
partial class Program
{
    private int _q;

    int Q { get => _q; set => _q = value; }
}";

            var xmlString = string.Format(@"
<Workspace>
    <Project Language=""{0}"" CommonReferences=""true"">
        <Document FilePath=""file1"">{1}</Document>
        <Document FilePath=""file2"">{2}</Document>
    </Project>
</Workspace>", LanguageNames.CSharp, file1, file2);

            using (var testWorkspace = TestWorkspace.Create(xmlString))
            {
                // refactor file2 and check
                var (_, action) = await GetCodeActionsAsync(testWorkspace, parameters: default);
                await TestActionAsync(
                    testWorkspace,
                    file2AfterRefactor,
                    action,
                    conflictSpans: ImmutableArray<TextSpan>.Empty,
                    renameSpans: ImmutableArray<TextSpan>.Empty,
                    warningSpans: ImmutableArray<TextSpan>.Empty,
                    navigationSpans: ImmutableArray<TextSpan>.Empty,
                    parameters: default);
            }
        }

        [Fact, Trait(Traits.Feature, Traits.Features.ConvertAutoPropertyToFullProperty)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task InvalidLocation()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(@"namespace NS
{
    public int G[||]oo { get; set; }
}");

            await TestMissingAsync("public int G[||]oo { get; set; }");
        }
    }
}
