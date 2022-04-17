# NHN Toast Notification Service Custom Connector #

This is to provide [Microsoft Power Platform](http://powerplatform.microsoft.com/?WT.mc_id=dotnet-58531-juyoo) with a custom connector for NHN Toast Notification Services including [SMS/MMS](https://www.toast.com/kr/service/notification/sms) and [KakaoTalk messages](https://www.toast.com/kr/service/notification/kakaotalk-bizmessage).


## High-Level Architecture ##

The elements in the green rectangle is the scope of this project.

[![high level architecture and scope of this project](./assets/architecture.png)](https://raw.githubusercontent.com/aliencube/nhn-toast-notification-service-custom-connector/main/assets/architecture.png)

* **[Azure Functions](https://azure.microsoft.com/services/functions/?WT.mc_id=dotnet-58531-juyoo) Proxy**: It works as a facade to NHN Notification Services ([SMS/MMS](https://www.toast.com/kr/service/notification/sms) and [KakaoTalk messages](https://www.toast.com/kr/service/notification/kakaotalk-bizmessage)) to handle requests.
* **[Azure API Management](https://azure.microsoft.com/services/api-management/?WT.mc_id=dotnet-58531-juyoo)**: It aggregates all the requests from the sender and the responses from the NHN Notification Services ([SMS/MMS](https://www.toast.com/kr/service/notification/sms) and [KakaoTalk messages](https://www.toast.com/kr/service/notification/kakaotalk-bizmessage)).
* **[OpenAPI](https://github.com/OAI/OpenAPI-Specification/blob/main/versions/3.0.1.md)**: It is a contract between [Azure API Management](https://azure.microsoft.com/services/api-management/?WT.mc_id=dotnet-58531-juyoo) and [Azure Functions](https://azure.microsoft.com/services/functions/?WT.mc_id=dotnet-58531-juyoo) Proxy, and between [Custom Connector](https://docs.microsoft.com/connectors/custom-connectors/?WT.mc_id=dotnet-58531-juyoo) and [Azure API Management](https://azure.microsoft.com/services/api-management/?WT.mc_id=dotnet-58531-juyoo).
* **[Azure Monitor](https://azure.microsoft.com/services/monitor/?WT.mc_id=dotnet-58531-juyoo)**: [Azure Monitor](https://azure.microsoft.com/services/monitor/?WT.mc_id=dotnet-58531-juyoo) includes [Azure Application Insights](https://docs.microsoft.com/azure/azure-monitor/app/app-insights-overview?WT.mc_id=dotnet-58531-juyoo) and [Azure Log Analytics](https://docs.microsoft.com/azure/azure-monitor/logs/log-analytics-overview?WT.mc_id=dotnet-58531-juyoo) to monitor/trace/observe requests/responses.


## Getting Started ##

### GitHub Secrets ###

Following GitHub Secrets are required for CI/CD pipelines:

* `TOAST_APPKEY`: AppKey that NHN Toast issues.
* `TOAST_SECRETKEY`: SecretKey that NHN Toast issues.
* `REQUEST_ID`: Any request ID that was used to send SMS messages.
* `SENDER_NO`: Any registered and verified sender number for integration tests.
* `RECIPIENT_NO`: Recipient mobile phone number for integration tests. Make sure to use your mobile phone number, instead of random number.


### Local Development ###

TBD


### Initial Autopilot to Azure ###

TBD


### Initial Autopilot to NHN Cloud ###

TBD
