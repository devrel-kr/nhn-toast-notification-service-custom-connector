resource storageAccount 'Microsoft.Storage/storageAccounts@2021-08-01' = {
  name: 'nhntoaststorageaccount'
  location: 'koreacentral'
  sku: {
    name: 'Standard_LRS'
    tier: 'Standard'
  }
  kind: 'Storage'
  properties: {
    allowBlobPublicAccess: true
    encryption: {
      keySource: 'Microsoft.Storage'
      services: {
        blob: {
          enabled: true
          keyType: 'Account'
        }
        file: {
          enabled: true
          keyType: 'Account'
        }
      }
    }
    minimumTlsVersion: 'TLS1_2'
    networkAcls: {
      bypass: 'AzureServices'
      defaultAction: 'Allow'
      ipRules: []
      virtualNetworkRules: []
    }
    supportsHttpsTrafficOnly: true
  }
}

output storageAccountName string = storageAccount.name
