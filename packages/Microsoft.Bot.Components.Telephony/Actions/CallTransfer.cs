﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Components.Telephony.Actions
{
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using AdaptiveExpressions.Properties;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using Newtonsoft.Json;

    /// <summary>
    /// Transfers call to given phone number.
    /// </summary>
    public class CallTransfer : Dialog
    {
        /// <summary>
        /// Class identifier.
        /// </summary>
        [JsonProperty("$kind")]
        public const string Kind = "Microsoft.Telephony.CallTransfer";

        /// <summary>
        /// Initializes a new instance of the <see cref="CallTransfer"/> class.
        /// </summary>
        /// <param name="sourceFilePath">Optional, source file full path.</param>
        /// <param name="sourceLineNumber">Optional, line number in source file.</param>
        [JsonConstructor]
        public CallTransfer([CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
            : base()
        {
            // enable instances of this command as debug break point
            this.RegisterSourceLocation(sourceFilePath, sourceLineNumber);
        }

        /// <summary>
        /// Gets or sets the phone number to be included when sending the handoff activity.
        /// </summary>
        /// <value>
        /// <see cref="StringExpression"/>.
        /// </value>
        [JsonProperty("phoneNumber")]
        public StringExpression PhoneNumber { get; set; }

        /// <summary>
        /// Called when the dialog is started and pushed onto the dialog stack.
        /// </summary>
        /// <param name="dc">The <see cref="DialogContext"/> for the current turn of conversation.</param>
        /// <param name="options">Optional, initial information to pass to the dialog.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async override Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var phoneNumber = this.PhoneNumber?.GetValue(dc.State);

            // Create handoff event, passing the phone number to transfer to as context.
            var context = new { TargetPhoneNumber = phoneNumber };
            var handoffEvent = EventFactory.CreateHandoffInitiation(dc.Context, context);
            await dc.Context.SendActivityAsync(handoffEvent, cancellationToken).ConfigureAwait(false);
            return await dc.EndDialogAsync(result: null, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}