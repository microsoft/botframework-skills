﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace ITSMSkill.Dialogs.Teams.TicketTaskModule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AdaptiveCards;
    using ITSMSkill.Dialogs.Teams.CreateTicketTaskModuleView;
    using ITSMSkill.Extensions;
    using ITSMSkill.Extensions.Teams;
    using ITSMSkill.Extensions.Teams.TaskModule;
    using ITSMSkill.Models;
    using ITSMSkill.Models.UpdateActivity;
    using ITSMSkill.Services;
    using ITSMSkill.TeamsChannels;
    using ITSMSkill.TeamsChannels.Invoke;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Connector;
    using Microsoft.Bot.Schema;
    using Microsoft.Bot.Schema.Teams;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// UpdateTicket Handler for Updating Ticket OnFetch and OnSumbit Activity for TaskModules
    /// </summary>
    [TeamsInvoke(FlowType = nameof(TeamsFlowType.UpdateTicket_Form))]
    public class UpdateTicketTeamsImplementation : ITeamsTaskModuleHandler<TaskModuleResponse>
    {
        private readonly IStatePropertyAccessor<SkillState> _stateAccessor;
        private readonly ConversationState _conversationState;
        private readonly BotSettings _settings;
        private readonly BotServices _services;
        private readonly IServiceManager _serviceManager;
        private readonly IStatePropertyAccessor<ActivityReferenceMap> _activityReferenceMapAccessor;
        private readonly IConnectorClient _connectorClient;
        private readonly ITeamsActivity<Ticket> _teamsTicketUpdateActivity;

        public UpdateTicketTeamsImplementation(IServiceProvider serviceProvider)
        {
            _conversationState = serviceProvider.GetService<ConversationState>();
            _settings = serviceProvider.GetService<BotSettings>();
            _services = serviceProvider.GetService<BotServices>();
            _stateAccessor = _conversationState.CreateProperty<SkillState>(nameof(SkillState));
            _serviceManager = serviceProvider.GetService<IServiceManager>();
            _activityReferenceMapAccessor = _conversationState.CreateProperty<ActivityReferenceMap>(nameof(ActivityReferenceMap));
            _connectorClient = serviceProvider.GetService<IConnectorClient>();
            _teamsTicketUpdateActivity = serviceProvider.GetService<ITeamsActivity<Ticket>>();
        }

        public async Task<TaskModuleResponse> OnTeamsTaskModuleFetchAsync(ITurnContext context, CancellationToken cancellationToken)
        {
            var taskModuleMetadata = context.Activity.GetTaskModuleMetadata<TaskModuleMetadata>();

            var ticketDetails = taskModuleMetadata.FlowData != null ?
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(taskModuleMetadata.FlowData))
                    .TryGetValue("IncidentDetails", out var ticket) ? (Ticket)ticket : null
                    : null;

            return new TaskModuleResponse()
            {
                Task = new TaskModuleContinueResponse()
                {
                    Value = new TaskModuleTaskInfo()
                    {
                        Title = "Please Update The Card Below",
                        Height = "medium",
                        Width = 500,
                        Card = new Attachment
                        {
                            ContentType = AdaptiveCard.ContentType,
                            Content = TicketDialogHelper.UpdateIncidentCard(ticketDetails)
                        }
                    }
                }
            };
        }

        public async Task<TaskModuleResponse> OnTeamsTaskModuleSubmitAsync(ITurnContext context, CancellationToken cancellationToken)
        {
            var state = await _stateAccessor.GetAsync(context, () => new SkillState());

            var taskModuleMetadata = context.Activity.GetTaskModuleMetadata<TaskModuleMetadata>();

            var ticketDetails = taskModuleMetadata.FlowData != null ?
                JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(taskModuleMetadata.FlowData))
                .TryGetValue("IncidentDetails", out var ticket) ? (Ticket)ticket : null
                : null;

            // If ticket is valid go ahead and update
            if (ticketDetails != null)
            {
                ActivityReferenceMap activityReferenceMap = await _activityReferenceMapAccessor.GetAsync(
                    context,
                    () => new ActivityReferenceMap(),
                    cancellationToken)
                .ConfigureAwait(false);

                // Get Activity Id from ActivityReferenceMap
                activityReferenceMap.TryGetValue(context.Activity.Conversation.Id, out var activityReference);

                // Get User Input from AdatptiveCard
                var activityValueObject = JObject.FromObject(context.Activity.Value);

                var isDataObject = activityValueObject.TryGetValue("data", StringComparison.InvariantCultureIgnoreCase, out JToken dataValue);
                JObject dataObject = null;
                if (isDataObject)
                {
                    dataObject = dataValue as JObject;

                    // Get Title
                    var title = dataObject.GetValue("IncidentTitle");

                    // Get Description
                    var description = dataObject.GetValue("IncidentDescription");

                    // Get Urgency
                    var urgency = dataObject.GetValue("IncidentUrgency");

                    // Create Managemenet object
                    var management = _serviceManager.CreateManagement(_settings, state.AccessTokenResponse, state.ServiceCache);
                    var result = await management.UpdateTicket(ticketDetails.Id, title.Value<string>(), description.Value<string>(), (UrgencyLevel)Enum.Parse(typeof(UrgencyLevel), urgency.Value<string>()));

                    if (result.Success)
                    {
                        await _teamsTicketUpdateActivity.UpdateTaskModuleActivityAsync(
                            context,
                            activityReference,
                            result.Tickets.FirstOrDefault(),
                            cancellationToken);

                        // Return Added Incident Envelope
                        return new TaskModuleResponse()
                        {
                            Task = new TaskModuleContinueResponse()
                            {
                                Type = "continue",
                                Value = new TaskModuleTaskInfo()
                                {
                                    Title = "Incident Updated",
                                    Height = "small",
                                    Width = 300,
                                    Card = new Attachment
                                    {
                                        ContentType = AdaptiveCard.ContentType,
                                        Content = RenderCreateIncidentHelper.ImpactTrackerResponseCard("Incident has been Updated")
                                    }
                                }
                            }
                        };
                    }
                }
            }

            // Failed to update incident
            return new TaskModuleResponse
            {
                Task = new TaskModuleContinueResponse()
                {
                    Type = "continue",
                    Value = new TaskModuleTaskInfo()
                    {
                        Title = "Incident Update Failed",
                        Height = "medium",
                        Width = 500,
                        Card = new Attachment
                        {
                            ContentType = AdaptiveCard.ContentType,
                            Content = RenderCreateIncidentHelper.ImpactTrackerResponseCard("Incident Update Failed")
                        }
                    }
                }
            };
        }
    }
}
