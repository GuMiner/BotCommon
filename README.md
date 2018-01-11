BotCommon
=========
Provides common functionality to simplify [Microsoft Bot Framework](https://dev.botframework.com/) application development.

Functionality
-------------
* Sanitizes and simplifies Bot Framework data types for ease-of-consumption.
* Provides an **IActivityProcessor** interface and a generic Bot Framework API controller using that interface.
* Defines an abstract **ConversationActivityProcessor** for standard conversational workflows.
* Simplifies access to Azure Blob storage for long-term data storage and [SAS](https://docs.microsoft.com/en-us/azure/storage/common/storage-dotnet-shared-access-signature-part-1) delegated access.
* Standardizes [Newtonsoft.JSON](https://www.newtonsoft.com/json) settings for consistent formatting.

The goal of this functionality is to easily write simple conversational bots, without the overhead of a powerful multipurpose library.
