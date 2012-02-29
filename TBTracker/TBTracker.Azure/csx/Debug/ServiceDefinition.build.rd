<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="TBTracker.Azure" generation="1" functional="0" release="0" Id="042615c7-527e-42de-bde0-57117f4efff4" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="TBTracker.AzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="TBTracker:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/TBTracker.Azure/TBTracker.AzureGroup/LB:TBTracker:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="TBTracker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/TBTracker.Azure/TBTracker.AzureGroup/MapTBTracker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="TBTrackerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/TBTracker.Azure/TBTracker.AzureGroup/MapTBTrackerInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:TBTracker:Endpoint1">
          <toPorts>
            <inPortMoniker name="/TBTracker.Azure/TBTracker.AzureGroup/TBTracker/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapTBTracker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/TBTracker.Azure/TBTracker.AzureGroup/TBTracker/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapTBTrackerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/TBTracker.Azure/TBTracker.AzureGroup/TBTrackerInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="TBTracker" generation="1" functional="0" release="0" software="C:\Users\Jason\Documents\ic2012\TBTracker\TBTracker.Azure\csx\Debug\roles\TBTracker" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;TBTracker&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;TBTracker&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/TBTracker.Azure/TBTracker.AzureGroup/TBTrackerInstances" />
            <sCSPolicyFaultDomainMoniker name="/TBTracker.Azure/TBTracker.AzureGroup/TBTrackerFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="TBTrackerFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="TBTrackerInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="c828821c-ae68-4913-b557-40a9bea0b287" ref="Microsoft.RedDog.Contract\ServiceContract\TBTracker.AzureContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="daef8d67-c1e6-4e2f-8192-8216db253d6d" ref="Microsoft.RedDog.Contract\Interface\TBTracker:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/TBTracker.Azure/TBTracker.AzureGroup/TBTracker:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>