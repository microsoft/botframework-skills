﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneSkill.Responses.Main;
using PhoneSkill.Responses.OutgoingCall;
using PhoneSkill.Tests.Flow.Utterances;

namespace PhoneSkill.Tests.Flow
{
    [TestClass]
    [TestCategory("UnitTests")]
    public class InterruptionTests : PhoneSkillTestBase
    {
        [TestMethod]
        public async Task Test_Help_Interruption()
        {
            await GetTestFlow()
                .SendConversationUpdate()
                .AssertReply(Message(PhoneMainResponses.WelcomeMessage))
               .Send(OutgoingCallUtterances.OutgoingCallNoEntities)
               .AssertReply(Message(OutgoingCallResponses.RecipientPrompt))
               .Send(GeneralUtterances.Help)
               .AssertReply(Message(PhoneMainResponses.HelpMessage))
               .AssertReply(Message(OutgoingCallResponses.RecipientPrompt))
               .StartTestAsync();
        }

        [TestMethod]
        public async Task Test_Cancel_Interruption()
        {
            await GetTestFlow()
               .SendConversationUpdate()
               .AssertReply(Message(PhoneMainResponses.WelcomeMessage))
               .Send(OutgoingCallUtterances.OutgoingCallNoEntities)
               .AssertReply(Message(OutgoingCallResponses.RecipientPrompt))
               .Send(GeneralUtterances.Cancel)
               .AssertReply(Message(PhoneMainResponses.CancelMessage))
               .StartTestAsync();
        }

        [TestMethod]
        public async Task Test_Action_Help_Interruption()
        {
            var activity = new Activity()
            {
                Type = ActivityTypes.Event,
                Name = "OutgoingCall",
            };

            await GetSkillTestFlow()
               .Send(activity)
               .AssertReply(Message(OutgoingCallResponses.RecipientPrompt))
               .Send(GeneralUtterances.Help)
               .AssertReply(Message(PhoneMainResponses.HelpMessage))
               .AssertReply(Message(OutgoingCallResponses.RecipientPrompt))
               .StartTestAsync();
        }

        [TestMethod]
        public async Task Test_Action_Cancel_Interruption()
        {
            var activity = new Activity()
            {
                Type = ActivityTypes.Event,
                Name = "OutgoingCall",
            };

            await GetSkillTestFlow()
               .Send(activity)
               .AssertReply(Message(OutgoingCallResponses.RecipientPrompt))
               .Send(GeneralUtterances.Cancel)
               .AssertReply(Message(PhoneMainResponses.CancelMessage))
               .StartTestAsync();
        }
    }
}
