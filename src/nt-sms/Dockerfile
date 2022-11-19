FROM mcr.microsoft.com/azure-functions/dotnet:4

LABEL org.devrel-kr.nhn-toast-notification-service-custom-connector.author="Justin Yoo"
LABEL org.devrel-kr.nhn-toast-notification-service-custom-connector.profile="https://twitter.com/devrelkorea"
LABEL org.devrel-kr.nhn-toast-notification-service-custom-connector.projectUrl="https://github.com/devrel-kr/nhn-toast-notification-service-custom-connector"
LABEL org.devrel-kr.nhn-toast-notification-service-custom-connector.repositoryUrl="https://github.com/devrel-kr/nhn-toast-notification-service-custom-connector"
LABEL org.devrel-kr.nhn-toast-notification-service-custom-connector.license="https://raw.githubusercontent.com/devrel-kr/nhn-toast-notification-service-custom-connector/main/LICENSE"
LABEL description="This API works as a facade to NHN Toast Notification Services - SMS API.\n\n \
For more details to the API, please visit the [NHN Toast document page](https://docs.toast.com/ko/Notification/SMS/ko/api-guide/)."

RUN mkdir -p /home/site/wwwroot

COPY . /home/site/wwwroot
