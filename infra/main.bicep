param name string
param apiMgmtPublisherName string
param apiMgmtPublisherEmail string

param location string = resourceGroup().location
param suffix string = ''
param suffixNames array = [
  ''
  '-verify'
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

module provisionfncapp './provision-functionApp.bicep' = [for suffixName in suffixNames: {
  name: 'Provision-FunctionApp${suffix}${suffixName}'
  params: {
    name: name
    location: location
    suffix: '${suffix}${suffixName}'
    env: env
  }
  dependsOn:[
    provisonapimgmt
  ]
}]
