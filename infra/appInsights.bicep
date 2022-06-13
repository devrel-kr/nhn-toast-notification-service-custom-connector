param name string
param suffix string = ''
param location string = resourceGroup().location
param env string = 'dev'

@allowed([
    'web'
    'other'
])
param appInsightsType string = 'web'

@allowed([
    'ApplicationInsights'
    'ApplicationInsightsWithDiagnosticSettings'
    'LogAnalytics'
])
param appInsightsIngestionMode string = 'LogAnalytics'

param workspaceId string

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

var workspace = {
    id: workspaceId
}
var appInsights = {
    name: suffix == '' ? format(metadata.longName, 'appins', '') : format(metadata.longName, 'appins', '-${suffix}')
    location: location
    appType: appInsightsType
    ingestionMode: appInsightsIngestionMode
}

resource appins 'Microsoft.Insights/components@2020-02-02' = {
    name: appInsights.name
    location: appInsights.location
    kind: 'web'
    properties: {
        Application_Type: appInsights.appType
        Flow_Type: 'Bluefield'
        IngestionMode: appInsights.ingestionMode
        Request_Source: 'rest'
        WorkspaceResourceId: workspace.id
    }
}

output id string = appins.id
output name string = appins.name
output instrumentationKey string = appins.properties.InstrumentationKey
