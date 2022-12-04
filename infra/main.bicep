param name string
param location string = resourceGroup().location
param env string = 'dev'
param gitHubRepository string = 'devrel-kr/nhn-toast-notification-service-custom-connector'
param gitHubBranch string = 'main'
param apiMgmtPublisherName string
param apiMgmtPublisherEmail string
@allowed([
    'Device'
    'ForeignGroup'
    'Group'
    'ServicePrincipal'
    'User'
])
param deploymentScriptPrincipalType string = 'ServicePrincipal'
param deploymentScriptAzureCliVersion string = '2.40.0'
param apps array

module apim './provision-apiManagement.bicep' = {
    name: 'Provision-ApiManagement'
    params: {
        name: name
        location: location
        env: env
        apiMgmtPublisherEmail: apiMgmtPublisherEmail
        apiMgmtPublisherName: apiMgmtPublisherName
        apiMgmtPolicyFormat: 'xml-link'
        apiMgmtPolicyValue: 'https://raw.githubusercontent.com/${gitHubRepository}/${gitHubBranch}/infra/apim-global-policy.xml'
    }
}

module fncapps './provision-functionApp.bicep' = [for (app, index) in apps: {
    name: 'Provision-FunctionApp_${app.suffix}'
    dependsOn: [
        apim
    ]
    params: {
        name: name
        location: location
        suffix: app.suffix
        env: env
    }
}]

module apis './provision-apiManagementApi.bicep' = [for (app, index) in apps: {
    name: 'ApiManagementApi_${app.suffix}'
    dependsOn: [
        apim
        fncapps
    ]
    params: {
        name: name
        location: location
        apiMgmtNameValueName: 'X_FUNCTIONS_KEY_${replace(toUpper(app.apiName), '-', '_')}'
        apiMgmtNameValueDisplayName: 'X_FUNCTIONS_KEY_${replace(toUpper(app.apiName), '-', '_')}'
        apiMgmtNameValueValue: 'to_be_replaced'
        apiMgmtApiName: app.apiName
        apiMgmtApiDisplayName: app.apiName
        apiMgmtApiDescription: app.apiName
        apiMgmtApiServiceUrl: 'https://fncapp-${name}-${app.suffix}.azurewebsites.net/api'
        apiMgmtApiPath: app.apiPath
        apiMgmtApiFormat: app.apiFormat
        apiMgmtApiValue: 'https://raw.githubusercontent.com/${gitHubRepository}/${gitHubBranch}/infra/openapi-${toLower(app.apiName)}.${app.apiExtension}'
        apiMgmtApiPolicyFormat: 'xml-link'
        apiMgmtApiPolicyValue: 'https://raw.githubusercontent.com/${gitHubRepository}/${gitHubBranch}/infra/apim-api-policy-${toLower(app.apiName)}.xml'
    }
}]

module uai './deploymentScript.bicep' = {
    name: 'Provision-DeploymentScript'
    dependsOn: [
        apim
        fncapps
    ]
    params: {
        name: name
        location: location
        env: env
        principalType: deploymentScriptPrincipalType
        azureCliVersion: deploymentScriptAzureCliVersion
        functionAppSuffixes: suffixes
    }
}
