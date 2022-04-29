param name string
param apiMgmtPublisherName string
param apiMgmtPublisherEmail string
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

module provisonapimgmt './provision-apiManagement.bicep' = {
  name: 'Provision-ApiManagement'
  params: {
    name: name
    apiMgmtPublisherEmail: apiMgmtPublisherEmail
    apiMgmtPublisherName: apiMgmtPublisherName
    location: location
    env: env
  }
}

module provisionfncapp './provision-functionApp.bicep' = [for suffix in suffixes: {
  name: 'Provision-FunctionApp${suffix}'
  params: {
    name: name
    location: location
    suffix: suffix
    env: env
  }
  dependsOn:[
    provisonapimgmt
  ]
}]
