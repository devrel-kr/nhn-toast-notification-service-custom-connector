param name string
param location string = resourceGroup().location
param suffixes string = 'sms,sms-verify'
param env string = 'dev'

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
param deploymentScriptAzureCliVersion string = '2.36.0'

module apim './provision-apiManagement.bicep' = {
    name: 'Provision-ApiManagement'
    params: {
        name: name
        location: location
        env: env
        apiMgmtPublisherEmail: apiMgmtPublisherEmail
        apiMgmtPublisherName: apiMgmtPublisherName
        apiMgmtPolicyFormat: 'xml-link'
        apiMgmtPolicyValue: 'https://raw.githubusercontent.com/devrel-kr/nhn-toast-notification-service-custom-connector/main/infra/apim-global-policy.xml'
    }
}

module fncapps './provision-functionApp.bicep' = [for suffix in split(suffixes, ','): {
    name: 'Provision-FunctionApp_${suffix}'
    dependsOn: [
        apim
    ]
    params: {
        name: name
        location: location
        suffix: suffix
        env: env
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
