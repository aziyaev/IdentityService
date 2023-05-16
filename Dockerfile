# ���������� ������� ����� .NET SDK ��� ������
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

# �������� csproj ���� � ��������������� �����������
COPY IdentityService.csproj .
RUN dotnet restore

# �������� ��������� ����� ������� � �������� ����������
COPY . .
RUN dotnet publish -c Release -o out

# ���������� ������� ����� ASP.NET ��� �������
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

WORKDIR /app
COPY --from=build /app/out .

# ������������� ���������� ���������
ENV ASPNETCORE_URLS=http://*:8000

# ��������� ������
ENTRYPOINT ["dotnet", "IdentityService.dll"]