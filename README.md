# Azure Service Bus Rest API Sample
A .net sample to send and receive messages from Azure Service Bus over HTTPS/443.
This works in scenarios where outbound port 5671 cannot be opened due to various constraints. 
This sample uses polling using timer to receive messages from Azure Service Bus subscription. 
If there are many subscribers, the subscribers needs to be created using Azure ARM Rest API using Azure authentication. 



