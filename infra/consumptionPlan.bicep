param name string
param suffix string = ''
param location string = resourceGroup().location
param env string = 'dev'

param consumptionPlanIsLinux bool = false

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

var consumption = {
    name: suffix == '' ? format(metadata.longName, 'csplan', '') : format(metadata.longName, 'csplan', '-${suffix}')
    location: location
    isLinux: consumptionPlanIsLinux
}

resource csplan 'Microsoft.Web/serverfarms@2021-02-01' = {
    name: consumption.name
    location: consumption.location
    kind: 'functionApp'
    sku: {
        name: 'Y1'
        tier: 'Dynamic'
    }
    properties: {
        reserved: consumption.isLinux
    }
}

output id string = csplan.id
output name string = csplan.name
