targetScope = 'subscription'

param name string
@allowed([
    'Australia Central'
    'Australia East'
    'Australia Southeast'
    'Brazil South'
    'Canada Central'
    'Canada East'
    'Central India'
    'Central US'
    'East Asia'
    'East US'
    'East US 2'
    'France Central'
    'Germany West Central'
    'Japan East'
    'Japan West'
    'Jio India West'
    'Korea Central'
    'Korea South'
    'North Central US'
    'North Europe'
    'Norway East'
    'South Africa North'
    'South Central US'
    'South India'
    'Southeast Asia'
    'Sweden Central'
    'Switzerland North'
    'UAE North'
    'UK South'
    'UK West'
    'West Central US'
    'West Europe'
    'West India'
    'West US'
    'West US 2'
    'West US 3'
])
param location string = 'Korea Central'
param env string = 'dev'

param apiManagementPublisherName string
param apiManagementPublisherEmail string
param deploymentScriptAzureCliVersion string = '2.36.0'
param functionAppNames string = 'sms,sms-verify'

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

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
    name: format(metadata.longName, 'rg')
    location: location
}

module resources './main.bicep' = {
    name: 'Resources'
    scope: rg
    params: {
        name: name
        location: location
        suffixes: functionAppNames
        env: env
        apiMgmtPublisherName: apiManagementPublisherName
        apiMgmtPublisherEmail: apiManagementPublisherEmail
        deploymentScriptAzureCliVersion: deploymentScriptAzureCliVersion
    }
}
