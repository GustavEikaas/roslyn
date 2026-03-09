// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeStyle;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp.CodeStyle;
using Microsoft.CodeAnalysis.CSharp.Completion.Providers;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Completion.CompletionProviders
{
    public class PartialMethodCompletionProviderTests : AbstractCSharpCompletionProviderTests
    {
        public PartialMethodCompletionProviderTests(CSharpTestWorkspaceFixture workspaceFixture) : base(workspaceFixture)
        {
        }

        internal override CompletionProvider CreateCompletionProvider()
        {
            return new PartialMethodCompletionProvider();
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoPartialMethods1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"class c
{
    $$
}";
            await VerifyNoItemsExistAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoPartialMethods2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"class c
{
    private void goo() { };

    partial void $$
}";
            await VerifyNoItemsExistAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PartialMethodInPartialClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"partial class c
{
    partial void goo();

    partial void $$
}";
            await VerifyItemExistsAsync(text, "goo()");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PartialMethodInPartialGenericClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"partial class c<T>
{
    partial void goo(T bar);

    partial void $$
}";
            await VerifyItemExistsAsync(text, "goo(T bar)");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PartialMethodInPartialStruct()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"partial struct c
{
    partial void goo();

    partial void $$
}";
            await VerifyItemExistsAsync(text, "goo()");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CompletionOnPartial1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"partial class c
{
    partial void goo();

    partial $$
}";
            await VerifyItemExistsAsync(text, "goo()");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CompletionOnPartial2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"partial class c
{
    partial void goo();

    void partial $$
}";
            await VerifyItemExistsAsync(text, "goo()");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StaticUnsafePartial()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"partial class c
{
    partial static unsafe void goo();

    void static unsafe partial $$
}";
            await VerifyItemExistsAsync(text, "goo()");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PartialCompletionWithPrivate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"partial class c
{
    partial static unsafe void goo();

    private partial $$
}";
            await VerifyItemExistsAsync(text, "goo()");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotCompletionDespiteValidModifier()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"partial class c
{
    partial void goo();

    void partial unsafe $$
}";
            await VerifyNoItemsExistAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotIfPublic()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"partial class c
{
    public partial void goo();

    void partial $$
}";
            await VerifyNoItemsExistAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotIfInternal()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"partial class c
{
    internal partial void goo();

    void partial $$
}";
            await VerifyNoItemsExistAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotIfProtected()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"partial class c
{
    protected partial void goo();

    void partial $$
}";
            await VerifyNoItemsExistAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotIfProtectedInternal()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"partial class c
{
    protected internal partial void goo();

    void partial $$
}";
            await VerifyNoItemsExistAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotIfExtern()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"partial class c
{
    partial void goo();

    extern void partial $$
}";
            await VerifyNoItemsExistAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotIfVirtual()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"partial class c
{
    virtual partial void goo();

    void partial $$
}";
            await VerifyNoItemsExistAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotIfNonVoidReturnType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"partial class c
{
    partial int goo();

    partial $$
}";
            await VerifyNoItemsExistAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInsideInterface()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"partial interface i
{
    partial void goo();

    partial $$
}";
            await VerifyNoItemsExistAsync(text);
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CommitInPartialClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markupBeforeCommit = @"partial class c
{
    partial void goo();

    partial $$
}";

            var expectedCodeAfterCommit = @"partial class c
{
    partial void goo();

    partial void goo()
    {
        throw new System.NotImplementedException();$$
    }
}";

            await VerifyCustomCommitProviderAsync(markupBeforeCommit, "goo()", expectedCodeAfterCommit);
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CommitGenericPartialMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markupBeforeCommit = @"partial class c<T>
{
    partial void goo(T bar);

    partial $$
}";

            var expectedCodeAfterCommit = @"partial class c<T>
{
    partial void goo(T bar);

    partial void goo(T bar)
    {
        throw new System.NotImplementedException();$$
    }
}";

            await VerifyCustomCommitProviderAsync(markupBeforeCommit, "goo(T bar)", expectedCodeAfterCommit);
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CommitMethodErasesPrivate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markupBeforeCommit = @"partial class c
{
    partial void goo();

    private partial $$
}";

            var expectedCodeAfterCommit = @"partial class c
{
    partial void goo();

    partial void goo()
    {
        throw new System.NotImplementedException();$$
    }
}";

            await VerifyCustomCommitProviderAsync(markupBeforeCommit, "goo()", expectedCodeAfterCommit);
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CommitInPartialClassPart()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markupBeforeCommit = @"partial class c
{
    partial void goo();
}

partial class c
{
    partial $$
}";

            var expectedCodeAfterCommit = @"partial class c
{
    partial void goo();
}

partial class c
{
    partial void goo()
    {
        throw new System.NotImplementedException();$$
    }
}";

            await VerifyCustomCommitProviderAsync(markupBeforeCommit, "goo()", expectedCodeAfterCommit);
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CommitInPartialStruct()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markupBeforeCommit = @"partial struct c
{
    partial void goo();

    partial $$
}";

            var expectedCodeAfterCommit = @"partial struct c
{
    partial void goo();

    partial void goo()
    {
        throw new System.NotImplementedException();$$
    }
}";

            await VerifyCustomCommitProviderAsync(markupBeforeCommit, "goo()", expectedCodeAfterCommit);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotIfNoPartialKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"partial class C
    {
        partial void Goo();
    }
 
    partial class C
    {
        void $$
    }
";
            await VerifyNoItemsExistAsync(text);
        }

        [WorkItem(578757, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/578757")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DoNotConsiderFollowingDeclarationPartial()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"class Program
{
    partial $$
 
    void Goo()
    {
        
    }
}
";
            await VerifyNoItemsExistAsync(text);
        }

        [WorkItem(578078, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/578078")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.Completion)]
        public async Task CommitAsync()
        {
            var markupBeforeCommit = @"using System;

partial class Bar
{
    partial void Goo();

    async partial $$
}";

            var expectedCodeAfterCommit = @"using System;

partial class Bar
{
    partial void Goo();

    async partial void Goo()
    {
        throw new NotImplementedException();$$
    }
}";

            await VerifyCustomCommitProviderAsync(markupBeforeCommit, "Goo()", expectedCodeAfterCommit);
        }

        [WorkItem(578078, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/578078")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AmbiguityCommittingWithParen()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markupBeforeCommit = @"using System;

partial class Bar
{
    partial void Goo();

    partial Goo$$
}";

            var expectedCodeAfterCommit = @"using System;

partial class Bar
{
    partial void Goo();

    partial void Goo()
    {
        throw new NotImplementedException();$$
    }
}";

            await VerifyCustomCommitProviderAsync(markupBeforeCommit, "Goo()", expectedCodeAfterCommit, commitChar: '(');
        }

        [WorkItem(965677, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/965677")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoDefaultParameterValues()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"namespace PartialClass
{
    partial class PClass
    {
        partial void PMethod(int i = 0);

        partial $$
    }
}
";

            var expected = @"namespace PartialClass
{
    partial class PClass
    {
        partial void PMethod(int i = 0);

        partial void PMethod(int i)
        {
            throw new System.NotImplementedException();$$
        }
    }
}
";
            await VerifyCustomCommitProviderAsync(text, "PMethod(int i)", expected);
        }

        [WorkItem(26388, "https://github.com/dotnet/roslyn/issues/26388")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExpressionBodyMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var workspace = WorkspaceFixture.GetWorkspace();
            var originalOptions = workspace.Options;

            try
            {
                workspace.Options = originalOptions.WithChangedOption(
                    CSharpCodeStyleOptions.PreferExpressionBodiedMethods,
                    new CodeStyleOption<ExpressionBodyPreference>(ExpressionBodyPreference.WhenPossible, NotificationOption.Silent));

                var text = @"using System;
partial class Bar
{
    partial void Foo();
    partial $$
}
"
;

                var expected = @"using System;
partial class Bar
{
    partial void Foo();
    partial void Foo() => throw new NotImplementedException();$$
}
"
;

                await VerifyCustomCommitProviderAsync(text, "Foo()", expected);
            }
            finally
            {
                workspace.Options = originalOptions;
            }
        }
    }
}
