FROM mcr.microsoft.com/windows/servercore:ltsc2016
ADD https://raw.githubusercontent.com/microsoft/service-fabric-scripts-and-templates/master/docker/service-fabric-reliableservices-windowsservercore/InstallPreReq.ps1 /
RUN powershell -File C:\InstallPreReq.ps1
RUN setx PATH "%PATH%;C:\sffabricbin;C:\sffabricruntimeload" /M
ADD publish/ /
CMD C:\init.bat