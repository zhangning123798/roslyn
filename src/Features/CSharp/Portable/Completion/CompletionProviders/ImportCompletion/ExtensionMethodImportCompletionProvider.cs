﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.Completion.Providers;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Shared.Extensions.ContextQuery;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.CSharp.Completion.Providers
{
    [ExportCompletionProvider(nameof(ExtensionMethodImportCompletionProvider), LanguageNames.CSharp)]
    [ExtensionOrder(After = nameof(TypeImportCompletionProvider))]
    [Shared]
    internal sealed class ExtensionMethodImportCompletionProvider : AbstractExtensionMethodImportCompletionProvider
    {
        [ImportingConstructor]
        [Obsolete(MefConstruction.ImportingConstructorMessage, error: true)]
        public ExtensionMethodImportCompletionProvider()
        {
        }

        protected override string GenericSuffix => "<>";

        internal override bool IsInsertionTrigger(SourceText text, int characterPosition, OptionSet options)
            => CompletionUtilities.IsTriggerCharacter(text, characterPosition, options);

        internal override ImmutableHashSet<char> TriggerCharacters { get; } = CompletionUtilities.CommonTriggerCharacters;

        protected override ImmutableArray<string> GetImportedNamespaces(
            SyntaxNode location,
            SemanticModel semanticModel,
            CancellationToken cancellationToken)
            => ImportCompletionProviderHelper.GetImportedNamespaces(location, semanticModel);

        protected override Task<SyntaxContext> CreateContextAsync(Document document, int position, CancellationToken cancellationToken)
            => ImportCompletionProviderHelper.CreateContextAsync(document, position, cancellationToken);
    }
}
