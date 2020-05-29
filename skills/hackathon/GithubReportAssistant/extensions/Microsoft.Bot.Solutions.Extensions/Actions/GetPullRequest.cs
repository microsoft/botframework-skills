﻿using AdaptiveExpressions.Properties;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Solutions.Extensions.Common;
using Microsoft.Bot.Solutions.Extensions.Models;
using Microsoft.Graph;
using Newtonsoft.Json;
using Octokit;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Bot.Solutions.Extensions.Actions
{
    class GetPullRequest : Dialog
    {
        [JsonProperty("$kind")]
        public const string DeclarativeType = "Github.GetPullRequest";

        [JsonConstructor]
        public GetPullRequest([CallerFilePath] string callerPath = "", [CallerLineNumber] int callerLine = 0)
            : base()
        {
            this.RegisterSourceLocation(callerPath, callerLine);
        }

        [JsonProperty("owner")]
        public StringExpression Owner { get; set; }

        [JsonProperty("name")]
        public StringExpression Name { get; set; }

        [JsonProperty("status")]
        public StringExpression Status { get; set; }

        [JsonProperty("resultProperty")]
        public string resultProperty { get; set; }


        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            var dcState = dc.State;
            var owner = Owner.GetValue(dcState);
            var name = Name.GetValue(dcState);
            var status = Status.GetValue(dcState);
            var github = new GitHubClient(new ProductHeaderValue("TestClient"));

            IReadOnlyList<PullRequest> pullRequests = new List<PullRequest>();
            var resultPullRequest = new List<GitHubPullRequest>();
            try
            {
                var prRequest = new PullRequestRequest
                {
                    State = GetStatus(status),
                };

                pullRequests = await github.PullRequest.GetAllForRepository(owner, name);

                foreach(var pr in pullRequests)
                {
                    resultPullRequest.Add(new GitHubPullRequest()
                    {
                        Title = pr.Title,
                        CreatedAt = pr.CreatedAt,
                        UpdatedAt = pr.UpdatedAt,
                        ClosedAt = pr.ClosedAt,
                        Status = pr.State.StringValue,
                        Url = pr.Url
                    });
                }
            }
            catch (ServiceException)
            {
                return null;
            }

            if (this.resultProperty != null)
            {
                dcState.SetValue(resultProperty, resultPullRequest);
            }

            // return the actionResult as the result of this operation
            return await dc.EndDialogAsync(result: resultPullRequest, cancellationToken: cancellationToken);
        }

        private ItemStateFilter GetStatus(string status)
        {
            var stateFilter = ItemStateFilter.All;
            if (status.ToLower().Equals("open"))
            {
                stateFilter = ItemStateFilter.Open;
            }
            else if (status.ToLower().Equals("closed"))
            {
                stateFilter = ItemStateFilter.Closed;
            }

            return stateFilter;
        }
    }
}
