// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.LanguageServices;
using Microsoft.CodeAnalysis.Shared.Extensions;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.TypeInferrer
{
    public partial class TypeInferrerTests
    {
        private async Task TestDelegateAsync(string text, string expectedType)
        {
            MarkupTestFile.GetSpan(text, out text, out var textSpan);

            Document document = fixture.UpdateDocument(text, SourceCodeKind.Regular);

            var root = await document.GetSyntaxRootAsync();
            var node = FindExpressionSyntaxFromSpan(root, textSpan);

            var typeInference = document.GetLanguageService<ITypeInferenceService>();
            var delegateType = typeInference.InferDelegateType(await document.GetSemanticModelAsync(), node, CancellationToken.None);

            Assert.NotNull(delegateType);
            Assert.Equal(expectedType, delegateType.ToNameDisplayString());
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDeclaration1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System;
class C
{
  void M()
  {
    Func<int> q = [|here|];
  }
}";

            await TestDelegateAsync(text, "System.Func<int>");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAssignment1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System;
class C
{
  void M()
  {
    Func<int> f;
    f = [|here|]
  }
}";

            await TestDelegateAsync(text, "System.Func<int>");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArgument1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System;
class C
{
  void M()
  {
    Bar([|here|]);
  }

  void Bar(Func<int> f);
}";

            await TestDelegateAsync(text, "System.Func<int>");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstructor1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System;
class C
{
  void M()
  {
    new C([|here|]);
  }

  public C(Func<int> f);
}";

            await TestDelegateAsync(text, "System.Func<int>");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDelegateConstructor1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System;
class C
{
  void M()
  {
    new Func<int>([|here|]);
  }
}";

            await TestDelegateAsync(text, "System.Func<int>");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCastExpression1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System;
class C
{
  void M()
  {
    (Func<int>)[|here|]
  }
}";

            await TestDelegateAsync(text, "System.Func<int>");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCastExpression2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System;
class C
{
  void M()
  {
    (Func<int>)([|here|]
  }
}";

            await TestDelegateAsync(text, "System.Func<int>");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestReturnFromMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System;
class C
{
  Func<int> M()
  {
    return [|here|]
  }
}";

            await TestDelegateAsync(text, "System.Func<int>");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideLambda1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System;
class C
{
  void M()
  {
    Func<int,Func<string,bool>> f = i => [|here|]
  }
}";

            await TestDelegateAsync(text, "System.Func<string, bool>");
        }
    }
}
