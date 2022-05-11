param name string
param location string = resourceGroup().location
param suffixes array = [
    'sms'
    'sms-verify'
]
@allowed([
    'dev'
    'test'
    'prod'

    'kdy'
    'kms'
    'lsw'
    'pjm'
])
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
param deploymentScriptAzureCliVersion string = '2.33.1'

module apim './provision-apiManagement.bicep' = {
    name: 'Provision-ApiManagement'
    params: {
        name: name
        apiMgmtPublisherEmail: apiMgmtPublisherEmail
        apiMgmtPublisherName: apiMgmtPublisherName
        location: location
        env: env
    }
}

module fncapps './provision-functionApp.bicep' = [for suffix in suffixes: {
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
        principalType: deploymentScriptPrincipalType
        azureCliVersion: deploymentScriptAzureCliVersion
        functionAppSuffixes: suffixes
    }
}
