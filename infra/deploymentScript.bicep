param name string
param location string = resourceGroup().location
param env string = 'dev'

@allowed([
    'Device'
    'ForeignGroup'
    'Group'
    'ServicePrincipal'
    'User'
])
param principalType string = 'ServicePrincipal'
param azureCliVersion string = '2.36.0'
param functionAppSuffixes string

var locationCodeMap = {
    australiacentral: 'auc'
    'Australia Central': 'auc'
    australiaeast: 'aue'
    'Australia East': 'aue'
    australiasoutheast: 'ause'
    'Australia Southeast': 'ause'
    brazilsouth: 'brs'
    'Brazil South': 'brs'
    canadacentral: 'cac'
    'Canada Central': 'cac'
    canadaeast: 'cae'
    'Canada East': 'cae'
    centralindia: 'cin'
    'Central India': 'cin'
    centralus: 'cus'
    'Central US': 'cus'
    eastasia: 'ea'
    'East Asia': 'ea'
    eastus: 'eus'
    'East US': 'eus'
    eastus2: 'eus2'
    'East US 2': 'eus2'
    francecentral: 'frc'
    'France Central': 'frc'
    germanywestcentral: 'dewc'
    'Germany West Central': 'dewc'
    japaneast: 'jpe'
    'Japan East': 'jpe'
    japanwest: 'jpw'
    'Japan West': 'jpw'
    jioindianorthwest: 'jinw'
    'Jio India North West': 'jinw'
    koreacentral: 'krc'
    'Korea Central': 'krc'
    koreasouth: 'krs'
    'Korea South': 'krs'
    northcentralus: 'ncus'
    'North Central US': 'ncus'
    northeurope: 'neu'
    'North Europe': 'neu'
    norwayeast: 'noe'
    'Norway East': 'noe'
    southafricanorth: 'zan'
    'South Africa North': 'zan'
    southcentralus: 'scus'
    'South Central US': 'scus'
    southindia: 'sin'
    'South India': 'sin'
    southeastasia: 'sea'
    'Southeast Asia': 'sea'
    swedencentral: 'sec'
    'Sweden Central': 'sec'
    switzerlandnorth: 'chn'
    'Switzerland North': 'chn'
    uaenorth: 'uaen'
    'UAE North': 'uaen'
    uksouth: 'uks'
    'UK South': 'uks'
    ukwest: 'ukw'
    'UK West': 'ukw'
    westcentralus: 'wcus'
    'West Central US': 'wcus'
    westeurope: 'weu'
    'West Europe': 'weu'
    westindia: 'win'
    'West India': 'win'
    westus: 'wus'
    'West US': 'wus'
    westus2: 'wus2'
    'West US 2': 'wus2'
    westus3: 'wus3'
    'West US 3': 'wus3'
}
var locationCode = locationCodeMap[location]

var metadata = {
    longName: '{0}-${name}-${env}-${locationCode}'
    shortName: replace('{0}${name}{1}${env}${locationCode}', '-', '')
}

var userAssignedIdentity = {
    name: format(metadata.longName, 'spn')
    location: location
}

resource uai 'Microsoft.ManagedIdentity/userAssignedIdentities@2018-11-30' = {
    name: userAssignedIdentity.name
    location: userAssignedIdentity.location
}

var roleAssignment = {
    name: guid(resourceGroup().id, 'contributor')
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions', 'b24988ac-6180-42a0-ab88-20f7382dd24c')
    principalType: principalType
}

resource role 'Microsoft.Authorization/roleAssignments@2020-10-01-preview' = {
    name: roleAssignment.name
    scope: resourceGroup()
    properties: {
        roleDefinitionId: roleAssignment.roleDefinitionId
        principalId: uai.properties.principalId
        principalType: roleAssignment.principalType
    }
}

var deploymentScript = {
    name: format(metadata.longName, 'depscrpt')
    location: location
    resourceName: name
    environmentCode: env
    locationCode: locationCode
    containerGroupName: format(metadata.longName, 'contgrp')
    functionAppSuffixes: functionAppSuffixes
    azureCliVersion: azureCliVersion
    scriptUri: 'https://raw.githubusercontent.com/devrel-kr/nhn-toast-notification-service-custom-connector/main/infra/setup-apim.sh'
}

resource ds 'Microsoft.Resources/deploymentScripts@2020-10-01' = {
    name: deploymentScript.name
    location: deploymentScript.location
    dependsOn: [
        role
    ]
    kind: 'AzureCLI'
    identity: {
        type: 'UserAssigned'
        userAssignedIdentities: {
            '${uai.id}': {}
        }
    }
    properties: {
        azCliVersion: deploymentScript.azureCliVersion
        containerSettings: {
            containerGroupName: deploymentScript.containerGroupName
        }
        environmentVariables: [
            {
                name: 'AZ_APP_SUFFIXES'
                value: deploymentScript.functionAppSuffixes
            }
            {
                name: 'AZ_RESOURCE_NAME'
                value: deploymentScript.resourceName
            }
            {
                name: 'AZ_ENVIRONMENT_CODE'
                value: deploymentScript.environmentCode
            }
            {
                name: 'AZ_LOCATION_CODE'
                value: deploymentScript.locationCode
            }
        ]
        primaryScriptUri: deploymentScript.scriptUri
        cleanupPreference: 'OnExpiration'
        retentionInterval: 'P1D'
    }
}
