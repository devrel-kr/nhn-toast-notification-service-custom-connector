param name string
param suffix string = ''
param location string = resourceGroup().location
param env string = 'dev'

@allowed([
    'Standard_LRS'
    'Standard_ZRS'    
    'Standard_GRS'
    'Standard_GZRS'
    'Standard_RAGRS'
    'Standard_RAGZRS'
    'Premium_LRS'
    'Premium_ZRS'
])
param storageAccountSku string = 'Standard_LRS'

// Array item should be in the form of:
// {
//     name: '<container_name>'
//     publicAccess: '<None|Blob|Container>'
// }
param storageAccountBlobContainers array = []
param storageAccountTables array = []

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
    name: suffix == '' ? format(metadata.shortName, 'st', '') : format(metadata.shortName, 'st', replace(suffix, '-', ''))
    location: location
    sku: storageAccountSku
    blob: {
        containers: storageAccountBlobContainers
    }
    table: {
        tables: storageAccountTables
    }
}

resource st 'Microsoft.Storage/storageAccounts@2021-06-01' = {
    name: storage.name
    location: storage.location
    kind: 'StorageV2'
    sku: {
        name: storage.sku
    }
    properties: {
        supportsHttpsTrafficOnly: true
    }
}

resource stblob 'Microsoft.Storage/storageAccounts/blobServices@2021-06-01' = if (length(storage.blob.containers) > 0) {
    name: '${st.name}/default'
    properties: {
        deleteRetentionPolicy: {
            enabled: false
        }
        cors: {
            corsRules: [
                {
                    allowedOrigins: [
                        'https://flow.microsoft.com'
                        'https://asia.flow.microsoft.com'
                        'https://korea.flow.microsoft.com'
                    ]
                    allowedMethods: [
                        'GET'
                    ]
                    allowedHeaders: [
                        '*'
                    ]
                    exposedHeaders: [
                        '*'
                    ]
                    maxAgeInSeconds: 0
                }
            ]
        }
    }
}

resource stblobcontainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-06-01' = [for container in storage.blob.containers: if (length(storage.blob.containers) > 0) {
    name: '${stblob.name}/${container.name}'
    properties: {
        immutableStorageWithVersioning: {
            enabled: false
        }
        defaultEncryptionScope: '$account-encryption-key'
        denyEncryptionScopeOverride: false
        publicAccess: container.publicAccess
    }
}]

resource sttable 'Microsoft.Storage/storageAccounts/tableServices@2021-06-01' = if (length(storage.table.tables) > 0) {
    name: '${st.name}/default'
}

resource sttabletable 'Microsoft.Storage/storageAccounts/tableServices/tables@2021-06-01' = [for table in storage.table.tables: if (length(storage.table.tables) > 0) {
    name: '${sttable.name}/${table}'
}]

output id string = st.id
output name string = st.name
