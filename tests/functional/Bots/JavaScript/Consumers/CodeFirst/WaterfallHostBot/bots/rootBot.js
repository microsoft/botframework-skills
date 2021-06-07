// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

const { ActivityHandler, ActivityTypes, CardFactory, MessageFactory } = require('botbuilder');
const { runDialog } = require('botbuilder-dialogs');
const WelcomeCard = require('../cards/welcomeCard.json');

class RootBot extends ActivityHandler {
  /**
   * @param {import('botbuilder').ConversationState} conversationState
   * @param {import('botbuilder').SkillHttpClient} skillClient
   * @param {import('botbuilder-dialogs').Dialog} mainDialog
   */
  constructor (conversationState, skillClient, mainDialog) {
    super();
    if (!conversationState) throw new Error('[RootBot]: Missing parameter. conversationState is required');
    if (!mainDialog) throw new Error('[RootBot]: Missing parameter. mainDialog is required');

    this.conversationState = conversationState;
    this.skillClient = skillClient;
    this.mainDialog = mainDialog;
    this.botId = process.env.MicrosoftAppId;

    // Create state property to track the active skill
    this.activeSkillProperty = this.conversationState.createProperty(RootBot.ActiveSkillPropertyName);

    this.onTurn(async (turnContext, next) => {
      if (turnContext.activity.type !== ActivityTypes.ConversationUpdate) {
        // Run the Dialog with the activity.
        await runDialog(this.mainDialog, turnContext, this.conversationState.createProperty('DialogState'));
      }

      await next();
    });

    this.onMembersAdded(async (context, next) => {
      const membersAdded = context.activity.membersAdded;
      for (const member of membersAdded) {
        if (member.id !== context.activity.recipient.id) {
          const welcomeCard = CardFactory.adaptiveCard(WelcomeCard);
          const activity = MessageFactory.attachment(welcomeCard);
          activity.speak = 'Welcome to the waterfall host bot';
          await context.sendActivity(activity);
          await runDialog(this.mainDialog, context, conversationState.createProperty('DialogState'));
        }
      }

      // By calling next() you ensure that the next BotHandler is run.
      await next();
    });
  }

  /**
   * Override the ActivityHandler.run() method to save state changes after the bot logic completes.
   * @param {import('botbuilder').TurnContext} turnContext
   */
  async run (context) {
    await super.run(context);

    // Save any state changes. The load happened during the execution of the Dialog.
    await this.conversationState.saveChanges(context, false);
  }

  /**
   * @param {import('botbuilder').TurnContext} turnContext
   * @param {*} targetSkill
   */
  async sendToSkill (context, targetSkill) {
    // NOTE: Always SaveChanges() before calling a skill so that any activity generated by the skill
    // will have access to current accurate state.
    await this.conversationState.saveChanges(context, true);

    // route the activity to the skill
    const response = await this.skillClient.postToSkill(this.botId, targetSkill, this.skillsConfig.skillHostEndpoint, context.activity);

    // Check response status
    if (!(response.status >= 200 && response.status <= 299)) {
      throw new Error(`[RootBot]: Error invoking the skill id: "${targetSkill.id}" at "${targetSkill.skillEndpoint}" (status is ${response.status}). \r\n ${response.body}`);
    }
  }
}

module.exports.RootBot = RootBot;
