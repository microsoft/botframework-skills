# Webapp scenarios
## Scenario 1: I want a local web app bot and I want to connect to a local web app  skill via manual connection to the skill

| Date | Test Result| Issues|
|:-----|:-----------|:------|
|4/26 | Fail| [1](https://github.com/microsoft/BotFramework-Composer/issues/7383) 
|4/27| Pass*| [1](https://github.com/microsoft/BotFramework-Composer/issues/7422)


## Scenario 2: I want a web app  local bot and I want to connect to a local web app  skill via intent recognition wiring


| Date | Test Result| Issues|
|:-----|:-----------|:------|
|4/26 | Pass |  |
|4/27| Pass||


## Scenario 3: I want a local web app bot and I want to connect to a remote web app skill via intent recognition
- Will need ngrok for skill host endpoint


| Date | Test Result| Issues|
|:-----|:-----------|:------|
|4/26| Pass||
|4/27| Pass||


## Scenario 5: I want a deployed web app  bot and I want to connect to a remote web app  skill via intent recognition

| Date | Test Result| Issues|
|:-----|:-----------|:------|
|4/26|Pass||
|4/27| Pass||

## Scenario 6: I want a deployed web app  bot and I want to connect to a remote skill chaining with web app via intent recognition 


| Date | Test Result| Issues|
|:-----|:-----------|:------|
|4/27| Pass||

# Function Scenarios
## Scenario 1: I want a function app bot and I want to connect to a local function app  skill via manual connection to the skill


| Date | Test Result| Issues|
|:-----|:-----------|:------|
|4/27| **Blocked** | [1](https://github.com/microsoft/BotFramework-Composer/issues/7343) | 


## Scenario 2: I want a function local bot and I want to connect to a local function skill via intent recognition wiring


| Date | Test Result| Issues|
|:-----|:-----------|:------|
|4/27| **Blocked** | [1](https://github.com/microsoft/BotFramework-Composer/issues/7343) | 


## Scenario 3: I want a function app bot and I want to connect to a remote function skill via intent recognition


| Date | Test Result| Issues|
|:-----|:-----------|:------|
|4/27| **Blocked** | [1](https://github.com/microsoft/BotFramework-Composer/issues/7343) | 

## Scenario 5: I want a deployed function  bot and I want to connect to a remote function  skill via intent recognition

| Date | Test Result| Issues|
|:-----|:-----------|:------|
|4/27| **Blocked** | [1](https://github.com/microsoft/BotFramework-Composer/issues/7343) | 

## Scenario 6: I want a deployed function bot and I want to connect to a remote skill chaining with function via intent recognition 
| Date | Test Result| Issues|
|:-----|:-----------|:------|
|4/27| **Blocked** | [1](https://github.com/microsoft/BotFramework-Composer/issues/7343) | 

# Web app & Function crossover

## web app parent, function skill
| Date | Test Result| Issues|
|:-----|:-----------|:------|
|4/27| **Blocked** | [1](https://github.com/microsoft/BotFramework-Composer/issues/7343) | 

## function parent, web app skill
| Date | Test Result| Issues|
|:-----|:-----------|:------|
|4/27| **Blocked** | [1](https://github.com/microsoft/BotFramework-Composer/issues/7343) | 

## web app parent, function skill, web app skill 
| Date | Test Result| Issues|
|:-----|:-----------|:------|
|4/27| **Blocked** | [1](https://github.com/microsoft/BotFramework-Composer/issues/7343) | 

## web app parent, web app skill, function skill
| Date | Test Result| Issues|
|:-----|:-----------|:------|
|4/27| **Blocked** | [1](https://github.com/microsoft/BotFramework-Composer/issues/7343) | 

## function parent, function skill, web app skill
| Date | Test Result| Issues|
|:-----|:-----------|:------|
|4/27| **Blocked** | [1](https://github.com/microsoft/BotFramework-Composer/issues/7343) | 

## function parent, web app skill, function skill
| Date | Test Result| Issues|
|:-----|:-----------|:------|
|4/27| **Blocked** | [1](https://github.com/microsoft/BotFramework-Composer/issues/7343) | 