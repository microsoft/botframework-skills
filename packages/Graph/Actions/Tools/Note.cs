demo note action// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Components.Graph.Actions
{
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Newtonsoft.Json;

    /// <summary>
    /// This action accepts a collection of MSGraph event options and filters them based.
    /// </summary>
    [GraphCustomActionRegistration(Note.DeclarativeType)]
    public class Note : Dialog
    {
        /// <summary>
        /// The declarative type name for this action.
        /// </summary>
        [JsonProperty("$kind")]
        public const string DeclarativeType = "Microsoft.Bot.Components.Actions.Note";

        /// <summary>
        /// Initializes a new instance of the <see cref="Note"/> class.
        /// </summary>
        /// <param name="callerPath">The path of the caller.</param>
        /// <param name="callerLine">The line number at which the method is called.</param>
        [JsonConstructor]
        public Note([CallerFilePath] string callerPath = "", [CallerLineNumber] int callerLine = 0)
            : base()
        {
            this.RegisterSourceLocation(callerPath, callerLine);
        }

        public async override Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            return await dc.EndDialogAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}
