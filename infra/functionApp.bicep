param name string
param suffix string = ''
param location string = resourceGroup().location
param env string = 'dev'

param storageAccountId string
param storageAccountName string
param appInsightsId string
param consumptionPlanId string

param functionIsLinux bool = false

@allowed([
    'Development'
    'Staging'
    'Production'
])
param functionEnvironment string = 'Production'

@allowed([
    'v3'
    'v4'
])
param functionExtensionVersion string = 'v4'

@allowed([
    'dotnet'
    'dotnet-isolated'
    'java'
    'node'
    'python'
    'poweshell'
])
param functionWorkerRuntime string = 'dotnet'

@allowed([
    // dotnet / dotnet-isolated
    'v6.0'
    // java
    'v8'
    'v11'
    // node.js
    'v12'
    'v14'
    'v16'
    // python
    'v3.7'
    'v3.8'
    'v3.9'
    // powershell
    'v7'
])
param functionWorkerVersion string = 'v6.0'

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
    longName: '{0}-${name}{1}-${env}-${locationCode}'
    shortName: replace('{0}${name}{1}${env}${locationCode}', '-', '')
}

var storage = {
    id: storageAccountId
    name: storageAccountName
}
var consumption = {
    id: consumptionPlanId
}
var appInsights = {
    id: appInsightsId
}
var linuxFxVersionMap = {
    'dotnet': ''
    'dotnet-isolated': ''
    'java': 'Java|{0}'
    'node': 'Node|{0}'
    'python': 'Python|{0}'
    'powershell': 'PowerShell|{0}'
}
var functionApp = {
    name: suffix == '' ? format(metadata.longName, 'fncapp', '') : format(metadata.longName, 'fncapp', '-${suffix}')
    location: location
    kind: functionIsLinux ? 'functionapp,linux' : 'functionapp'
    linuxFxVersion: functionIsLinux ? format(linuxFxVersionMap[functionWorkerRuntime], replace(functionWorkerVersion, 'v', '')) : ''
    netFrameworkVersion: 'v6.0'
    nodeVersion: ''
    javaVersion: !functionIsLinux && functionWorkerRuntime == 'java' ? replace(functionWorkerVersion, 'v', '') : null
    pythonVersion: ''
    powerShellVersion: !functionIsLinux && functionWorkerRuntime == 'powershell' ? replace(functionWorkerVersion, 'v', '~') : ''
    environment: functionEnvironment
    extensionVersion: replace(functionExtensionVersion, 'v', '~')
    workerRuntime: functionWorkerRuntime
}

resource fncapp 'Microsoft.Web/sites@2021-02-01' = {
    name: functionApp.name
    location: functionApp.location
    kind: functionApp.kind
    properties: {
        serverFarmId: consumption.id
        httpsOnly: true
        siteConfig: {
            linuxFxVersion: functionApp.linuxFxVersion
            netFrameworkVersion: functionApp.netFrameworkVersion
            nodeVersion: functionApp.nodeVersion
            javaVersion: functionApp.javaVersion
            pythonVersion: functionApp.pythonVersion
            powerShellVersion: functionApp.powerShellVersion
            appSettings: [
                // Common Settings
                {
                    name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
                    value: '${reference(appInsights.id, '2020-02-02', 'Full').properties.InstrumentationKey}'
                }
                {
                    name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
                    value: '${reference(appInsights.id, '2020-02-02', 'Full').properties.connectionString}'
                }
                {
                    name: 'AZURE_FUNCTIONS_ENVIRONMENT'
                    value: functionApp.environment
                }
                {
                    name: 'AzureWebJobsStorage'
                    value: 'DefaultEndpointsProtocol=https;AccountName=${storage.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storage.id, '2021-06-01').keys[0].value}'
                }
                {
                    name: 'FUNCTION_APP_EDIT_MODE'
                    value: 'readonly'
                }
                {
                    name: 'FUNCTIONS_EXTENSION_VERSION'
                    value: functionApp.extensionVersion
                }
                {
                    name: 'FUNCTIONS_WORKER_RUNTIME'
                    value: functionApp.workerRuntime
                }
                {
                    name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
                    value: 'DefaultEndpointsProtocol=https;AccountName=${storage.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storage.id, '2021-06-01').keys[0].value}'
                }
                {
                    name: 'WEBSITE_CONTENTSHARE'
                    value: functionApp.name
                }
                // OpenAPI
                {
                    name: 'OpenApi__Version'
                    value: 'v3'
                }
                {
                    name: 'OpenApi__HostNames'
                    value: 'https://${functionApp.name}.azurewebsites.net/api'
                }
                // NHN Toast
                {
                    name: 'Toast__BaseUrl'
                    value: 'https://api-sms.cloud.toast.com/'
                }
                {
                    name: 'Toast__Version'
                    value: 'v3.0'
                }
            ]
        }
    }
}

output id string = fncapp.id
output name string = fncapp.name
